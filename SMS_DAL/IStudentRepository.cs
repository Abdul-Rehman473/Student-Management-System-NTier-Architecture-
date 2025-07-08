using SMS_Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMS_DAL
{
    public interface IStudentRepository
    {
        IEnumerable<studentBO> GetAll();
        studentBO GetById(int id);
        void Add(studentBO student);
        void Update(studentBO student);
        void Delete(int id);
        Task<studentBO> GetByEmail(string email);
    }
}
