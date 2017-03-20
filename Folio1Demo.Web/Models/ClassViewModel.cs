using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Folio1Demo.Web.Models
{
    public class ClassViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Teacher { get; set; }
        public List<StudentViewModel> Students { get; set; }
    }
}