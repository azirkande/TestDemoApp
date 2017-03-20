using School.Core.Database.DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Service
{
    public interface IStudentService
    {
        List<Student> GetStudents(int classId);
        Task<StudentStatus> Save(Student student);
        Task Delete(int studentId);
        Task<Student> GetStudentById(int studentId);
    }
}
