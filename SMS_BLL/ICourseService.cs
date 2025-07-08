using SMS_Objects;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace SMS_BLL
{
    public interface ICourseService
    {
        IEnumerable<courseBO> GetAll();
        courseBO? GetById(int id);
        void Add(courseBO course);
        void Update(courseBO course);
        void Delete(int id);
        IEnumerable<courseBO> GetAssignedCourses();
        IEnumerable<SelectListItem> GetAllStudentsForSelect();
        IEnumerable<courseBO> GetUnassignedCourses();
        courseBO? GetUnassignedCourseByTitle(string title);
        courseBO? GetCourseByTitle(string title);
        bool IsCourseAssigned(int studentId, string courseTitle);
    }
}