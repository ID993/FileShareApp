using Make_a_Drop.Core.Entities;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories;

public interface IDropFileRepository : IBaseRepository<DropFile> 
{
    Task<DropFile> GetFirstAsync(Expression<Func<DropFile, bool>> predicate);
        
}

