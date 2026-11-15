using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace JuliaKazakovaKt_41_23.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Student Student { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Discipline Discipline { get; set; } 
    }
}
