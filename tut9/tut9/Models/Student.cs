using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace tut9.Models
{
    public partial class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        [ForeignKey("Enrollment")]
        public int IdEnrollment { get; set; }

        public virtual Enrollment IdEnrollmentNavigation { get; set; }
    }
}
