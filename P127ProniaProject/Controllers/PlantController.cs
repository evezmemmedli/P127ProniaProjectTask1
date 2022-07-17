using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P127ProniaProject.DAL;
using P127ProniaProject.Models;
using P127ProniaProject.ViewModels;
using System.Collections.Generic;
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
        public async Task <IActionResult> Detail(int? id)
        {
            if(id == null || id==0) return NotFound();
            PlantVM model = new PlantVM
            {
                Plant = await _context.Plants.Include(p => p.PlantImages)
                .Include(p => p.PlantTags)
                .ThenInclude(p => p.Tag)
                .Include(p => p.PlantCategories)
                .ThenInclude(p => p.Category)
                .Include(p => p.PlantInformation)
                .FirstOrDefaultAsync(p => p.Id == id),
                Plants = new List<Plant>()

            };
            List<Plant>plants = new List<Plant>();
            foreach (PlantCategory plantCategory  in model.Plant.PlantCategories)
            {
                plants= await _context.Plants.Include(p=>p.PlantCategories)
                    .Include(p=>p.PlantImages)
                    .Where(p=>p.PlantCategories.Any(p=>p.CategoryId == plantCategory.CategoryId)).ToListAsync();
                model.Plants.AddRange(plants);
            }
            model.Plants = model.Plants.Distinct().ToList();
            return View(model);
        }
        public async Task<IActionResult> Partial()
        {
            List<Plant> plants = await _context.Plants.Include(p => p.PlantImages).ToListAsync();
            return PartialView("_plantsPartialView", plants);
        }
        public async Task<IActionResult> GetDetail(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Plant plant = await _context.Plants.Include(p => p.PlantImages)
                .Include(p => p.PlantInformation)
                .Include(p => p.PlantTags)
                .ThenInclude(p => p.Tag)
                .Include(p => p.PlantCategories)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            return PartialView("_plantDetail", plant);
        }

        public async Task< IActionResult> AddBasket(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Plant plant = await _context.Plants.FirstOrDefaultAsync(p=>p.Id == id);
            if (plant == null) return NotFound();
            string basketStr = HttpContext.Request.Cookies["Basket"];

            //HttpContext.Response.Cookies.Append("basket", plant.Name);
            BasketVM basket ;
            if (string.IsNullOrEmpty(basketStr))
            {
                basket = new BasketVM();
               
                BasketCookieItemVM cookieItem = new BasketCookieItemVM
                {
                    Id = plant.Id,
                    Quantity = 1
                };
                basket.BasketCookieItemVMs = new List<BasketCookieItemVM>();
                basket.BasketCookieItemVMs.Add(cookieItem);
                basket.TotalPrice = plant.Price;
            }
            else
            {
                basket=JsonConvert.DeserializeObject<BasketVM>(basketStr);
                BasketCookieItemVM existed = basket.BasketCookieItemVMs.Find(p => p.Id == id);
                if(existed == null)
                {
                    BasketCookieItemVM cookieItem = new BasketCookieItemVM
                    {
                        Id = plant.Id,
                        Quantity = 1
                    };
                    basket.BasketCookieItemVMs.Add(cookieItem);
                    basket.TotalPrice += plant.Price;
                }
                else
                {
                    basket.TotalPrice += plant.Price;
                    existed.Quantity++;

                }
            }

           
            basketStr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("Basket", basketStr);

            return RedirectToAction("Index","Home");
        }


        public IActionResult Showbasket()
        {
            if(HttpContext.Request.Cookies["Basket"]==null) return NotFound();
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Basket"]);
            return Json(basket);
        }
    }
}
