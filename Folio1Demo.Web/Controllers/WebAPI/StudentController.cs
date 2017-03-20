using Folio1Demo.Web.Infrastructure;
using Folio1Demo.Web.Models;
using School.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Folio1Demo.Web.Controllers.WebAPI
{
    [RoutePrefix("api/student")]
    public class StudentController : ApiController
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [Route("students/{classId}")]
        public IEnumerable<Models.StudentViewModel> GetStudents(int classId)
        {
            var dbStudents = _studentService.GetStudents(classId);
            return dbStudents.Select(s => EntityMapper.MapFromDbEntity(s));
        }

        [Route("remove/{studentId}")]
        [HttpPost]
        public async Task DeleteStudent(int studentId)
        {
            await _studentService.Delete(studentId);
        }

        [Route("save")]
        [HttpPost]
        public async Task<HttpResponseMessage> Save(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dbStudent = EntityMapper.MapToDbEntity(model);
                    var status = await _studentService.Save(dbStudent);

                    switch (status)
                    {
                        case StudentStatus.DuplicateStudent:
                            return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Student with this last name already exist.");
                        case StudentStatus.StudentAdded:
                        case StudentStatus.StudentEdited:
                            return new HttpResponseMessage(HttpStatusCode.OK);
                        default:
                            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Error ocurred while saving student details.");
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
            }
            else
            {
                var messages = from state in ModelState.Values
                               from error in state.Errors
                               where !string.IsNullOrEmpty(error.ErrorMessage)
                               select error.ErrorMessage;
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, string.Join(" ", messages.ToArray()));
            }
        }


    }
}
