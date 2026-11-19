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
            List<EquipmentDto> equipment;

            try
            {
                equipment = await _client.GetFromJsonAsync<List<EquipmentDto>>("api/Equipment") ?? new List<EquipmentDto>();
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage = "Unable to load equipment. Please try again later.";

                equipment = new List<EquipmentDto>();
            }
            catch (TaskCanceledException)
            {
                ViewBag.ErrorMessage = "The equipment request timed out. Please try again.";

                equipment = new List<EquipmentDto>();
            }

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

            EquipmentDto? equipment;

            try
            {
                equipment = await _client.GetFromJsonAsync<EquipmentDto>($"api/Equipment/{id}");
            }
            catch (HttpRequestException)
            {
                TempData["ErrorMessage"] = "Unable to load the selected equipment. Please try again.";

                return RedirectToAction("Index");
            }
            catch (TaskCanceledException)
            {
                TempData["ErrorMessage"] = "The equipment request timed out. Please try again.";

                return RedirectToAction("Index");
            }

            if (equipment == null)
            {
                return NotFound();
            }

            if (!equipment.IsAvailable)
            {
                TempData["ErrorMessage"] = "This equipment is currently unavailable.";

                return RedirectToAction("Index");
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

            HttpResponseMessage response;

            try
            {
                response = await _client.PostAsJsonAsync("api/Bookings", booking);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("", "Unable to connect to the booking service. Please try again later.");

                await LoadEquipmentName(model);
                return View(model);
            }
            catch (TaskCanceledException)
            {
                ModelState.AddModelError("", "The booking request timed out. Please check your bookings before trying again.");

                await LoadEquipmentName(model);
                return View(model);
            }

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
            List<BookingDto> bookings;

            try
            {
                bookings = await _client.GetFromJsonAsync<List<BookingDto>>("api/Bookings/mine") ?? new List<BookingDto>();
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage = "Unable to load your bookings. Please try again later.";

                bookings = new List<BookingDto>();
            }
            catch (TaskCanceledException)
            {
                ViewBag.ErrorMessage = "The booking request timed out. Please try again.";

                bookings = new List<BookingDto>();
            }

            return View(bookings);
        }

        // Helper method
        private async Task LoadEquipmentName(BookingViewModel model)
        {
            model.EquipmentName = "Selected Equipment";

            if (model.EquipmentId <= 0)
            {
                return;
            }

            try
            {
                var equipment = await _client.GetFromJsonAsync<EquipmentDto>($"api/Equipment/{model.EquipmentId}");

                model.EquipmentName = equipment?.EquipmentName ?? "Selected Equipment";
            }
            catch (HttpRequestException)
            {
                // When the API is unavailable, we keep the fallback name
            }
            catch (TaskCanceledException)
            {
                // If the request times out, then we keep the fallback name
            }
        }
    }
}