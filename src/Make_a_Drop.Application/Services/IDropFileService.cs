using Make_a_Drop.Application.Models.DropFile;

namespace Make_a_Drop.Application.Services
{
    public interface IDropFileService
    {

        Task<DropFileModel> CreateAsync(CreateDropFileModel createDropModel, Guid guid,
             CancellationToken cancellationToken = default);

        Task<DropFileModel> GetByIdAsync(Guid id);

        Task<List<DropFileModel>> GetByDropIdAsync(Guid? id);

        Task DeleteAsync(Guid id);
    }
}
