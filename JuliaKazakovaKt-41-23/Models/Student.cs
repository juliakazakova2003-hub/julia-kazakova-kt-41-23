using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JuliaKazakovaKt_41_23.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public int GroupId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [JsonIgnore]
        [ValidateNever]
        public Group Group { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public ICollection<Grade> Grades { get; set; }
            = new List<Grade>();
    }
}
