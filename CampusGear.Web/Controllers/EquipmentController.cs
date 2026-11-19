using CampusGear.Web.Models;
using Microsoft.AspNetCore.Mvc;
using CampusGear.Web.ViewModels;

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

        // GET (/Equipment/Book/id)
        public async Task<IActionResult> Book(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var equipment = await _client
                .GetFromJsonAsync<EquipmentDto>($"api/Equipment/{id}");

            if (equipment == null)
            {
                return NotFound();
            }

            if (!equipment.IsAvailable)
            {
                return BadRequest("This equipment is not available.");
            }

            var model = new BookingViewModel
            {
                EquipmentId = equipment.EquipmentId,
                EquipmentName = equipment.EquipmentName
            };

            return View(model);
        }

        // POST (/Equipment/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookingViewModel model)
        {
            if(model.EquipmentId <= 0)
            {
                ModelState.AddModelError("EquipmentId", "Invalid equipment selection.");
            }

            if (!ModelState.IsValid || !model.Duration.HasValue)
            {
                await LoadEquipmentName(model);
                return View(model);
            }

            var booking = new BookingRequestDto
            {
                EquipmentId = model.EquipmentId,
                Duration = model.Duration.Value,
            };

            var response = await _client.PostAsJsonAsync("api/Bookings", booking);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Your booking request was submitted successfully.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Unable to submit booking. Please try again");

            await LoadEquipmentName(model);
            return View(model);
        }

        // GET (/Equipment/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            var bookings = await _client
                .GetFromJsonAsync<List<BookingDto>>("api/Bookings/mine") ?? new List<BookingDto>();

            return View(bookings);
        }

        // Helper method
        private async Task LoadEquipmentName(BookingViewModel model)
        {
            var equipment = await _client
                .GetFromJsonAsync<EquipmentDto>($"api/Equipment/{model.EquipmentId}");

            model.EquipmentName = equipment?.EquipmentName ?? "Selected Equipment";
        }
    }
}