using Make_a_Drop.Core.Entities;
using Make_a_Drop.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories.Impl
{
    public class DropFileRepository : BaseRepository<DropFile>, IDropFileRepository
    {

        public DropFileRepository(DatabaseContext context) : base(context) { }

        public async Task<DropFile> GetFirstAsync(Expression<Func<DropFile, bool>> predicate)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync();
        }
    }
    
}
