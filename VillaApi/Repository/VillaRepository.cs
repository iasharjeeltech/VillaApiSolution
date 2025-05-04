using VillaApi.Data;
using VillaApi.Models;
using VillaApi.Repository.IRepository;

namespace VillaApi.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db) // its work from parent to child so thats why need to add base first give the value to parent then child and if you think who give value to child do its a di
        {
            _db = db;
        }

        public async Task UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();

        }
    }
}
