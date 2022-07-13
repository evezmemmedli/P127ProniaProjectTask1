using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P127ProniaProject.DAL;
using P127ProniaProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace P127ProniaProject.Controllers
{
    public class PlantController : Controller
    {
        private readonly AppDbContext _context;

        public PlantController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Plant plant = await _context.Plants.Include(p => p.PlantImages)
                .Include(p => p.PlantInformation)
                .Include(p => p.PlantCategories).ThenInclude(p => p.Category)
                .Include(p=>p.PlantTags).ThenInclude(p=>p.Tag)
                .Include(p => p.PlantTags).FirstOrDefaultAsync(p => p.Id == id);
            
            if (plant == null) return NotFound();
            return View(plant);
        }
    }
}
