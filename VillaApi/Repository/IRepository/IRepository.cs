using System.Linq.Expressions;
using VillaApi.Models;

namespace VillaApi.Repository.IRepository
{
    public interface IRepository<T> where T : class   // IRepository ek generic interface hai jo kisi bhi class (T) ke liye kaam kar sakta hai.
                                                      // 'where T : class' ka matlab hai ki T sirf reference types (yaani classes) hi ho sakta hai, value types (int, bool, etc.) nahi.
    {
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task SaveAsync();
    }
}
