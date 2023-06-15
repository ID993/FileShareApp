using Make_a_Drop.Application.Models.Collaboration;

namespace Make_a_Drop.Application.Services
{
    public interface ICollaborationService
    {
        Task<CollaborationModel> CreateAsync(CreateCollaborationModel createCollaborationModel, CancellationToken cancellationToken = default);
        Task<List<CollaborationModel>> GetAllAsync();

        Task<List<CollaborationModel>> GetAllPartsAsync();
        Task<List<CollaborationModel>> FindAllAsync();
        Task<CollaborationModel> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<CollaborationModel> UpdateAsync(CollaborationModel collaborationModel);
        Task<CollaborationModel> AddUsers(CreateCollaborationModel createCollaborationModel, CancellationToken cancellationToken = default);

        List<CollaborationModel> OrderByNameAsc();

        List<CollaborationModel> OrderByNameDesc();

        List<CollaborationModel> SearchByName(string name);

        List<CollaborationModel> PaginatedList(int page, int pageSize);

        List<CollaborationModel> OrderByNameAscParts();

        List<CollaborationModel> OrderByNameDescParts();

        List<CollaborationModel> SearchByNameParts(string name);

        List<CollaborationModel> PaginatedListParts(int page, int pageSize);
    }
}
