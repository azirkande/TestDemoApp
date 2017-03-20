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
    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            HasKey(column => column.Id);
            HasRequired(s => s.Class).WithMany().WillCascadeOnDelete(true);
            Property(column => column.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(column => column.FirstName).HasMaxLength(255).IsRequired();
            Property(column => column.LastName).HasMaxLength(255).IsRequired();
            Property(column => column.Gpa).IsRequired();
            Property(column => column.Age).IsRequired();
        }
    }
}
