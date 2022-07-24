using P127ProniaProject.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P127ProniaProject.Models
{
    public class Category:BaseEntity
    {
        [Required,StringLength(maximumLength:20)]
        public string Name { get; set; }

        public List<PlantCategory> PlantCategories { get; set; }
    }
}
