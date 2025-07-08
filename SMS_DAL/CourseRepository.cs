using SMS_DAL.Data;
using SMS_Objects;
using Microsoft.EntityFrameworkCore;

namespace SMS_DAL
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<courseBO> GetAll()
        {
            return _context.Courses.Include(c => c.Student).ToList();
        }

        public courseBO? GetById(int id)
        {
            return _context.Courses.Include(c => c.Student).FirstOrDefault(c => c.Id == id);
        }

        public void Add(courseBO course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges(); // Explicit save
        }

        public void Update(courseBO course)
        {
            // Ensure the entity is being tracked
            var existingCourse = _context.Courses.Find(course.Id);
            if (existingCourse != null)
            {
                // Update properties
                _context.Entry(existingCourse).CurrentValues.SetValues(course);
                _context.SaveChanges(); // Explicit save
            }
        }

        public void Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges(); // Explicit save
            }
        }

        public IEnumerable<courseBO> GetAssignedCourses()
        {
            return _context.Courses.Include(c => c.Student).Where(c => c.StudentId != null).ToList();
        }

        public IEnumerable<courseBO> GetUnassignedCourses()
        {
            return _context.Courses.Include(c => c.Student).Where(c => c.StudentId == null).ToList();
        }

        public courseBO? GetUnassignedCourseByTitle(string title)
        {
            return _context.Courses
                .Include(c => c.Student)
                .FirstOrDefault(c => c.Title == title && c.StudentId == null);
        }

        public courseBO? GetCourseByTitle(string title)
        {
            return _context.Courses
                .Include(c => c.Student)
                .FirstOrDefault(c => c.Title == title);
        }

        public bool IsCourseAssigned(int studentId, string courseTitle)
        {
            return _context.Courses.Any(c => c.StudentId == studentId && c.Title == courseTitle);
        }

        public IEnumerable<studentBO> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}