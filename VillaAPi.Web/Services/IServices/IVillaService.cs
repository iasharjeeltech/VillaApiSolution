using VillaApi.Web.Models.Dto.Villa;

namespace VillaAPi.Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> UpdateAsync<T>(VillaUpdateDto dto);
        Task<T> CreateAsync<T>(VillaCreateDto dto);
        Task DeleteAsync<T>(int id);
    }
}
