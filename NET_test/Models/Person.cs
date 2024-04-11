using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NET_test.Models
{
    public class Person
    {

        public int Id { get; set; }
        [Name("Name")]
        [Required]
        public string Name { get; set; }
        [Name("Date of Birth")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public decimal Salary { get; set; }
    }
}
