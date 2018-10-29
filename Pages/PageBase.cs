using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageBase : Page
    {
        public Literal LtlMessage;

        public int SiteId { get; private set; }

        public List<IChannelInfo> ChannelInfoList { get; private set; }

        public IRequest AuthRequest { get; private set; }

        private ConfigInfo _configInfo;

        public ConfigInfo ConfigInfo => _configInfo ?? (_configInfo = Main.GetConfigInfo(SiteId));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AuthRequest = SiteServer.Plugin.Context.GetCurrentRequest();

            SiteId = AuthRequest.GetQueryInt("siteId");

            if (!AuthRequest.AdminPermissions.HasSitePermissions(SiteId, Main.PluginId))
            {
                HttpContext.Current.Response.Write("<h1>未授权访问</h1>");
                HttpContext.Current.Response.End();
            }

            ChannelInfoList = InteractManager.GetInteractChannelInfoList(SiteId);

            if (ChannelInfoList.Count == 0)
            {
                Utils.Redirect(PageInit.GetRedirectUrl(SiteId, Request.RawUrl));
            }
        }
    }
}
