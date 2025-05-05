using Microsoft.EntityFrameworkCore;
using VillaApi.Data;
using VillaApi.Models;
using VillaApi.Repository.IRepository;

namespace VillaApi.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            //_db.Entry(entity).State = EntityState.Modified; //yeh pehle use hota tha
            _db.VillaNumbers.Update(entity); // ab yeh use hota hai 
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
