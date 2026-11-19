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
        public async Task<IActionResult> Index(
            string? search,
            int? categoryId,
            bool? available)
        {
            var equipment = await _client
                .GetFromJsonAsync<List<EquipmentDto>>(
                    "api/Equipment")
                ?? new List<EquipmentDto>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                equipment = equipment
                    .Where(e => e.EquipmentName.Contains(
                        search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (categoryId.HasValue)
            {
                equipment = equipment
                    .Where(e => e.CategoryId == categoryId.Value)
                    .ToList();
            }

            if (available.HasValue)
            {
                equipment = equipment
                    .Where(e => e.IsAvailable == available.Value)
                    .ToList();
            }

            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Available = available;

            return View(equipment);
        }
    }
}