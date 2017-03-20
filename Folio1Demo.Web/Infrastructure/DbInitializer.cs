using School.Core.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Folio1Demo.Web.Infrastructure
{
    public class DbInitializer
    {
        public static void ForceInitializeDb()
        {
            System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<SchoolDbContext>());
            using (var context = new SchoolDbContext())
            {
                context.Database.Initialize(force: true);
            }
        }
    }
}