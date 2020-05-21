using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut9.Configuration;
using tut9.Models;

namespace tut9.Services
{
    public class SqlServerStudentDbService : ControllerBase, IStudentDbService
    {
  
        public readonly s19312Context context;

        private Enrollment newEnroll = new Enrollment();
        private Student newStudent = new Student();

        public SqlServerStudentDbService(s19312Context context)
        {
            this.context = context;
        }


        public IActionResult PromoteStudent(PromoteStudent promote)
        {
            //Checks if the Studies Name exists
            int studiesId = context.Studies.Where(s => s.Name == promote.Studies).Select(s1 => s1.IdStudy).FirstOrDefault();

            if (studiesId == 0) {
                return NotFound("Stadies does not exist");
            }

            //Update table Endrollment
            context.Enrollment.Where(e => e.IdStudy == studiesId && e.Semester == promote.Semester).ToList().ForEach(e1 => e1.Semester = promote.Semester + 1);
            context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult DeleteStudent(DeleteStudent delete)
        {
            //Search for Student index number to make sure that Student exist
            string studentIndex = context.Student.Where(s => s.IndexNumber == delete.IndexNumber).Select(s1 => s1.IndexNumber).FirstOrDefault();
            //Get IdEnrollment that assign to this Student
            int studentIdEnroll = context.Student.Where(s => s.IndexNumber == delete.IndexNumber).Select(s1 => s1.IdEnrollment).FirstOrDefault();

            if (studentIndex == null)
            {
                return NotFound("Student does not Exist!");
            }
            else {

                //remove student from Db
                context.Student.Remove(context.Student.Where(s => s.IndexNumber == studentIndex).First());
            }
           
            //count how many Stundents are assigned to this idEnrollment
            int countSameIdEnroll = context.Student.Where(s => s.IdEnrollment == studentIdEnroll).Count();


            if (countSameIdEnroll == 1)
            {
                //remove Enrollment
                context.Enrollment.Remove(context.Enrollment.Where(e => e.IdEnrollment == studentIdEnroll).First());
            }
            context.SaveChanges();
            return Ok("Student has been deleted from Database!");
          
        }

        public IActionResult EnrollStudent(EnrollStudent enrollStudent)
        {
            //Search for Studies Id
            var studiesId = context.Studies.Where(s => s.Name == enrollStudent.StadiesName).Select(s1 => s1.IdStudy).FirstOrDefault();
            //Checks if the Studies Name exists
            if (studiesId == 0) {
                return NotFound("Name of Study does not Exist!");
            }


            //Search for IdEnrollment with the same Date, Semester and Studies name
            int enrollExist = 0;
            enrollExist = context.Enrollment.Where(enroll => enroll.StartDate == DateTime.Today
                                                                 && enroll.IdStudy == studiesId
                                                                 && enroll.Semester == enrollStudent.Semester
                                                          ).Select(e => e.IdEnrollment).FirstOrDefault();
            //if record exists - assign new Enrollment as old one (that exists)
            if (enrollExist != 0)
            {
                newEnroll = context.Enrollment.Where(e => e.IdEnrollment == enrollExist).First();
            }
            else{
                //create new Endrollment
                if (context.Enrollment.Where(e => e.IdEnrollment > 0).Count() == 0)
                {
                    newEnroll.IdEnrollment = 1;
                }
                else {
                    newEnroll.IdEnrollment = context.Enrollment.Max(e => e.IdEnrollment) + 1;
                }
                newEnroll.Semester = enrollStudent.Semester;
                newEnroll.IdStudy = studiesId;
                newEnroll.StartDate = DateTime.Now;
            }
            //Search for the dame indexNumber
            int indexNumberExist = context.Student.Where(s => s.IndexNumber == enrollStudent.IndexNumber).Count();
            if (indexNumberExist > 0) {
                return BadRequest("Try to enter enother IndexNumber for this student!\n" +
                    $"The last enrolled student has IndexNumber : {context.Student.OrderBy(s1 => s1.IndexNumber).Select(s => s.IndexNumber).Last().ToString()}");
            }

                     
            newStudent.IndexNumber = enrollStudent.IndexNumber;
            newStudent.FirstName = enrollStudent.FirstName;
            newStudent.LastName = enrollStudent.LastName;
            newStudent.BirthDate = enrollStudent.BirthDate;
            newStudent.IdEnrollment = newEnroll.IdEnrollment;

            //context.Enrollment.Add(newEnroll);
            newStudent.IdEnrollmentNavigation = newEnroll;

            //Checks if such a Student exists
            string studentExist = context.Student.Where(s => s.FirstName == newStudent.FirstName
                                                       && s.LastName == newStudent.LastName
                                                       && s.BirthDate == newStudent.BirthDate).Select(s1 => s1.IndexNumber).FirstOrDefault();

            //Adds new Student to Db
            if (studentExist == null)
            {
                context.Student.Add(newStudent);
                context.SaveChanges();
            }
            else
            {
                return BadRequest($"This Student already exists INdexNumber : {studentExist}");
            }
            return Ok($"A new Student has been added IndexNUmber : {newStudent.IndexNumber}");
   
        }
    }
}
