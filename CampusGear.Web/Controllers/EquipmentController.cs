using CampusGear.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusGear.Web.Controllers
{
    public class EquipmentController : Controller
    {   
        private readonly HttpClient _client;

        public EquipmentController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("CampusGearApi");
        }

        // GET (/Equipment)
        public async Task<IActionResult> Index()
        {
            var equipment = await _client
                .GetFromJsonAsync<List<EquipmentDto>>(
                    "api/Equipment");

            return View(equipment ?? new List<EquipmentDto>());
        }
    }
}