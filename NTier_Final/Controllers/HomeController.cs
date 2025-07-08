using Microsoft.AspNetCore.Mvc;
using SMS_BLL;

namespace NTier_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public HomeController(IStudentService studentService, ICourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            
            ViewBag.TotalStudents = _studentService.GetAllStudents().Count();
            ViewBag.TotalCourses = _courseService.GetAll().Count();

           
            var recentActivities = new[]
            {
                new { Description = "System initialized", Date = DateTime.Now }
            };
            ViewBag.RecentActivities = recentActivities;

            return View();
        }
    }
}