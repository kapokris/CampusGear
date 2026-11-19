using CampusGear.Api.Dtos;
using CampusGear.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CampusGear.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/equipment
        // GET api/equipment?search=mac&categoryId=1&available=true
        [HttpGet]
        public async Task<ActionResult<List<EquipmentDto>>> GetEquipment(string? search, int? categoryId, bool? available)
        {
            var query = _context.Equipments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => e.EquipmentName.Contains(search));

            if (categoryId != null)
                query = query.Where(e => e.CategoryId == categoryId);

            if (available != null)
                query = query.Where(e => e.IsAvailable == available);

            return await query
                .OrderBy(e => e.EquipmentName)
                .Select(e => new EquipmentDto
                {
                    EquipmentId = e.EquipmentId,
                    EquipmentName = e.EquipmentName,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category!.CategoryName,
                    Description = e.Description,
                    Location = e.Location,
                    Condition = e.Condition,
                    IsAvailable = e.IsAvailable
                })
                .ToListAsync();
        }

        // GET api/equipment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentDto>> GetEquipmentById(int id)
        {
            var equipment = await _context.Equipments
                .Where(e => e.EquipmentId == id)
                .Select(e => new EquipmentDto
                {
                    EquipmentId = e.EquipmentId,
                    EquipmentName = e.EquipmentName,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category!.CategoryName,
                    Description = e.Description,
                    Location = e.Location,
                    Condition = e.Condition,
                    IsAvailable = e.IsAvailable
                })
                .FirstOrDefaultAsync();

            if (equipment == null)
                return NotFound();

            return equipment;
        }
    }
}
