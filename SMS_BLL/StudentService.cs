using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS_Objects;
using SMS_DAL;
using Microsoft.EntityFrameworkCore;

namespace SMS_BLL
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<studentBO> GetAllStudents()
        {
            return _repository.GetAll();
        }

        public studentBO GetStudentById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddStudent(studentBO student)
        {
            _repository.Add(student);
        }

        public void UpdateStudent(studentBO student)
        {
            _repository.Update(student);
        }

        public void DeleteStudent(int id)
        {
            _repository.Delete(id);
        }

        public async Task<studentBO> GetStudentByEmail(string email)
        {
            return await _repository.GetByEmail(email);
        }
    }
}
