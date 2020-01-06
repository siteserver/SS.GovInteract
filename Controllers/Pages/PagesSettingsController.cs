using System;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/settings")]
    public class PagesSettingsController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var settings = ApplicationUtils.GetSettings(siteId);

                return Ok(new
                {
                    Value = settings
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId))
                    return Unauthorized();

                var settings = ApplicationUtils.GetSettings(siteId);

                var type = request.GetPostString("type");
                if (ApplicationUtils.EqualsIgnoreCase(type, nameof(Settings.IsClosed)))
                {
                    settings.IsClosed = request.GetPostBool(nameof(Settings.IsClosed).ToCamelCase());
                    Context.ConfigApi.SetConfig(ApplicationUtils.PluginId, siteId, settings);
                }
                else if (ApplicationUtils.EqualsIgnoreCase(type, nameof(Settings.DaysWarning)))
                {
                    settings.DaysWarning = request.GetPostInt(nameof(Settings.DaysWarning).ToCamelCase());
                    Context.ConfigApi.SetConfig(ApplicationUtils.PluginId, siteId, settings);
                }
                else if (ApplicationUtils.EqualsIgnoreCase(type, nameof(Settings.DaysDeadline)))
                {
                    settings.DaysDeadline = request.GetPostInt(nameof(Settings.DaysDeadline).ToCamelCase());
                    Context.ConfigApi.SetConfig(ApplicationUtils.PluginId, siteId, settings);
                }
                else if (ApplicationUtils.EqualsIgnoreCase(type, nameof(Settings.IsDeleteAllowed)))
                {
                    settings.IsDeleteAllowed = request.GetPostBool(nameof(Settings.IsDeleteAllowed).ToCamelCase());
                    Context.ConfigApi.SetConfig(ApplicationUtils.PluginId, siteId, settings);
                }
                else if (ApplicationUtils.EqualsIgnoreCase(type, nameof(Settings.IsSelectCategory)))
                {
                    settings.IsSelectCategory = request.GetPostBool(nameof(Settings.IsSelectCategory).ToCamelCase());
                    Context.ConfigApi.SetConfig(ApplicationUtils.PluginId, siteId, settings);
                }
                else if (ApplicationUtils.EqualsIgnoreCase(type, nameof(Settings.IsSelectDepartment)))
                {
                    settings.IsSelectDepartment = request.GetPostBool(nameof(Settings.IsSelectDepartment).ToCamelCase());
                    Context.ConfigApi.SetConfig(ApplicationUtils.PluginId, siteId, settings);
                }

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
