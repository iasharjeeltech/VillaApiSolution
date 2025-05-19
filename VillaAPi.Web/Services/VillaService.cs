using VillaApi.Web.Models.Dto.Villa;
using VillaAPi.Web.Models;
using VillaAPi.Web.Services.IServices;
using static VillaApi.Utility.StaticDetails;

namespace VillaAPi.Web.Services
{
    public class VillaService : BaseServices, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _villaApiUrl;

        public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _villaApiUrl = configuration["ServiceUrls:VillaApi"];
        }

        public Task<T> CreateAsync<T>(VillaUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.POST,
                Url =_villaApiUrl +"/api/villaapi",
                Data = dto

            });
        }

        public Task DeleteAsync<T>(int Id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.DELETE,
                Url = _villaApiUrl + "/api/villaapi"+Id,

            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = _villaApiUrl + "/api/villaapi",
            });
        }

        public Task<T> GetAsync<T>(int Id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = _villaApiUrl + "/api/villaapi" + Id,
            });
        }

        public Task<T> UpdateAsync<T>(VillaCreateDto dto)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.PUT,
                Url = _villaApiUrl + "/api/villaapi"+dto.Id,
                Data = dto
            });
        }
    }
}
