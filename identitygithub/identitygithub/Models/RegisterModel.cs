using System.ComponentModel.DataAnnotations;

namespace PluralsightDemo.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "adress cannot be longer than 40 characters.")]
        public string adress { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "postcode cannot be longer than 20 characters.")]
        public string postcode { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "woonplaats cannot be longer than 40 characters.")]
        public string woonplaats { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "naam cannot be longer than 40 characters.")]
        public string naam { get; set; }

        
    }
}