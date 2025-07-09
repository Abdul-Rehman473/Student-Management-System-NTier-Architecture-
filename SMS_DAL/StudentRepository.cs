using SMS_Objects;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SMS_Identity;

namespace SMS_DAL
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<studentBO> GetAll()
        {
            return _context.Students.Include(s => s.AssignedCourses).ToList();
        }

        public studentBO GetById(int id)
        {
            return _context.Students.Include(s => s.AssignedCourses).FirstOrDefault(s => s.Id == id);
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
            return await _context.Students
                .Include(s => s.AssignedCourses)
                .FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
