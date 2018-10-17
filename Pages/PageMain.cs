using System;
using System.Web;
using SiteServer.Plugin;

namespace SS.GovInteract.Pages
{
    public class PageMain : PageBase
    {
        public static string GetRedirectUrl(int siteId, string linkUrl)
        {
            return $"{nameof(PageMain)}.aspx?siteId={siteId}&linkUrl={HttpUtility.UrlEncode(linkUrl)}";
        }

        public string ContentModelPluginId => Main.PluginId;

        public string LinkUrl => Main.PluginApi.GetPluginUrl(Main.PluginId, Request.QueryString["linkUrl"]);

        public string AdminUrl => Main.UtilsApi.GetAdminDirectoryUrl(string.Empty);

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
