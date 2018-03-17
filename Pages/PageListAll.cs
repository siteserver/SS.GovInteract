using SS.GovInteract.Core;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SS.GovInteract.Controls;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageListAll : PageBaseList
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlState;
        public DateTimeTextBox tbDateFrom;
        public DateTimeTextBox tbDateTo;
        public TextBox tbKeyword;

        public static string GetRedirectUrl(int siteId, int channelId)
        {
            return $"{nameof(PageListAll)}.aspx?siteId={siteId}&channelId={channelId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            EBooleanUtils.AddListItems(ddlTaxis, "倒序", "正序");
            if (Request.QueryString["isTaxisDESC"] != null)
            {
                var isTaxisDesc = Utils.ToBool(Request.QueryString["isTaxisDESC"]);
                Utils.SelectSingleItemIgnoreCase(ddlTaxis, isTaxisDesc.ToString());
            }
            var listItem = new ListItem("全部", string.Empty);
            ddlState.Items.Add(listItem);
            EGovInteractStateUtils.AddListItems(ddlState);
            if (Request.QueryString["state"] != null)
            {
                Utils.SelectSingleItemIgnoreCase(ddlState, Request.QueryString["state"]);
            }
            tbDateFrom.Text = Request.QueryString["dateFrom"];
            tbDateTo.Text = Request.QueryString["dateTo"];
            tbKeyword.Text = Request.QueryString["keyword"];
        }

        public void Search_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(PageUrl);
        }

        protected override string GetSelectString()
        {
            if (Request.QueryString["state"] != null || Request.QueryString["dateFrom"] != null || Request.QueryString["dateTo"] != null || Request.QueryString["keyword"] != null)
            {
                return Main.ContentDao.GetSelectString(SiteId, _channelId, Request.QueryString["state"], Request.QueryString["dateFrom"], Request.QueryString["dateTo"], Request.QueryString["keyword"]);
            }
            else
            {
                return Main.ContentDao.GetSelectString(SiteId, _channelId);
            }
        }

        protected string GetSortMode()
        {
            var isTaxisDesc = true;
            if (!string.IsNullOrEmpty(Request.QueryString["isTaxisDESC"]))
            {
                isTaxisDesc = Utils.ToBool(Request.QueryString["isTaxisDESC"]);
            }
            return isTaxisDesc ? "DESC" : "ASC";
        }

        private string _pageUrl;

        protected override string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_pageUrl))
                {
                    return GetRedirectUrl(SiteId, _channelId) + $"&isTaxisDESC={ddlTaxis.SelectedValue}&state={ddlState.SelectedValue}&dateFrom={tbDateFrom.Text}&dateTo={tbDateTo.Text}&keyword={tbKeyword.Text}&page={Utils.ToInt(Request.QueryString["page"], 1)}";
                }
                return _pageUrl;
            }
        }
    }
}
