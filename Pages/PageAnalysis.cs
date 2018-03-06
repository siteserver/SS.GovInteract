using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.GovInteract.Controls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class PageAnalysis : Page
	{
	    public Literal LtlMessage;

        public Repeater RptContents;
        public SqlPager SpContents;

        public Button BtnDelete;

        private int _siteId;

        public static string GetRedirectUrl(int siteId)
        {
            return Main.Instance.PluginApi.GetPluginUrl($"{nameof(PageAnalysis)}.aspx?siteId={siteId}");
        }

		public void Page_Load(object sender, EventArgs e)
        {
            _siteId = Convert.ToInt32(Request.QueryString["siteId"]);

            if (!Main.Instance.AdminApi.IsSiteAuthorized(_siteId))
            {
                Response.Write("<h1>未授权访问</h1>");
                Response.End();
                return;
            }

            

        }
    }
}
