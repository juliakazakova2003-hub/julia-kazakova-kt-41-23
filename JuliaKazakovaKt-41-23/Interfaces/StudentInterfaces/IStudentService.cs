//using JuliaKazakovaKt_41_23.Data;
using JuliaKazakovaKt_41_23.Data;
using JuliaKazakovaKt_41_23.Database;
using JuliaKazakovaKt_41_23.Filters.StudentFilters;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace JuliaKazakovaKt_41_23.Interfaces.StudentsInterfaces
{
    public interface IStudentService
    {
        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter,CancellationToken cancellationToken);
        public Task<Student[]> GetStudentsByLastNameAsync(StudentLastNameFilter filter, CancellationToken cancellationToken);
        public Task<Student> CreateStudentAsync(Student student,CancellationToken cancellationToken);
        public Task<Student> UpdateStudentAsync(Student studentNew,CancellationToken cancellationToken);
        public Task<bool> DeleteStudentAsync(int idDelete,CancellationToken cancellationToken);
    }

    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;

        public StudentService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            var students = _dbContext.Set<Student>()
                .Where(w => w.Group.Name == filter.GroupName 
                      && !w.IsDeleted
                      && !w.Group.IsDeleted)
                .Select(s => new Student
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GroupId = s.GroupId,
                    Group = null
                })
                .ToArrayAsync(cancellationToken);
            
            return students;
        }

        public Task<Student[]> GetStudentsByLastNameAsync(StudentLastNameFilter filter, CancellationToken cancellationToken = default)
        {
            var students = _dbContext.Set<Student>()
                .Where(w => w.LastName == filter.LastName 
                      && !w.IsDeleted
                      && !w.Group.IsDeleted)
                .Select(s => new Student
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GroupId = s.GroupId,
                    Group = null
                }).ToArrayAsync(cancellationToken);

            return students;
        }

        /*public async Task<Student> CreateStudentAsync(Student student,CancellationToken cancellationToken)
        {
            var studentNew = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                GroupId = student.GroupId,
                IsDeleted = false
            };

            _dbContext.Set<Student>().Add(studentNew);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return studentNew;
        }*/

        public async Task<Student> CreateStudentAsync(Student student, CancellationToken cancellationToken)
        {
            var existingStudent = await _dbContext.Set<Student>()
                .FirstOrDefaultAsync(s =>
                    s.FirstName == student.FirstName &&
                    s.LastName == student.LastName &&
                    s.GroupId == student.GroupId,
                    cancellationToken);

            if (existingStudent != null)
            {
                return null;
            }

            var studentNew = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                GroupId = student.GroupId
            };

            _dbContext.Set<Student>().Add(studentNew);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return studentNew;
        }

        public async Task<Student> UpdateStudentAsync(Student studentNew, CancellationToken cancellationToken)
        {
            var student = await _dbContext.Set<Student>()
                .FirstOrDefaultAsync(s => s.StudentId == studentNew.StudentId, cancellationToken);

            if (student == null)
            {
                return null;
            }

            if (studentNew.FirstName != null && studentNew.FirstName != "string")
                student.FirstName = studentNew.FirstName;

            if (studentNew.LastName != null && studentNew.LastName != "string")
                student.LastName = studentNew.LastName;

            if (studentNew.GroupId != 0)
                student.GroupId = studentNew.GroupId;

            student.IsDeleted = studentNew.IsDeleted;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return student;
        }
        public async Task<bool> DeleteStudentAsync(int idDelete,CancellationToken cancellationToken)
        {
            var student = await _dbContext.Set<Student>()
                .FirstOrDefaultAsync(s => s.StudentId == idDelete && !s.IsDeleted);

            if (student == null)
            {
                return false;
            }

            student.IsDeleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
