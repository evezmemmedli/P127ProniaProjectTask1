using P127ProniaProject.Models;
using System.Collections.Generic;

namespace P127ProniaProject.ViewModels
{
    public class LayoutBasketVM
    {
        public List<Plant> Plants { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
