using CampusGear.Api.Dtos;
using CampusGear.Api.Models;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Mvc;

namespace CampusGear.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BookingsController(AppDbContext db)
        {
            _db = db;
        }

        // POST api/bookings
        [HttpPost]
        public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingRequest request)
        {
            DateTime startDate = request.StartDate.Date;
            DateTime endDate = startDate.AddDays(request.Duration * 7);

            if (startDate < DateTime.Today)
                return BadRequest("Start date cannot be in the past.");

            var equipment = await _db.Equipments.FindAsync(request.EquipmentId);
            if (equipment == null)
                return NotFound("Equipment not found.");

            if (!equipment.IsAvailable)
                return BadRequest("This item is not available for booking.");

            bool userExists = await _db.Users.AnyAsync(u => u.UserId == request.UserId);
            if (!userExists)
                return NotFound("User not found.");

            // The rule: say no if the item is already booked for any part of that time.
            // Two bookings overlap when each one starts before the other ends.
            bool alreadyBooked = await _db.Bookings.AnyAsync(b =>
                b.EquipmentId == request.EquipmentId &&
                (b.Status == "Pending" || b.Status == "Approved") &&
                b.StartDate < endDate &&
                b.EndDate > startDate);

            if (alreadyBooked)
                return Conflict("This item is already booked for that time.");

            var booking = new Booking
            {
                EquipmentId = request.EquipmentId,
                UserId = request.UserId,
                StartDate = startDate,
                EndDate = endDate,
                Duration = request.Duration,
                Status = "Pending"
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, ToDto(booking, equipment.EquipmentName));
        }

        // GET api/bookings
        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetBookings(int? userId, string? status)
        {
            var query = _db.Bookings.Include(b => b.Equipment).AsQueryable();

            if (userId != null)
                query = query.Where(b => b.UserId == userId);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(b => b.Status == status);

            var bookings = await query.OrderBy(b => b.StartDate).ToListAsync();
            return bookings.Select(b => ToDto(b, b.Equipment?.EquipmentName)).ToList();
        }

        // GET api/bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            var booking = await _db.Bookings.Include(b => b.Equipment).FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            return ToDto(booking, booking.Equipment?.EquipmentName);
        }

        // PUT api/bookings/5/approve
        [HttpPut("{id}/approve")]
        public async Task<ActionResult<BookingDto>> ApproveBooking(int id)
        {
            var booking = await _db.Bookings.Include(b => b.Equipment).FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
                return NotFound();

            if (booking.Status != "Pending")
                return BadRequest("Only pending bookings can be approved.");

            booking.Status = "Approved";
            await _db.SaveChangesAsync();

            return ToDto(booking, booking.Equipment?.EquipmentName);
        }

        private static BookingDto ToDto(Booking b, string? equipmentName) => new BookingDto
        {
            BookingId = b.BookingId,
            EquipmentId = b.EquipmentId,
            EquipmentName = equipmentName ?? string.Empty,
            UserId = b.UserId,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Duration = b.Duration,
            Status = b.Status
        };
    }
}
