﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P127ProniaProject.DAL;
using P127ProniaProject.Models;
using P127ProniaProject.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace P127ProniaProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM model = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                Plants = _context.Plants.Include(p=>p.PlantImages).ToList(),
            };

            return View(model); 
        }
    }
}
