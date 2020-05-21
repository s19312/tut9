using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut9.Models;

namespace tut9.Services
{
    public interface IStudentDbService
    {
       IActionResult EnrollStudent(EnrollStudent enrollStudent);
       IActionResult DeleteStudent(DeleteStudent indexNumber);
        IActionResult PromoteStudent(PromoteStudent promote);

    }
}
