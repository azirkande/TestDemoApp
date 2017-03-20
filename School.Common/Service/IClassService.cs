using School.Core.Database.DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Service
{
    public interface IClassService
    {
        List<Class> GetClassess();
        Task<int> Save(Class courseClass);
        Task Delete(int classId);
        Task<Class> GetClassById(int classId);
    }
}
