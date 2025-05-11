using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using VillaApi.Models;
using VillaAPi.Web.Models;
using VillaAPi.Web.Services.IServices;

namespace VillaAPi.Web.Services
{
    public class BaseServices : IBaseServices
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIResponse ResponseModel { get; set; }
        public BaseServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            this.ResponseModel = new APIResponse();
        }


        public Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            HttpRequestMessage requestMessage = new();

            //ContentNegotiation
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.RequestUri = new Uri(apiRequest.Url);

            if (apiRequest.Data != null)
            {
                requestMessage.Content = new StringContent(
                    //content: JsonSerializer.Serialize(apiRequest.Data),  //this is builtin serializer
                    //newtonsoft.json used package
                    content: JsonConvert.SerializeObject(apiRequest.Data),
                    mediaType: "application/json",
                    encoding: Encoding.UTF8
                );
            }



            var client = _httpClientFactory.CreateClient("VillaApi");
            client.SendAsync(requestMessage);
        }
    }
}
