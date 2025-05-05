using System.Linq.Expressions;
using VillaApi.Models;

namespace VillaApi.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {  
        Task<Villa> UpdateAsync(Villa entity);
        Task SaveAsync();
    }
}
