using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace identitygithub.Models
{
    public class Editmodel
    {       
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
        [Required]
        public DateTime deadline { get; set; }
    }
}
