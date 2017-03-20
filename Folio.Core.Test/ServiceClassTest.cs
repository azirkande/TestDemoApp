using NUnit.Framework;
using Moq;
using School.Core.Database;
using School.Core.Service;
using System.Data.Entity;
using School.Core.Database.DbEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace Folio.Core.Test
{
    [TestFixture]
    public class ServiceClassTest
    {
        Mock<ISchoolDbContext> dbCondextMock = new Mock<ISchoolDbContext>();
        Mock<ISchoolDbContextFactory> dbFactory = new Mock<ISchoolDbContextFactory>();
        Student demoStudent = new Student { FirstName = "Abc", LastName = "pqr", Age = 19, ClassId = 1, Gpa = 3.2, Id = 1 };
        IClassService classService;
        IStudentService studentService;

        IQueryable<Student> students = new List<Student>
            {
                new Student { FirstName = "Abc", LastName ="pqr", Age = 19, ClassId = 1, Gpa = 3.2, Id=1 },
                  new Student { FirstName = "Amrita", LastName ="Zirkande", Age = 19, ClassId = 1, Gpa = 3.2, Id=2 },
                  new Student { FirstName = "Prashant", LastName ="Dhange", Age = 19, ClassId = 1, Gpa = 3.2, Id=3 },
            }.AsQueryable();

        IQueryable<Class> classes = new List<Class>
            {
                new Class { Id = 1 , Name ="UI/UX", Location ="Camberwell" , Teacher="Mr. David" },
                  new Class { Id = 2 , Name ="DevOps", Location ="Camberwell" , Teacher="Mr. John" }

            }.AsQueryable();

        Mock<DbSet<Class>> mockClassSet = new Mock<DbSet<Class>>();
        Mock<DbSet<Student>> mockStudentSet = new Mock<DbSet<Student>>();

        [OneTimeSetUp]
        public void initializeTest()
        {
            mockStudentSet = new Mock<DbSet<Student>>();

            mockStudentSet.As<IDbAsyncEnumerable<Student>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Student>(students.GetEnumerator()));

            mockStudentSet.As<IQueryable<Student>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Student>(students.Provider));

            mockStudentSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(students.Expression);
            mockStudentSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(students.ElementType);
            mockStudentSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(students.GetEnumerator());

            var dbCondextMock = new Mock<ISchoolDbContext>();
            dbCondextMock.Setup(c => c.Students).Returns(mockStudentSet.Object);

            mockClassSet.As<IDbAsyncEnumerable<Class>>()
               .Setup(m => m.GetAsyncEnumerator())
               .Returns(new TestDbAsyncEnumerator<Class>(classes.GetEnumerator()));

            mockClassSet.As<IQueryable<Student>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Student>(classes.Provider));


            mockClassSet.As<IQueryable<Class>>().Setup(m => m.Expression).Returns(classes.Expression);
            mockClassSet.As<IQueryable<Class>>().Setup(m => m.ElementType).Returns(classes.ElementType);
            mockClassSet.As<IQueryable<Class>>().Setup(m => m.GetEnumerator()).Returns(classes.GetEnumerator());

            dbCondextMock.Setup(s => s.Classess).Returns(mockClassSet.Object);
            dbFactory.Setup(s => s.Create()).Returns(dbCondextMock.Object);
            classService = new ClassService(dbFactory.Object);
            studentService = new StudentService(dbFactory.Object);
        }


        [Test]
        public void get_all_students()
        {
            var listOfStudents = studentService.GetStudents(1);
            Assert.AreEqual(3, listOfStudents.Count());
        }

        [Test]
        public void get_student_by_id()
        {
            var result = studentService.GetStudentById(1);
            var student = (Student)result.Result;
            Assert.AreEqual("Abc", student.FirstName);
        }

        [Test]
        public void create_student_with_same_last_name()
        {
            var result = studentService.Save(new Student { FirstName = "Diff", LastName = "pqr", Age = 19, ClassId = 1, Gpa = 8, Id = 0 });
            var status = (StudentStatus)result.Result;
            Assert.AreEqual(StudentStatus.DuplicateStudent, status);
        }

        [Test]
        public void create_student_with_different_last_name()
        {
            var result = studentService.Save(new Student { FirstName = "Diff", LastName = "Lname", Age = 19, ClassId = 1, Gpa = 8, Id = 0 });
            var status = (StudentStatus)result.Result;
            Assert.AreEqual(StudentStatus.StudentAdded, status);
        }

        [Test]
        public void edit_existing_student_with_diff_name()
        {
            var newStudent = new Student { FirstName = "Diff", LastName = "Lname", Age = 19, ClassId = 1, Gpa = 8, Id = 1 };
            var existingStudent = new Student { FirstName = "Abc", LastName = "pqr", Age = 19, ClassId = 1, Gpa = 3.2, Id = 1 };
            dbCondextMock.Setup(s => s.UpdateEnity(existingStudent, newStudent)).Verifiable();
            var result = studentService.Save(newStudent);
            var status = (StudentStatus)result.Result;
            Assert.AreEqual(StudentStatus.StudentEdited, status);
        }

        [Test]
        public void get_all_classes()
        {
            var classes = classService.GetClassess();
            Assert.AreEqual(2, classes.Count);
        }

        [Test]
        public void get_class_by_id()
        {
            var classTaks = classService.GetClassById(1);
            var result = (Class)classTaks.Result;
            Assert.AreEqual("UI/UX", result.Name);
        }

    }
}
