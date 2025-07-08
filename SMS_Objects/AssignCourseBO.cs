using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SMS_Objects
{
    public class AssignCourseBO
    {
        public int StudentId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
        public List<courseBO> AssignedCourses { get; set; } = new List<courseBO>();
    }
}