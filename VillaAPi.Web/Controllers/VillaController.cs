using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace VillaAPi.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _villaApiUrl;
        public VillaController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            this._villaApiUrl = configuration.GetValue<string>("ServiceUrls:VillaApi")!;
        }

        public async Task<IActionResult> IndexVilla()
        {
            var client = _httpClientFactory.CreateClient("VillaApi");

            //https://localhost:7001/api/villaapi yeh aisa baneyga

            var response = await client.GetAsync(_villaApiUrl + "/api/villaapi");

            var result = await response.Content.ReadAsStringAsync();

            return View();
        }
    }
}
