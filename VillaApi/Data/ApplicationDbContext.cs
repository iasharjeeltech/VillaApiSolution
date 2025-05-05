using Microsoft.EntityFrameworkCore;
using VillaApi.Models.Dto;
using VillaApi.Models;

namespace VillaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>(entity =>
            {
                entity.HasData( new Villa { Id = 1, Name = "Qaisar Villa", SqFt = 570, Occupancy = 6, Amenity = "", Details = "", ImageUrl="",Rate=1000,CreatedDate=DateTime.Now },
                                new Villa { Id = 2, Name = "Badruddin Villa", SqFt = 500, Occupancy = 4, Amenity = "", Details = "", ImageUrl = "", Rate = 2000, CreatedDate = DateTime.Now },
                                new Villa { Id = 3, Name = "Ahmed Villa", SqFt = 580, Occupancy = 6, Amenity = "", Details = "", ImageUrl = "", Rate = 3000, CreatedDate = DateTime.Now }
                );
            });
        }
    }
}
