using AutoMapper;
using VillaApi.Web.Models.Dto;
using VillaApi.Web.Models.Dto.Villa;
using VillaApi.Web.Models.Dto.VillaNumber;

namespace VillaAPi.Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();

            CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberUpdateDto>().ReverseMap();
        }
    }
}
