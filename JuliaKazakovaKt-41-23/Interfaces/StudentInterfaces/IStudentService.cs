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
    }

    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;

        public StudentService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /*public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter,CancellationToken cancellationToken = default)
        {
            var students = _dbContext.Set<Student>().Where(w => w.Group.Name == filter.GroupName).ToArrayAsync(cancellationToken);
                return students;
        }*/
        public async Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter,CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Student>().Where(s => s.Group.Name == filter.GroupName).Select(s => new Student
                {StudentId = s.StudentId,FirstName = s.FirstName,LastName = s.LastName,GroupId = s.GroupId,IsDeleted = s.IsDeleted}).ToArrayAsync(cancellationToken);
        }
    }
}
