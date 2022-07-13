using P127ProniaProject.Models.Base;
using System.Collections.Generic;

namespace P127ProniaProject.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        public List<PlantCategory> PlantCategories { get; set; }
    }
}
