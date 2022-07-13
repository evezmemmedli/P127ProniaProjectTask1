using P127ProniaProject.Models.Base;

namespace P127ProniaProject.Models
{
    public class PlantCategory:BaseEntity
    {
        public int PlantId { get; set; }
        public Plant Plant { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
