using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Objects
{
    public class courseBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instructor name is required.")]
        public string Instructor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Credit Hours are required.")]
        [Range(1, 4, ErrorMessage = "Credit Hours must be between 1 and 4.")]
        public int CreditHours { get; set; }

        public int? StudentId { get; set; }
        public studentBO? Student { get; set; }
    }
}
