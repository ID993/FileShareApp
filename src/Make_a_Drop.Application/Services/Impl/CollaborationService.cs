using AutoMapper;
using Make_a_Drop.Application.Common.Email;
using Make_a_Drop.Application.Exceptions;
using Make_a_Drop.Application.Models.Collaboration;
using Make_a_Drop.Core.Entities;
using Make_a_Drop.Core.Entities.Identity;
using Make_a_Drop.DataAccess.Repositories;
using Make_a_Drop.Shared.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace Make_a_Drop.Application.Services.Impl
{
    public class CollaborationService : ICollaborationService
    {
        private readonly IMapper _mapper;
        private readonly ICollaborationRepository _collaborationRepository;
        private readonly IClaimService _claimService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IDropRepository _dropRepository;

        public CollaborationService(ICollaborationRepository collaborationRepository, IClaimService claimService,
            IMapper mapper, UserManager<ApplicationUser> userManager, IEmailService emailService, IDropRepository dropRepository)
        {
            _collaborationRepository = collaborationRepository;
            _claimService = claimService;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _dropRepository = dropRepository;
        }

        public async Task<CollaborationModel> CreateAsync(CreateCollaborationModel createCollaborationModel, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(_claimService.GetUserId());
            ICollection<ApplicationUser> users = new Collection<ApplicationUser>();

            if (createCollaborationModel.Users != null)
            {
                var userEmails = createCollaborationModel.Users.Split(',');
                foreach (var u in userEmails)
                {
                    var userByEmail = await _userManager.FindByEmailAsync(u);
                    users.Add(userByEmail);
                    var emailBody = $"Hello {userByEmail.FirstName}   {userByEmail.LastName}.<br>" +
                                    $"You have became part of Make A Drop Collaboration - '{createCollaborationModel.Name}'.";
                    await _emailService.SendEmailAsync(EmailMessage.Create(userByEmail.Email, emailBody, "MakeADrop - Collaboration invitation"));
                }
            }

            var newCollabModel = new CollaborationModel
            {
                Name = createCollaborationModel.Name,
                Users = users,
                OwnerId = user.Id,
                Owner = user,
            };

            var collaboration = _mapper.Map<Collaboration>(newCollabModel);
            var newCollaboration = await _collaborationRepository.AddAsync(collaboration);
            newCollabModel.Id = newCollaboration.Id;

            return newCollabModel;
        }

        public async Task<List<CollaborationModel>> GetAllAsync()
        {
            var user = await _userManager.FindByIdAsync(_claimService.GetUserId());
            var collaborations = await _collaborationRepository.FindAllByUserIdAsync(c => c.OwnerId == user.Id);
            var collabs = _mapper.Map<List<CollaborationModel>>(collaborations);
            foreach (var c in collabs)
            {
                var owner = await _userManager.FindByIdAsync(c.OwnerId);
                c.Owner = owner;
            }
            return collabs;
        }

        public async Task<List<CollaborationModel>> GetAllPartsAsync()
        {
            var user = await _userManager.FindByIdAsync(_claimService.GetUserId());
            var collaborations = await _collaborationRepository.FindAllByUserIdAsync(c => c.Users.Contains(user));
            var collabs = _mapper.Map<List<CollaborationModel>>(collaborations);
            foreach (var c in collabs)
            {
                var owner = await _userManager.FindByIdAsync(c.OwnerId);
                c.Owner = owner;
            }
            return collabs;
        }

        public async Task<List<CollaborationModel>> FindAllAsync()
        {
            var collaborations = await _collaborationRepository.FindAllAsync();
            return _mapper.Map<List<CollaborationModel>>(collaborations);
        }

        public async Task<CollaborationModel> GetByIdAsync(Guid id)
        {
            var collaboration = await _collaborationRepository.FindByIdAsync(d => d.Id == id);
            var user = await _userManager.FindByIdAsync(collaboration.OwnerId);
            var collab = _mapper.Map<CollaborationModel>(collaboration);
            collab.Owner = user;
            return collab;
        }

        public async Task DeleteAsync(Guid id)
        {
            var collaboration = await _collaborationRepository.GetFirstAsync(d => d.Id == id);
            await _collaborationRepository.DeleteAsync(collaboration);
        }

        public async Task<CollaborationModel> UpdateAsync(CollaborationModel collaborationModel)
        {
            var collaboration = await _collaborationRepository.FindByIdAsync(d => d.Id == collaborationModel.Id) ?? throw new NotFoundException("Collaboration not found"); ;
            collaboration.Name = collaborationModel.Name;
            collaboration.Users = collaborationModel.Users;
            collaboration.Drops = collaborationModel.Drops;

            await _collaborationRepository.UpdateAsync(collaboration);
            return _mapper.Map<CollaborationModel>(collaboration);
        }

        public async Task<CollaborationModel> AddUsers(CreateCollaborationModel createCollaborationModel, CancellationToken cancellationToken = default)
        {
            var collaboration = await _collaborationRepository.FindByIdAsync(d => d.Id == createCollaborationModel.Id) ?? throw new NotFoundException("Collaboration not found"); ;
            ICollection<ApplicationUser> users = new Collection<ApplicationUser>();
            var userEmails = createCollaborationModel.Users.Split(',');

            foreach (var u in userEmails)
            {
                var userByEmail = await _userManager.FindByEmailAsync(u);
                collaboration.Users.Add(userByEmail);
                var emailBody = $"Hello {userByEmail.FirstName} {userByEmail.LastName},\n\n" +
                                $"You have became part of Make A Drop Collaboration - '{collaboration.Name}'.";
                await _emailService.SendEmailAsync(EmailMessage.Create(userByEmail.Email, emailBody, "MakeADrop - Collaboration invitation"));
            }

            await _collaborationRepository.UpdateAsync(collaboration);
            return _mapper.Map<CollaborationModel>(collaboration);
        }

        public List<CollaborationModel> OrderByNameAsc()
        {
            var userId = _claimService.GetUserId();
            var collabList = _collaborationRepository.OrderByNameAsc(cl => cl.OwnerId == userId);
            return _mapper.Map<List<CollaborationModel>>(collabList);
        }

        public List<CollaborationModel> OrderByNameDesc()
        {
            var userId = _claimService.GetUserId();
            var collabList = _collaborationRepository.OrderByNameDesc(cl => cl.OwnerId == userId);
            return _mapper.Map<List<CollaborationModel>>(collabList);
        }


        public List<CollaborationModel> SearchByName(string name)
        {
            var userId = _claimService.GetUserId();
            var collabList = _collaborationRepository.SearchByName(cl => cl.OwnerId == userId, name);
            return _mapper.Map<List<CollaborationModel>>(collabList);
        }

        public List<CollaborationModel> PaginatedList(int page, int pageSize)
        {
            var userId = _claimService.GetUserId();
            var collabList = _collaborationRepository.PaginatedList(cl => cl.OwnerId == userId, page, pageSize);
            return _mapper.Map<List<CollaborationModel>>(collabList);
        }
        public List<CollaborationModel> OrderByNameAscParts()
        {
            var user = _userManager.FindByIdAsync(_claimService.GetUserId()).Result;
            var collabList = _collaborationRepository.OrderByNameAsc(cl => cl.Users.Contains(user));

            return _mapper.Map<List<CollaborationModel>>(collabList);
        }

        public List<CollaborationModel> OrderByNameDescParts()
        {
            var user = _userManager.FindByIdAsync(_claimService.GetUserId()).Result;
            var collabList = _collaborationRepository.OrderByNameDesc(cl => cl.Users.Contains(user));

            return _mapper.Map<List<CollaborationModel>>(collabList);
        }

        public List<CollaborationModel> SearchByNameParts(string name)
        {
            var user = _userManager.FindByIdAsync(_claimService.GetUserId()).Result;
            var collabList = _collaborationRepository.SearchByName(cl => cl.Users.Contains(user), name);

            return _mapper.Map<List<CollaborationModel>>(collabList);
        }

        public List<CollaborationModel> PaginatedListParts(int page, int pageSize)
        {
            var user = _userManager.FindByIdAsync(_claimService.GetUserId()).Result;
            var collabList = _collaborationRepository.PaginatedList(cl => cl.Users.Contains(user), page, pageSize);
            return _mapper.Map<List<CollaborationModel>>(collabList);
        }
    }
}
