using School.Core.Database;
using School.Core.Database.DbEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace School.Core.Service
{
    public class ClassService : IClassService
    {
        private readonly ISchoolDbContextFactory _dbFactory;
        public ClassService(ISchoolDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Class> GetClassess()
        {
            using (var dbContext = _dbFactory.Create())
            {
                return dbContext.Classess.ToList();
            }
        }

        public async Task<int> Save(Class courseClass)
        {

            using (var dbContext = _dbFactory.Create())
            {
                if (courseClass.Id == 0)
                {
                    dbContext.Classess.Add(courseClass);
                }
                else
                {
                    var existingClass = await dbContext.Classess.SingleOrDefaultAsync(c => c.Id == courseClass.Id);
                    dbContext.UpdateEnity(existingClass, courseClass);
                }
                return await dbContext.SaveChangesAsync();
            }
        }
        public async Task<Class> GetClassById(int classId)
        {
            using (var dbContext = _dbFactory.Create())
            {
                return await dbContext.Classess.SingleOrDefaultAsync(cls => cls.Id == classId);
            }
        }
        public async Task Delete(int classId)
        {
            using (var dbContext = _dbFactory.Create())
            {

                var existingClass = dbContext.Classess.Include(_ => _.Students).Where(s => s.Id == classId).FirstOrDefault();
                dbContext.Students.RemoveRange(existingClass.Students);
                dbContext.Classess.Remove(existingClass);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
