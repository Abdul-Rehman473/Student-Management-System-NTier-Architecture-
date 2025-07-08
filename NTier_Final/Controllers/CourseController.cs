using Microsoft.AspNetCore.Mvc;
using SMS_BLL;
using SMS_Objects;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace NTier_Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            var courses = _courseService.GetAll().ToList();
            return View(courses);
        }

        public IActionResult Details(int id)
        {
            var course = _courseService.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpGet]
        public IActionResult CourseForm(int? id)
        {
            if (id.HasValue)
            {
                var course = _courseService.GetById(id.Value);
                if (course == null) return NotFound();
                return View(course);
            }
            return View(new courseBO());
        }

        [HttpPost]
        public IActionResult CourseForm(courseBO course)
        {
            if (ModelState.IsValid)
            {
                if (course.Id == 0)
                    _courseService.Add(course);
                else
                    _courseService.Update(course);
                
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult _AllCourses()
        {
            var courses = _courseService.GetAll().ToList();
            return PartialView(courses);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _courseService.Delete(id);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        private List<SelectListItem> GetStudentSelectList()
        {
            var students = _courseService.GetAllStudentsForSelect();
            return students.ToList();
        }

        private List<SelectListItem> GetCourseTitles()
        {
            var courses = _courseService.GetAll();
            return courses.Select(c => new SelectListItem
            {
                Value = c.Title,
                Text = c.Title
            }).Distinct().ToList();
        }

        public IActionResult AssignCourse()
        {
            var viewModel = new AssignCourseBO
            {
                Students = GetStudentSelectList(),
                Courses = GetCourseTitles(),
                AssignedCourses = _courseService.GetAssignedCourses().ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AssignCourse(AssignCourseBO model)
        {
            try
            {
                if (model.StudentId == 0 || string.IsNullOrEmpty(model.CourseTitle))
                {
                    ModelState.AddModelError("", "Please select both student and course.");
                }
                else
                {
                    var alreadyAssigned = _courseService.IsCourseAssigned(model.StudentId, model.CourseTitle);
                    if (alreadyAssigned)
                    {
                        ModelState.AddModelError("", "This course is already assigned to this student.");
                    }
                    else
                    {
                        var course = _courseService.GetCourseByTitle(model.CourseTitle);
                        if (course == null)
                        {
                            ModelState.AddModelError("", "Selected course not found.");
                        }
                        else
                        {
                            course.StudentId = model.StudentId;
                            _courseService.Update(course);
                        }
                    }
                }

                var assignedCourses = _courseService.GetAssignedCourses().ToList();
                return PartialView("_AssignedCourses", assignedCourses);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while assigning the course: " + ex.Message);
                var viewModel = new AssignCourseBO
                {
                    Students = GetStudentSelectList(),
                    Courses = GetCourseTitles(),
                    AssignedCourses = _courseService.GetAssignedCourses().ToList()
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult DeleteAssignedCourse(int id)
        {
            try 
            {
                var course = _courseService.GetById(id);
                if (course != null)
                {
                    course.StudentId = null;
                    _courseService.Update(course);
                }
                var assignedCourses = _courseService.GetAssignedCourses().ToList();
                return PartialView("_AssignedCourses", assignedCourses);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}