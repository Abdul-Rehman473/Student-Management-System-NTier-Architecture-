using SMS_DAL;
using SMS_Objects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SMS_BLL
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;

        public CourseService(ICourseRepository repository)
        {
            _repository = repository;
        }

        private courseBO MapEntityToBO(courseBO entity)
        {
            return new courseBO
            {
                Id = entity.Id,
                Title = entity.Title,
                Instructor = entity.Instructor,
                CreditHours = entity.CreditHours,
                StudentId = entity.StudentId,
                Student = entity.Student != null ? new studentBO
                {
                    Id = entity.Student.Id,
                    Name = entity.Student.Name,
                    Email = entity.Student.Email,
                    PhoneNo = entity.Student.PhoneNo,
                    Age = entity.Student.Age,
                    Semester = entity.Student.Semester,
                    CGPA = entity.Student.CGPA
                } : null
            };
        }

        private courseBO MapBOToEntity(courseBO bo)
        {
            return new courseBO
            {
                Id = bo.Id,
                Title = bo.Title,
                Instructor = bo.Instructor,
                CreditHours = bo.CreditHours,
                StudentId = bo.StudentId
            };
        }

        public IEnumerable<courseBO> GetAll()
        {
            return _repository.GetAll().Select(MapEntityToBO);
        }

        public courseBO? GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : MapEntityToBO(entity);
        }

        public void Add(courseBO course)
        {
            _repository.Add(MapBOToEntity(course));
        }

        public void Update(courseBO course)
        {
            // Get existing course to ensure we're not tracking multiple instances
            var existingCourse = _repository.GetById(course.Id);
            if (existingCourse != null)
            {
                // Update properties
                existingCourse.Title = course.Title;
                existingCourse.Instructor = course.Instructor;
                existingCourse.CreditHours = course.CreditHours;
                existingCourse.StudentId = course.StudentId;
                
                // Save changes
                _repository.Update(existingCourse);
            }
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public IEnumerable<courseBO> GetAssignedCourses()
        {
            return _repository.GetAssignedCourses().Select(MapEntityToBO);
        }

        public IEnumerable<SelectListItem> GetAllStudentsForSelect()
        {
            var students = _repository.GetAllStudents();
            return students.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Id} - {s.Name}"
            });
        }

        public IEnumerable<courseBO> GetUnassignedCourses()
        {
            return _repository.GetUnassignedCourses().Select(MapEntityToBO);
        }

        public courseBO? GetUnassignedCourseByTitle(string title)
        {
            var entity = _repository.GetUnassignedCourseByTitle(title);
            return entity == null ? null : MapEntityToBO(entity);
        }

        public courseBO? GetCourseByTitle(string title)
        {
            var entity = _repository.GetCourseByTitle(title);
            return entity == null ? null : MapEntityToBO(entity);
        }

        public bool IsCourseAssigned(int studentId, string courseTitle)
        {
            return _repository.IsCourseAssigned(studentId, courseTitle);
        }
    }
}