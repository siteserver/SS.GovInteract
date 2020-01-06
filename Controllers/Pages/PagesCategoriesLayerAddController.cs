using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/categoriesLayerAdd")]
    public class PagesCategoriesLayerAddController : ApiController
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
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var categoryInfo = new CategoryInfo();

                var allUserNames = new List<string>();
                foreach (var userName in Context.AdminApi.GetUserNameList())
                {
                    var permissions = Context.AdminApi.GetPermissions(userName);
                    if (permissions.IsSiteAdmin(siteId)) continue;

                    allUserNames.Add(userName);
                }

                return Ok(new
                {
                    Value = categoryInfo,
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
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var categoryInfo = CategoryManager.GetCategoryInfo(siteId, id);

                var allUserNames = new List<string>();
                foreach (var userName in Context.AdminApi.GetUserNameList())
                {
                    var permissions = Context.AdminApi.GetPermissions(userName);
                    if (permissions.IsSiteAdmin(siteId)) continue;

                    allUserNames.Add(userName);
                }

                return Ok(new
                {
                    Value = categoryInfo,
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
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var categoryInfo = new CategoryInfo
                {
                    Id = 0,
                    SiteId = siteId,
                    CategoryName = request.GetPostString("categoryName"),
                    UserNames = request.GetPostString("userNames").Trim(','),
                    Taxis = request.GetPostInt("taxis")
                };

                categoryInfo.Id = Main.CategoryRepository.Insert(categoryInfo);

                return Ok(new
                {
                    Value = categoryInfo
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
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var categoryInfo = CategoryManager.GetCategoryInfo(siteId, id);
                categoryInfo.CategoryName = request.GetPostString("categoryName");
                categoryInfo.UserNames = request.GetPostString("userNames").Trim(',');
                categoryInfo.Taxis = request.GetPostInt("taxis");

                Main.CategoryRepository.Update(categoryInfo);

                return Ok(new
                {
                    Value = categoryInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
