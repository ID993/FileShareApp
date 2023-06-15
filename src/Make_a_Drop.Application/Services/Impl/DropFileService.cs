using AutoMapper;
using Make_a_Drop.Application.Exceptions;
using Make_a_Drop.Application.Models.DropFile;
using Make_a_Drop.Core.Entities;
using Make_a_Drop.DataAccess.Repositories;
using Make_a_Drop.Shared.Services;

namespace Make_a_Drop.Application.Services.Impl
{
    public class DropFileService : IDropFileService
    {
        
        private readonly IDropFileRepository _dropFileRepository;
        private readonly IDropRepository _dropRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;



        public DropFileService(IDropFileRepository dropFileRepository, IDropRepository dropRepository,
            IClaimService claimService, IMapper mapper)
        {
            _dropFileRepository = dropFileRepository;
            _dropRepository = dropRepository;
            _claimService = claimService;
            _mapper = mapper;

        }

        public async Task<DropFileModel> CreateAsync(CreateDropFileModel createDropFileModel, Guid guid,
            CancellationToken cancellationToken = default)
        {
            var drop = await _dropRepository.GetFirstAsync(d => d.Id == guid);
            var dropFile = _mapper.Map<DropFile>(createDropFileModel);
            dropFile.Drop = drop;

            var addedDropFile = await _dropFileRepository.AddAsync(dropFile);

            return new DropFileModel
            {
                Id = addedDropFile.Id,
            };
        }

        public async Task<DropFileModel> GetByIdAsync(Guid guid)
        {
            var dropFile = await _dropFileRepository.GetFirstAsync(df => df.Id == guid) ?? throw new NotFoundException("File not found");
            return _mapper.Map<DropFileModel>(dropFile);
        }


        public async Task<List<DropFileModel>> GetByDropIdAsync(Guid? guid)
        {
            var dropFiles = await _dropFileRepository.GetAllAsync(df => df.Drop.Id == guid);
            var files = _mapper.Map<IEnumerable<DropFileModel>>(dropFiles);
            return files.ToList();
        }

        public async Task DeleteAsync(Guid guid)
        {
            var dropFile = await _dropFileRepository.GetFirstAsync(df => df.Id == guid);
            _ = await _dropFileRepository.DeleteAsync(dropFile);
            
        }
    }
}
