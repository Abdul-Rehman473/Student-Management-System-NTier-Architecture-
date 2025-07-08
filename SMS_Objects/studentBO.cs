using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Objects
{
    public class studentBO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required")]
        [Range(16, 60, ErrorMessage = "Age must be between 16 and 60")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Semester is required")]
        [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "CGPA is required")]
        [Range(0, 4, ErrorMessage = "CGPA must be between 0 and 4")]
        public double CGPA { get; set; }

        public virtual ICollection<courseBO> AssignedCourses { get; set; } = new List<courseBO>();
    }
}
