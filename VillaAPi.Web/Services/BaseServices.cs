using Newtonsoft.Json; // Newtonsoft ka use JSON serialize/deserialize karne ke liye
using System.Text; // Encoding ke liye (UTF8)
using System.Text.Json; // .NET ka built-in serializer (yahan use nahi hua)
using VillaApi.Models; // APIResponse model
using VillaAPi.Web.Models; // Web side models
using VillaAPi.Web.Services.IServices; // IBaseServices interface
using static VillaApi.Utility.StaticDetails; // ApiType enum ke liye (GET, POST, etc.)

namespace VillaAPi.Web.Services
{
    public class BaseServices : IBaseServices
    {
        private readonly IHttpClientFactory _httpClientFactory; // HTTP request bhejne ke liye client factory

        public APIResponse ResponseModel { get; set; } // Response store karne ke liye model

        public BaseServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; // dependency inject kar rahe hain
            this.ResponseModel = new APIResponse(); // response object initialize
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {

                HttpRequestMessage requestMessage = new();             // HTTP request message create karna
                requestMessage.Headers.Add("Accept", "application/json");             // Server ko batana ki hume JSON format me data chahiye
                requestMessage.RequestUri = new Uri(apiRequest.Url);             // Request URL set karna (jo APIRequest me diya gaya hai)

                // Agar data bhejna hai (POST/PUT ke case me), to JSON format me body add karte hain
                if (apiRequest.Data != null)
                {
                    requestMessage.Content = new StringContent(
                        content: JsonConvert.SerializeObject(apiRequest.Data), // Object ko JSON string me convert
                        encoding: Encoding.UTF8, // Content encoding set
                        mediaType: "application/json" // Content-Type set
                    );
                }

                // API method type set karna (GET, POST, PUT, DELETE, PATCH)
                //switch (apiRequest.ApiType)
                //{
                //    case ApiType.POST:
                //        requestMessage.Method = HttpMethod.Post;
                //        break;
                //    case ApiType.PUT:
                //        requestMessage.Method = HttpMethod.Put;
                //        break;
                //    case ApiType.DELETE:
                //        requestMessage.Method = HttpMethod.Delete;
                //        break;
                //    case ApiType.PATCH:
                //        requestMessage.Method = HttpMethod.Patch;
                //        break;
                //    default:
                //        requestMessage.Method = HttpMethod.Get;
                //        break;
                //}

                //above concise code!
                requestMessage.Method = apiRequest.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                // HTTP client create karna (named client "VillaApi" Program.cs me define hoga)
                var client = _httpClientFactory.CreateClient("VillaApi");
                HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);             // Request send karna and response lena

                var result = await responseMessage.Content.ReadAsStringAsync();            // Response body read karna as string
                var deserializedResult = JsonConvert.DeserializeObject<T>(result); // String ko desired generic type me convert karna (example: List<VillaDTO> etc.)

                return deserializedResult;
            }
            catch (Exception ex)
            {
                var errorResponse = new APIResponse();
                errorResponse.IsSuccess = false;
                errorResponse.ErrorMessages.Add(ex.Message);

                var serializedDto = JsonConvert.SerializeObject(errorResponse);
                var deserializedDto = JsonConvert.DeserializeObject<T>(serializedDto);

                return deserializedDto;  

            }
        }
    }
}
