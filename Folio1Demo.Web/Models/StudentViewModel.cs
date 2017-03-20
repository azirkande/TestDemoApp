using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Folio1Demo.Web.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
        [Range(10, 100)]
        public int Age { get; set; }

        [Required]
        [Range(0, 10)]
        public double Gpa { get; set; }
        public int ClassId { get; set; }
    }
}