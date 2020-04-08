using System;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/departments")]
    public class PagesDepartmentsController : ApiController
    {
        private const string Route = "";
        private const string RouteId = "{id:int}";

        [HttpGet, Route(Route)]
        public IHttpActionResult List()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                return Ok(new
                {
                    Value = DepartmentManager.GetDepartmentInfoList(siteId)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route(RouteId)]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                Main.DepartmentRepository.Delete(siteId, id);

                return Ok(new
                {
                    Value = DepartmentManager.GetDepartmentInfoList(siteId)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
