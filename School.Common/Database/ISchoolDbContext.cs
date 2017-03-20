using School.Core.Database.DbEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Database
{
    public interface ISchoolDbContext : IDisposable
    {
        DbSet<Class> Classess { get; set; }
        DbSet<Student> Students { get; set; }
        DbEntityEntry Entry(object entity);
        Task<int> SaveChangesAsync();
        void UpdateEnity(object existingEntity, object newEntity);
    }
}
