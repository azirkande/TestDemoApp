using Folio1Demo.Web.Models;
using School.Core.Database.DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Folio1Demo.Web.Infrastructure
{
    public class EntityMapper
    {
        public static Class MapToDbEntity(ClassViewModel model)
        {
            var courseClass = new Class
            {
                Id = model.Id,
                Location = model.Location,
                Name = model.Name,
                Teacher = model.Teacher
            };

            return courseClass;
        }
        public static ClassViewModel MapFromDbEntity(Class courseClass)
        {
            if (courseClass == null)
            { return null; }
            var model = new ClassViewModel
            {
                Id = courseClass.Id,
                Location = courseClass.Location,
                Name = courseClass.Name,
                Teacher = courseClass.Teacher
            };

            return model;
        }

        public static Student MapToDbEntity(StudentViewModel model)
        {
            var student = new Student
            {
                Id = model.Id,
                Age = model.Age,
                ClassId = model.ClassId,
                Gpa = model.Gpa,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            return student;
        }

        public static StudentViewModel MapFromDbEntity(Student model)
        {
            var student = new StudentViewModel
            {
                Id = model.Id,
                Age = model.Age,
                ClassId = model.ClassId,
                Gpa = model.Gpa,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            return student;
        }
    }
}