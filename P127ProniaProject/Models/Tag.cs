using P127ProniaProject.Models.Base;
using System.Collections.Generic;

namespace P127ProniaProject.Models
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }

        List<PlantTag>PlantTags { get; set; }
    }
}
