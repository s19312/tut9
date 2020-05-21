using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut9.Models
{
    public class EnrollStudent
    {
        //public Student StudentToEnroll{get; set; }
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        //public Enrollment EnrollmentINfo { get; set; }
        public int Semester { get; set; }

        public string StadiesName { get; set; }
    }
}
