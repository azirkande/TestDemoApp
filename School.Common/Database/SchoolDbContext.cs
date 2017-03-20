using School.Core.Database.DbEntity;
using School.Core.Database.EnityConfiguration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;


namespace School.Core.Database
{
    public class SchoolDbContext : DbContext, ISchoolDbContext
    {
        public virtual DbSet<Class> Classess { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        public SchoolDbContext() : base("name=SchoolDbContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new ClassConfiguration());
            modelBuilder.Configurations.Add(new StudentConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public void UpdateEnity(object existingEntity, object newEntity)
        {
            this.Entry(existingEntity).CurrentValues.SetValues(newEntity);
        }
        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
