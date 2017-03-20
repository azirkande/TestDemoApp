using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Database.DbEntity
{
 
   public  class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Teacher { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
