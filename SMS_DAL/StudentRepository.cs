using SMS_DAL.Data;
using SMS_Objects;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SMS_DAL
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<studentBO> GetAll()
        {
            return _context.Students.ToList();
        }

        public studentBO GetById(int id)
        {
            return _context.Students.Find(id);
        }

        public void Add(studentBO student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void Update(studentBO student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public async Task<studentBO> GetByEmail(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
