using System.Security.Cryptography.X509Certificates;
using VillaApi.Models;
using VillaApi.Models.Dto.Villa;

namespace VillaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDto>VillaList = new List<VillaDto>()
        {
            new VillaDto{Id=1, Name="Qaisar Villa",SqFt=570,Occupancy=6},
            new VillaDto{Id=2, Name="Badruddin Villa",SqFt=500,Occupancy=4},
            new VillaDto{Id=3, Name="Ahmed Villa",SqFt=580,Occupancy=6},

        };
    }
}
