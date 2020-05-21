using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut9.Models;
using tut9.Services;

namespace tut9.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsControllers : ControllerBase
    {

        public readonly s19312Context _context;
        public readonly IStudentDbService _service;
        public StudentsControllers(s19312Context context, IStudentDbService service) {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetStudents() {
            return Ok(_context.Student.ToList());
        }

        [HttpPost("enrollment")]
        public IActionResult InrollStudent(EnrollStudent enroll) {
            return _service.EnrollStudent(enroll);
        }

        [HttpPost("promotion")]
        public IActionResult PromoteStudent(PromoteStudent promote) {
            return _service.PromoteStudent(promote);
        }

        [HttpDelete("delete")]
        public IActionResult DeleteStudent(DeleteStudent IndexNumber) {
            return _service.DeleteStudent(IndexNumber);
        }
    }
}
