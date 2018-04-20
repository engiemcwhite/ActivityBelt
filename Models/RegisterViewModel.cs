using System.ComponentModel.DataAnnotations;
namespace beltexam.Models
{
    public class RegisterViewModel : BaseEntity
    {

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        [Display(Name = "Last Name")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"(?=.*\d)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Password must contain a capital letter, a lowercase letter, and a number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation { get; set; }
    }
}
