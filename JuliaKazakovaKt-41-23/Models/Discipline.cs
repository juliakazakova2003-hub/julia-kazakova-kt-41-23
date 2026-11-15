using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JuliaKazakovaKt_41_23.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }
        public string Name { get; set; } 
        public string Direction { get; set; }
        public bool IsDeleted { get; set; } = false;

        [JsonIgnore]
        public ICollection<Grade> Grades { get; set; }
            = new List<Grade>();
    }
}
