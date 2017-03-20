using School.Core.Database.DbEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Database.EnityConfiguration
{
    public class ClassConfiguration : EntityTypeConfiguration<Class>
    {
        public ClassConfiguration()
        {
            HasKey(column => column.Id);
            Property(column => column.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasMany(column => column.Students).WithOptional().HasForeignKey(column => column.ClassId);
            Property(column => column.Name).HasMaxLength(255).IsRequired();
            Property(column => column.Teacher).HasMaxLength(255).IsRequired();
            Property(column => column.Location).HasMaxLength(500).IsRequired();
        }
    }
}
