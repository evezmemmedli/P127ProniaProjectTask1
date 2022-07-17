using P127ProniaProject.Models.Base;

namespace P127ProniaProject.Models
{
    public class Setting:BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
