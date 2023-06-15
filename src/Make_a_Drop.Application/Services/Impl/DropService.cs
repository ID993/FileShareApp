using AutoMapper;
using Make_a_Drop.Application.Models.Drop;
using Make_a_Drop.Core.Entities;
using Make_a_Drop.Core.Entities.Identity;
using Make_a_Drop.DataAccess.Repositories;
using Make_a_Drop.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Make_a_Drop.Application.Helpers;
using Make_a_Drop.Application.Exceptions;
using Make_a_Drop.Application.Common.Email;
using Make_a_Drop.Application.Models.Collaboration;

namespace Make_a_Drop.Application.Services.Impl
{
    public class DropService : IDropService
    {
        private readonly IMapper _mapper;
        private readonly IDropRepository _dropRepository;
        private readonly IDropFileRepository _dropFileRepository;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IClaimService _claimService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ICollaborationRepository _collaborationRepository;


        public DropService(IDropRepository dropRepository, IDropFileRepository dropFileRepository,
            IClaimService claimService, IMapper mapper, UserManager<ApplicationUser> userManager, 
            IFirebaseStorageService firebaseStorageService, IEmailService emailService, ICollaborationRepository collaborationRepository)
        {
            _dropRepository = dropRepository;
            _dropFileRepository = dropFileRepository;
            _claimService = claimService;
            _mapper = mapper;
            _userManager = userManager;
            _firebaseStorageService = firebaseStorageService;
            _emailService = emailService;
            _collaborationRepository = collaborationRepository;   
        }


        public async Task<DropModel> CreateAsync(CreateDropModel createDropModel, 
            CancellationToken cancellationToken = default)
        {      
            var userId = _claimService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var rawSecretKey = createDropModel.SecretKey;

            var drop = _mapper.Map<Drop>(createDropModel);
            drop.User = user;
            drop.SecretKey = SecretKeyHash.Hash(rawSecretKey);

            var addedDrop = await _dropRepository.AddAsync(drop);

            return new DropModel
            {
                Id = addedDrop.Id,
            };
        }

        public async Task<List<DropResponseModel>> GetAllAsync()
        {
            var currentUserId = _claimService.GetUserId() ?? throw new NotFoundException("User not found.");
            var dropList = await _dropRepository.GetAllAsync(dl => dl.User.Id == currentUserId);
            
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public async Task<List<DropResponseModel>> GetAllByCollaborationIdAsync(Guid collaborationId)
        {
            var dropList = await _dropRepository.GetAllAsync(dl => dl.Collaboration.Id == collaborationId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public async Task<DropResponseModel> GetByIdAsync(Guid? guid)
        {
            var drop = await _dropRepository.GetFirstAsync(d => d.Id == guid) ?? throw new NotFoundException("Drop not found");            
            return _mapper.Map<DropResponseModel>(drop);
        }

        public async Task<DropResponseModel> FindByIdAsync(Guid? guid)
        {
            var drop = await _dropRepository.FindByIdAsync(d => d.Id == guid) ?? throw new NotFoundException("Drop not found");     
            return _mapper.Map<DropResponseModel>(drop);
        }


        public async Task<DropResponseModel> GetByNameAsync(string name)
        {
            var drop = await _dropRepository.GetFirstAsync(du => du.Name == name);
            return _mapper.Map<DropResponseModel>(drop);
        }

        public async Task<bool> ExistByNameAsync(string name)
        {
            var drop = await _dropRepository.GetFirstAsync(du => du.Name == name);
            return drop != null;
        }

        public async Task<DropResponseModel> UpdateAsync(DropResponseModel dropResponseModel)
        {
            var drop = await _dropRepository.FindByIdAsync(d => d.Id == dropResponseModel.Id) ?? throw new NotFoundException("Drop not found");
            drop.ExpirationDate = dropResponseModel.ExpirationDate;
            await _dropRepository.UpdateAsync(drop);
            return _mapper.Map<DropResponseModel>(drop);
        }
        
        public async Task<int> UploadFiles(UploadModel model)
        { 
            var checkDrop = await _dropRepository.GetByNameAsync(d => d.Name == model.Name);
            if (checkDrop != null)
            {
                return 0;
            }
            var user = await _userManager.FindByIdAsync(_claimService.GetUserId());
            var collaboration = model.CollaborationId.HasValue
            ? await _collaborationRepository.FindByIdAsync(d => d.Id == model.CollaborationId.Value) : null;

            if (collaboration != null)
            {
                foreach (var users in collaboration.Users)
                {
                    var emailBody = $"Hello {users.FirstName} {users.LastName}.<br>" +
                                    $"A new Drop just dropped in collaboration '{collaboration.Name}'.<br><br>" +
                                    $"<b>Name:</b> {model.Name}<br><br>MAD Team";
                    await _emailService.SendEmailAsync(EmailMessage.Create(users.Email, emailBody, "MakeADrop - NewDrop"));
                }
            }

            var dropModel = await _dropRepository.AddAsync(new Drop
            {
                Name = model.Name,
                SecretKey = SecretKeyHash.Hash(model.SecretKey),
                Size = model.File.Sum(f => f.Length),
                User = user,
                Collaboration = collaboration,
                ExpirationDate = DateTime.Now.AddDays(7)
            });

            foreach (var file in model.File)
            {
                using (var stream = file.OpenReadStream())
                {
                    var fileName = await _firebaseStorageService.UploadFileAsync(stream, file.FileName);
                    var dropFiles = new DropFile
                    {
                        FileName = fileName,
                        Size = file.Length,
                        Drop = dropModel
                    };
                    await _dropFileRepository.AddAsync(dropFiles);
                };             
            }
            return 1;
        }

        public async Task IfOwnerAnonDelete(Guid? guid)
        {
            var drop = await _dropRepository.FindByIdAsync(d => d.Id == guid);
            if (drop.User == null)
            {
                await DeleteAsync(guid);
            }
        }

        public async Task<Drop> DeleteAsync(Guid? id)
        {
            var drop = await _dropRepository.GetFirstAsync(d => d.Id == id) ?? throw new NotFoundException("Drop not found");
            var dropFiles = await _dropFileRepository.GetAllAsync(df => df.Drop.Id == id);
            foreach (var item in dropFiles)
            {
                await _firebaseStorageService.DeleteFileAsync(item.FileName);
                await _dropFileRepository.DeleteAsync(item);
            }
            return await _dropRepository.DeleteAsync(drop);            
        }

        public async Task DeleteExpiredAsync()
        {
            var dropList = await _dropRepository.GetExpired();
            foreach (var item in dropList)
            {
                Console.WriteLine($"\nDrop: {item.Name} deleted at {DateTime.UtcNow}\n");
                await DeleteAsync(item.Id);
            }
        }

        public List<DropResponseModel> OrderByNameAsc()
        {
            var currentUserId = _claimService.GetUserId();
            var dropList = _dropRepository.OrderByNameAsc(dl => dl.User.Id == currentUserId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> OrderByNameDesc()
        {
            var currentUserId = _claimService.GetUserId();
            var dropList = _dropRepository.OrderByNameDesc(dl => dl.User.Id == currentUserId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> OrderBySizeAsc()
        {
            var currentUserId = _claimService.GetUserId();
            var dropList = _dropRepository.OrderBySizeAsc(dl => dl.User.Id == currentUserId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> OrderBySizeDesc()
        {
            var currentUserId = _claimService.GetUserId();
            var dropList = _dropRepository.OrderBySizeDesc(dl => dl.User.Id == currentUserId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> SearchByName(string name)
        {
            var currentUserId = _claimService.GetUserId();
            var dropList = _dropRepository.SearchByName(dl => dl.User.Id == currentUserId, name);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> PaginatedList(int page, int pageSize)
        {
            var currentUserId = _claimService.GetUserId();
            var dropList = _dropRepository.PaginatedList(dl => dl.User.Id == currentUserId, page, pageSize);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> OrderByNameAscCol(Guid colId)
        {
            var dropList = _dropRepository.OrderByNameAsc(dl => dl.Collaboration.Id == colId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> OrderByNameDescCol(Guid colId)
        {
            var dropList = _dropRepository.OrderByNameDesc(dl => dl.Collaboration.Id == colId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> OrderBySizeAscCol(Guid colId)
        {
            var dropList = _dropRepository.OrderBySizeAsc(dl => dl.Collaboration.Id == colId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> OrderBySizeDescCol(Guid colId)
        {
            var dropList = _dropRepository.OrderBySizeDesc(dl => dl.Collaboration.Id == colId);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> SearchByNameCol(Guid colId, string name)
        {
            var dropList = _dropRepository.SearchByName(dl => dl.Collaboration.Id == colId, name);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

        public List<DropResponseModel> PaginatedListCol(Guid colId, int page, int pageSize)
        {
            var dropList = _dropRepository.PaginatedList(dl => dl.Collaboration.Id == colId, page, pageSize);
            return _mapper.Map<List<DropResponseModel>>(dropList);
        }

    }
}
