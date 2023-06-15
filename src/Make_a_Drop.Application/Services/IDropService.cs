using Make_a_Drop.Application.Models.Drop;
using Make_a_Drop.Core.Entities;

namespace Make_a_Drop.Application.Services
{
    public interface IDropService
    {

        Task<DropModel> CreateAsync(CreateDropModel createDropModel,
             CancellationToken cancellationToken = default);
        Task<List<DropResponseModel>> GetAllAsync();

        Task<List<DropResponseModel>> GetAllByCollaborationIdAsync(Guid collaborationId);

        Task<int> UploadFiles(UploadModel model);

        Task<DropResponseModel> GetByNameAsync(string name);

        Task<bool> ExistByNameAsync(string name);

        Task<DropResponseModel> GetByIdAsync(Guid? id);

        Task<DropResponseModel> FindByIdAsync(Guid? id);

        Task<DropResponseModel> UpdateAsync(DropResponseModel dropResponseModel);

        Task IfOwnerAnonDelete(Guid? id);

        Task<Drop> DeleteAsync(Guid? id);

        Task DeleteExpiredAsync();

        List<DropResponseModel> OrderByNameAsc();

        List<DropResponseModel> OrderByNameDesc();

        List<DropResponseModel> OrderBySizeAsc();

        List<DropResponseModel> OrderBySizeDesc();

        List<DropResponseModel> SearchByName(string name);

        List<DropResponseModel> PaginatedList(int page, int pageSize);

        List<DropResponseModel> OrderByNameAscCol(Guid colId);

        List<DropResponseModel> OrderByNameDescCol(Guid colId);

        List<DropResponseModel> OrderBySizeAscCol(Guid colId);

        List<DropResponseModel> OrderBySizeDescCol(Guid colId);

        List<DropResponseModel> SearchByNameCol(Guid colId, string name);

        List<DropResponseModel> PaginatedListCol(Guid colId, int page, int pageSize);


    }
}
