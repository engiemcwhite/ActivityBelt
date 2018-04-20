using System;
using System.ComponentModel.DataAnnotations;
namespace beltexam.Models
{
    public class PlanViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Time, ErrorMessage = "Must provide a time.")]
        public TimeSpan Time {get;set;}

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Must provide a date.")]
        [CurrentDate(ErrorMessage = "Activities must be planned for the future!")]
        public DateTime Date { get; set; }

        [Required]
        [MinLength(10)]
        public string Desc { get; set;}

        [Required]
        [Range(0,Int32.MaxValue)]
        public int Duration { get; set;}

        [Required]
        public string Unit { get; set; }
    }
}