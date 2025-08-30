using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

//using Newtonsoft.Json;
using VillaApi.Models;
using VillaApi.Web.Models.Dto.Villa;
using VillaAPi.Web.Services.IServices;

namespace VillaAPi.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;

        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }

        public async Task<IActionResult> IndexVilla()
        {
            var apiResponse = await _villaService.GetAllAsync<APIResponse>();

            List<
                VillaDto> villaDtos = JsonSerializer.Deserialize<List<VillaDto>>(
                Convert.ToString(apiResponse.Result),new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
                
            return View(villaDtos);
        } 
    }
}
