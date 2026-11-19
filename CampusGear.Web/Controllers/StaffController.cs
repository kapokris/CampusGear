using CampusGear.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusGear.Web.Controllers
{
    [Authorize(Roles = "Staff,Admin")]
    public class StaffController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StaffController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("CampusGearApi");

            var bookings = await client.GetFromJsonAsync<List<StaffBookingDto>>("api/bookings?status=Pending");

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var client = _httpClientFactory.CreateClient("CampusGearApi");

            await client.PutAsync("api/bookings/" + id + "/approve", null);

            return RedirectToAction("Index");
        }
    }
}
