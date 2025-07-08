using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS_Objects;

namespace SMS_BLL
{
    public interface IStudentService
    {
        IEnumerable<studentBO> GetAllStudents();
        studentBO GetStudentById(int id);
        void AddStudent(studentBO student);
        void UpdateStudent(studentBO student);
        void DeleteStudent(int id);
        Task<studentBO> GetStudentByEmail(string email);
    }
}
