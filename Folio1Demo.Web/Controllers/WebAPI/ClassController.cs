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
using System.Web.Http.Results;

namespace Folio1Demo.Web.Controllers.WebAPI
{
    [RoutePrefix("api/class")]
    public class ClassController : ApiController
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [Route("classes")]
        public IEnumerable<ClassViewModel> GetClassess()
        {
            var dbClassess = _classService.GetClassess();
            return dbClassess.Select(s => EntityMapper.MapFromDbEntity(s));

        }

        [Route("remove/{classId}")]
        [HttpPost]
        public async Task DeleteClass(int classId)
        {
            await _classService.Delete(classId);
        }

        [Route("save")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveClass(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dbClass = EntityMapper.MapToDbEntity(model);
                    await _classService.Save(dbClass);
                    return new HttpResponseMessage(HttpStatusCode.Created);
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
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, string.Join("  ", messages.ToArray()));
            }
        }


    }
}
