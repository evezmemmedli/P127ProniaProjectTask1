using P127ProniaProject.Models;
using System.Collections.Generic;

namespace P127ProniaProject.ViewModels
{
    public class BasketVM
    {
        public List<BasketCookieItemVM> BasketCookieItemVMs { get; set; }

        public decimal TotalPrice { get; set; }


    }
}
