using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using P127ProniaProject.DAL;
using P127ProniaProject.Models;
using P127ProniaProject.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace P127ProniaProject.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public IHttpContextAccessor _http;

        public LayoutService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }
        public List<Setting> GetSetting()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }
        public BasketVM GetBasket()
        {
            //BasketVM basket = new BasketVM();

            string basketStr = _http.HttpContext.Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basketStr))
            {
                BasketVM basket= JsonConvert.DeserializeObject<BasketVM>(basketStr);
                foreach (BasketCookieItemVM cookie in basket.BasketCookieItemVMs)
                {
                    Plant existed = _context.Plants.FirstOrDefault(p => p.Id == cookie.Id);
                    if (existed is null)
                    {
                        basket.BasketCookieItemVMs.Remove(cookie);

                    }
                    
                }
                return basket;
            }
            return null;
        }
    }
}
