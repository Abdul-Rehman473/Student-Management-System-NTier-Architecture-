using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SMS_Identity;
using SMS_BLL;
using SMS_Objects;

namespace NTier_Final.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(IStudentService studentService, UserManager<ApplicationUser> userManager)
        {
            _studentService = studentService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var student = await _studentService.GetStudentByEmail(user.Email);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var students = _studentService.GetAllStudents();
            return View(students);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(studentBO student)
        {
            if (ModelState.IsValid)
            {              
                _studentService.AddStudent(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(studentBO student)
        {
            if (ModelState.IsValid)
            {
                _studentService.UpdateStudent(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _studentService.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
