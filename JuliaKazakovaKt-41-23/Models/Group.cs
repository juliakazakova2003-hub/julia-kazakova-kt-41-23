using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace JuliaKazakovaKt_41_23.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int Course { get; set; }
        public int SpecialtyId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [JsonIgnore]
        [ValidateNever]
        public Specialty Specialty { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public ICollection<Student> Students { get; set; }
            = new List<Student>();
        public bool IsValidName()
        {
            return Regex.IsMatch(Name ?? "", @"^\D*-\d*-\d{2}$");
        }
    }
}
