using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P127ProniaProject.DAL;
using P127ProniaProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P127ProniaProject.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Setting> model = await _context.Settings.ToListAsync();
            return View(model);
        }
    }
}
