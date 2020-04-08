using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/departmentsLayerAdd")]
    public class PagesDepartmentsLayerAddController : ApiController
    {
        private const string Route = "";
        private const string RouteId = "{id:int}";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var departmentInfo = new DepartmentInfo();

                var allUserNames = new List<string>();
                foreach (var userName in Context.AdminApi.GetUserNameList())
                {
                    var permissions = Context.AdminApi.GetPermissions(userName);
                    if (permissions.IsSiteAdmin(siteId)) continue;

                    allUserNames.Add(userName);
                }

                return Ok(new
                {
                    Value = departmentInfo,
                    allUserNames
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route(RouteId)]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var departmentInfo = DepartmentManager.GetDepartmentInfo(siteId, id);

                var allUserNames = new List<string>();
                foreach (var userName in Context.AdminApi.GetUserNameList())
                {
                    var permissions = Context.AdminApi.GetPermissions(userName);
                    if (permissions.IsSiteAdmin(siteId)) continue;

                    allUserNames.Add(userName);
                }

                return Ok(new
                {
                    Value = departmentInfo,
                    allUserNames
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Insert()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var departmentInfo = new DepartmentInfo
                {
                    Id = 0,
                    SiteId = siteId,
                    DepartmentName = request.GetPostString("departmentName"),
                    UserNames = request.GetPostString("userNames").Trim(','),
                    Taxis = request.GetPostInt("taxis")
                };

                departmentInfo.Id = Main.DepartmentRepository.Insert(departmentInfo);

                return Ok(new
                {
                    Value = departmentInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route(RouteId)]
        public IHttpActionResult Update(int id)
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var departmentInfo = DepartmentManager.GetDepartmentInfo(siteId, id);
                departmentInfo.DepartmentName = request.GetPostString("departmentName");
                departmentInfo.UserNames = request.GetPostString("userNames").Trim(',');
                departmentInfo.Taxis = request.GetPostInt("taxis");

                Main.DepartmentRepository.Update(departmentInfo);

                return Ok(new
                {
                    Value = departmentInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
