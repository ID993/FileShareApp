using Make_a_Drop.Core.Entities;
using Make_a_Drop.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories.Impl
{
    public class DropRepository : BaseRepository<Drop>, IDropRepository
    {
        public DropRepository(DatabaseContext context) : base(context) { }

        public async Task<Drop> GetFirstAsync(Expression<Func<Drop, bool>> predicate)
        {
            return await DbSet.Where(predicate).Include(c => c.Comments).FirstOrDefaultAsync();
        }
        public async Task<List<Drop>> FindAllAsync()
        {
            return await DbSet.Include(d => d.User).ToListAsync();
        }
        public async Task<Drop> FindByIdAsync(Expression<Func<Drop, bool>> predicate)
        {
            return await DbSet.Where(predicate).Include(d => d.User).FirstOrDefaultAsync();
        }
        
        public async Task<List<Drop>> GetExpired()
        {
            return await DbSet.Where(d => d.ExpirationDate < DateTime.Now).ToListAsync();
        }

        public async Task<Drop> GetByNameAsync(Expression<Func<Drop, bool>> predicate)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync();
        }
        
        public IQueryable<Drop> OrderByNameAsc(Expression<Func<Drop, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderBy(d => d.Name);
        }

        public IQueryable<Drop> OrderByNameDesc(Expression<Func<Drop, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderByDescending(d => d.Name);
        }

        public IQueryable<Drop> OrderBySizeAsc(Expression<Func<Drop, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderBy(d => d.Size);
        }

        public IQueryable<Drop> OrderBySizeDesc(Expression<Func<Drop, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderByDescending(d => d.Size);
        }

        public IQueryable<Drop> SearchByName(Expression<Func<Drop, bool>> predicate, string name)
        {
            return DbSet.Where(predicate).Where(d => d.Name.Contains(name));
        }

        public IQueryable<Drop> PaginatedList(Expression<Func<Drop, bool>> predicate, int page, int pageSize)
        {
            return DbSet.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
