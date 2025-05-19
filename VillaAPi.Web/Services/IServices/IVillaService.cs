using VillaApi.Web.Models.Dto.Villa;

namespace VillaAPi.Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>();
        Task<T> UpdateAsync<T>(VillaCreateDto dto);
        Task<T> CreateAsync<T>(VillaUpdateDto dto);
        Task DeleteAsync<T>(int id);
    }
}
