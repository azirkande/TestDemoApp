using Autofac;
using School.Core.Database;
using School.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Folio1Demo.Web.Infrastructure
{
    public class CommonIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SchoolDbContext>()
                      .As<ISchoolDbContext>()
                      .InstancePerLifetimeScope();
            builder.RegisterType<ClassService>()
               .As<IClassService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<StudentService>()
              .As<IStudentService>()
              .InstancePerLifetimeScope();
            builder.RegisterType<SchoolDbContextFactory>().As<ISchoolDbContextFactory>().InstancePerLifetimeScope();
        }
    }
}