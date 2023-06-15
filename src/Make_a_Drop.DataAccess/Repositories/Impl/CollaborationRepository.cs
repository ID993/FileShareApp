using Make_a_Drop.Core.Entities;
using Make_a_Drop.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories.Impl
{
    public class CollaborationRepository : BaseRepository<Collaboration>, ICollaborationRepository
    {
        public CollaborationRepository(DatabaseContext context) : base(context) { }

        public async Task<Collaboration> FindByIdAsync(Expression<Func<Collaboration, bool>> predicate)
        {
            return await DbSet.Where(predicate)
                .Include(c => c.Drops)
                .Include(c => c.Users)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Collaboration>> FindAllByUserIdAsync(Expression<Func<Collaboration, bool>> predicate)
        {
            return await DbSet.Where(predicate)
                .Include(c => c.Drops)
                .Include(c => c.Users)
                .ToListAsync();
        }

        public async Task<Collaboration> GetFirstAsync(Expression<Func<Collaboration, bool>> predicate)
        {
            return await DbSet.Where(predicate).Include(c => c.Drops).FirstOrDefaultAsync();
        }

        public async Task<List<Collaboration>> FindAllAsync()
        {
            return await DbSet
                .Include(c => c.Drops)
                .Include(c => c.Users)
                .ToListAsync();
        }


        public IQueryable<Collaboration> OrderByNameAsc(Expression<Func<Collaboration, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderBy(d => d.Name);
        }

        public IQueryable<Collaboration> OrderByNameDesc(Expression<Func<Collaboration, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderByDescending(d => d.Name);
        }

        public IQueryable<Collaboration> SearchByName(Expression<Func<Collaboration, bool>> predicate, string name)
        {
            return DbSet.Where(predicate).Where(d => d.Name.Contains(name));
        }

        public IQueryable<Collaboration> PaginatedList(Expression<Func<Collaboration, bool>> predicate, int page, int pageSize)
        {
            return DbSet.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
