using System.Linq.Expressions;
using VillaApi.Models;

namespace VillaApi.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber> //Jo methods IRepository<T> me likhe gaye hain, wo sab IVillaNumberRepository me bhi milenge.
    {  
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}
