 using System.ComponentModel.DataAnnotations;
namespace EcommerceBackend.Dto
{
    public class UserRegisterationDto
    {
         [Required(ErrorMessage = "Name is required!")]
        [MaxLength(30, ErrorMessage = "Name should not execeed 30 character!")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required!.")]
        [EmailAddress(ErrorMessage = "Invalid email address!.")]
        public string ?UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required!.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long!.")]
        [MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters!.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$",
    ErrorMessage = "Password must include lowercase, number, and special character!.")]
        public string? Password { get; set; }

    }
}
