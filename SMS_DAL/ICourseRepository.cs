using SMS_Objects;
using System.Collections.Generic;

namespace SMS_DAL
{
    public interface ICourseRepository
    {
        IEnumerable<courseBO> GetAll();
        courseBO? GetById(int id);
        void Add(courseBO course);
        void Update(courseBO course);
        void Delete(int id);
        IEnumerable<courseBO> GetAssignedCourses();
        IEnumerable<courseBO> GetUnassignedCourses();
        courseBO? GetUnassignedCourseByTitle(string title);
        courseBO? GetCourseByTitle(string title);
        bool IsCourseAssigned(int studentId, string courseTitle);
        IEnumerable<studentBO> GetAllStudents();
        void Save();
    }
}