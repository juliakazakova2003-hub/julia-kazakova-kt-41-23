using JuliaKazakovaKt_41_23.Data;
using JuliaKazakovaKt_41_23.Filters.DisciplineFilters;
using JuliaKazakovaKt_41_23.Filters.GradeFilters;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace JuliaKazakovaKt_41_23.Interfaces.GradeInterfaces
{
    public interface IGradeService
    {
        Task<double?> GetAverageGradeByGroupAndDisciplineAsync(GradeGroupDisciplineFilter filter, CancellationToken cancellationToken);
        Task<Grade[]> GetGradesByStudentAsync(GradeStudentFilter filter, CancellationToken cancellationToken);
        Task<double?> GetAverageGradeByCourseAsync(GradeCourseFilter filter, CancellationToken cancellationToken);
        Task<Grade?> CreateGradeAsync(Grade grade, CancellationToken cancellationToken);
        Task<Grade?> UpdateGradeAsync(Grade gradeNew, CancellationToken cancellationToken);
    }

    public class GradeService : IGradeService
    {
        private readonly StudentDbContext _dbContext;

        public GradeService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Средний балл по дисциплине в группе
        public async Task<double?> GetAverageGradeByGroupAndDisciplineAsync(
            GradeGroupDisciplineFilter filter,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Grade>()
                .Where(g =>
                    g.Student.GroupId == filter.GroupId &&
                    g.DisciplineId == filter.DisciplineId &&
                    !g.Student.IsDeleted &&
                    !g.Student.Group.IsDeleted)
                .Select(g => (double?)g.Value)
                .AverageAsync(cancellationToken);
        }

        // Получение всех оценок конкретного студента
        public async Task<Grade[]> GetGradesByStudentAsync(GradeStudentFilter filter, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Grade>()
                .Where(g => g.StudentId == filter.StudentId)
                .Select(g => new Grade
                {
                    GradeId = g.GradeId,
                    Value = g.Value,
                    StudentId = g.StudentId,
                    DisciplineId = g.DisciplineId,
                    Student = null!,
                    Discipline = null!
                })
                .ToArrayAsync(cancellationToken);
        }

        // Средний балл по курсу обучения
        public async Task<double?> GetAverageGradeByCourseAsync(GradeCourseFilter filter, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Grade>()
                .Where(g =>
                    g.Student.Group.Course == filter.Course &&
                    !g.Student.IsDeleted &&
                    !g.Student.Group.IsDeleted)
                .Select(g => (double?)g.Value)
                .AverageAsync(cancellationToken);
        }
        public async Task<Grade?> CreateGradeAsync(Grade grade, CancellationToken cancellationToken)
        {
            var studentIsActive = await _dbContext.Set<Student>()
                .AnyAsync(s =>
                    s.StudentId == grade.StudentId &&
                    !s.IsDeleted &&
                    !s.Group.IsDeleted,
                    cancellationToken);

            var disciplineIsActive = await _dbContext.Set<Discipline>()
                .AnyAsync(d =>
                    d.DisciplineId == grade.DisciplineId &&
                    !d.IsDeleted,
                    cancellationToken);

            if (!studentIsActive || !disciplineIsActive)
            {
                return null;
            }

            var gradeNew = new Grade
            {
                Value = grade.Value,
                StudentId = grade.StudentId,
                DisciplineId = grade.DisciplineId
            };

            _dbContext.Set<Grade>().Add(gradeNew);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return gradeNew;
        }
        public async Task<Grade> UpdateGradeAsync(Grade gradeNew, CancellationToken cancellationToken)
        {
            var grade = await _dbContext.Set<Grade>().FirstOrDefaultAsync(g => g.GradeId == gradeNew.GradeId);

            if (grade == null)
            {
                return null;
            }

            if (gradeNew.Value != 0)
                grade.Value = gradeNew.Value;

            if (gradeNew.StudentId != 0)
                grade.StudentId = gradeNew.StudentId;

            if (gradeNew.DisciplineId != 0)
                grade.DisciplineId = gradeNew.DisciplineId;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return grade;
        }
    }
}
