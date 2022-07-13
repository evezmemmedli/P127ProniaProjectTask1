using P127ProniaProject.Models.Base;

namespace P127ProniaProject.Models
{
    public class PlantImage:BaseEntity
    {
        public string Name { get; set; }

        public string Alternative { get; set; }

        public int PlantId { get; set; }

        public bool? IsMain { get; set; }

        public Plant Plant { get; set; }
    }
}
