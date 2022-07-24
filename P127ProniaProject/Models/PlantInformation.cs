using P127ProniaProject.Models.Base;
using System.Collections.Generic;

namespace P127ProniaProject.Models
{
    public class PlantInformation:BaseEntity

    {
        public string Shipping { get; set; }

        public string AboutReturnRequest { get; set; }

        public string Guarentee { get; set; }

        public List<Plant> Plants { get; set; }
    }
}
