using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P127ProniaProject.DAL;
using P127ProniaProject.Extensions;
using P127ProniaProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P127ProniaProject.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class PlantController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Plant> model = _context.Plants
                .Include(p => p.PlantInformation)
                .Include(p => p.PlantCategories)
                .ThenInclude(p => p.Category)
                .Include(p => p.PlantImages)
                .Include(p => p.PlantTags)
                .ThenInclude(p => p.Tag).ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Information = _context.PlantInformations.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                return View();
            }
            if (plant.MainPhoto == null || plant.HoverPhoto == null || plant.Photos == null)
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "You must choose 1 main photo and 1 hover photo and 1 another photo ");
                return View();
            }
            if (!plant.MainPhoto.ImageIsOkay(2) || !plant.HoverPhoto.ImageIsOkay(2))
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Please choose valid image file");
                return View();
            }
            plant.PlantImages = new List<PlantImage>();
            TempData["FileName"] = "";
            List<IFormFile> removeable = new List<IFormFile>(); 
            foreach (IFormFile photo in plant.Photos)
            {
                if (!photo.ImageIsOkay(2))
                {
                    removeable.Add(photo);
                    TempData["FileName"] += photo.FileName + " ,";
                    continue;
                }
                PlantImage another = new PlantImage
                {
                    Name = await photo.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                    IsMain = false,
                    Alternative = photo.Name,
                    Plant = plant
                };
                plant.PlantImages.Add(another);
            }

            plant.Photos.RemoveAll(p => removeable.Any(r=>r.FileName == p.FileName));
            PlantImage main = new PlantImage
            {
                Name = await plant.MainPhoto.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                IsMain = true,
                Alternative = plant.Name,
                Plant = plant
            };
            PlantImage hover = new PlantImage
            {
                Name = await plant.HoverPhoto.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                IsMain = null,
                Alternative = plant.Name,
                Plant = plant
            };
            plant.PlantImages.Add(main);
            plant.PlantImages.Add(hover);
            plant.PlantCategories = new List<PlantCategory>();
            foreach (int id in plant.CategoryIds)
            {
                PlantCategory category = new PlantCategory
                {
                    CategoryId = id,
                    Plant = plant
                };
                plant.PlantCategories.Add(category);
            }
            await _context.Plants.AddAsync(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
