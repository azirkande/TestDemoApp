using School.Core.Database;
using School.Core.Database.DbEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Service
{
    public class StudentService : IStudentService
    {
        private readonly ISchoolDbContextFactory _dbFactory;
        public StudentService(ISchoolDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public List<Student> GetStudents(int classId)
        {
            using (var dbContext = _dbFactory.Create())
            {
                return dbContext.Students.Where(c => c.ClassId == classId).ToList();
            }
        }
        private async Task<bool> CheckIfStudentWithLastNameAlreadyExist(ISchoolDbContext dbContext, Student newStudent)
        {
            var student = await dbContext.Students.SingleOrDefaultAsync(c => c.ClassId == newStudent.ClassId && c.LastName.ToLower().Trim().Equals(newStudent.LastName.ToLower().Trim()));
            if (newStudent.Id > 0)
            {
                if (student != null && student.Id != newStudent.Id)
                    return true;
                return false;
            }
            else
            {
                if (student != null)
                    return true;
                return false;
            }

        }

        public async Task<StudentStatus> Save(Student newStudent)
        {
            StudentStatus saveStatus = StudentStatus.Failure;
            using (var dbContext = _dbFactory.Create())
            {
                if (await CheckIfStudentWithLastNameAlreadyExist(dbContext, newStudent))
                {
                    saveStatus = StudentStatus.DuplicateStudent;
                }
                else
                {
                    var existingStudent = await dbContext.Students.SingleOrDefaultAsync(c => c.Id == newStudent.Id);
                    if (newStudent.Id == 0)
                    {
                        dbContext.Students.Add(newStudent);
                        saveStatus = StudentStatus.StudentAdded;
                    }
                    else
                    {
                        dbContext.UpdateEnity(existingStudent, newStudent);
                        saveStatus = StudentStatus.StudentEdited;
                    }

                    await dbContext.SaveChangesAsync();
                }
                return saveStatus;
            }
        }
        public async Task Delete(int studentId)
        {
            using (var dbContext = _dbFactory.Create())
            {
                var student = await dbContext.Students.SingleOrDefaultAsync(s => s.Id == studentId);
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            using (var dbContext = _dbFactory.Create())
            {
                return await dbContext.Students.SingleOrDefaultAsync(s => s.Id == studentId);
            }
        }
    }
}
