using VillaApi.Models;
using VillaAPi.Web.Models;

namespace VillaAPi.Web.Services.IServices
{
    public interface IBaseServices
    {
        Task<T> SendAsync<T>(APIRequest apiRequest);
        public APIResponse ResponseModel { get; set; }
    }
}
