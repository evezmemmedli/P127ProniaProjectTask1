using P127ProniaProject.Models.Base;
using System.Collections.Generic;

namespace P127ProniaProject.Models
{
    public class Plant:BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string SKU { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }    

        public int PlantInformationId { get; set; }

        public PlantInformation PlantInformation { get; set; }

        public List<PlantImage>PlantImages { get; set; }

        public List<PlantCategory> PlantCategories { get; set; }

        public List<PlantTag>PlantTags { get; set; }


    }
}
