using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaApi.Data;
using VillaApi.Models;
using VillaApi.Repository.IRepository;

namespace VillaApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; //initialize to make generic db
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>(); //initialize here to make _db.Villas Into dbSet
        }

        public async Task CreateAsync(T entity) // the T is a placeholder which work for DataBase Table like for Villa
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter is not null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter is not null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();

        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}