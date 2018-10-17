using SS.GovInteract.Core;
using System;
using System.Web.UI.WebControls;
using SS.GovInteract.Controls;
using SS.GovInteract.Model;
using SS.GovInteract.Provider;

namespace SS.GovInteract.Pages
{
    public class PageListAll : PageBaseList
    {
        public DropDownList DdlTaxis;
        public DropDownList DdlState;
        public DateTimeTextBox TbDateFrom;
        public DateTimeTextBox TbDateTo;
        public TextBox TbKeyword;

        public static string GetRedirectUrl(int siteId, int channelId)
        {
            return $"{nameof(PageListAll)}.aspx?siteId={siteId}&channelId={channelId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            EBooleanUtils.AddListItems(DdlTaxis, "倒序", "正序");
            if (Request.QueryString["isTaxisDESC"] != null)
            {
                var isTaxisDesc = Utils.ToBool(Request.QueryString["isTaxisDESC"]);
                Utils.SelectSingleItemIgnoreCase(DdlTaxis, isTaxisDesc.ToString());
            }
            var listItem = new ListItem("全部", string.Empty);
            DdlState.Items.Add(listItem);
            EStateUtils.AddListItems(DdlState);
            if (Request.QueryString["state"] != null)
            {
                Utils.SelectSingleItemIgnoreCase(DdlState, Request.QueryString["state"]);
            }
            TbDateFrom.Text = Request.QueryString["dateFrom"];
            TbDateTo.Text = Request.QueryString["dateTo"];
            TbKeyword.Text = Request.QueryString["keyword"];
        }

        public void Search_OnClick(object sender, EventArgs e)
        {
            Utils.Redirect(PageUrl);
        }

        protected override string GetSelectString()
        {
            if (Request.QueryString["state"] != null || Request.QueryString["dateFrom"] != null || Request.QueryString["dateTo"] != null || Request.QueryString["keyword"] != null)
            {
                return ContentDao.GetSelectString(SiteId, ChannelId, Request.QueryString["state"], Request.QueryString["dateFrom"], Request.QueryString["dateTo"], Request.QueryString["keyword"]);
            }
            return ContentDao.GetSelectString(SiteId, ChannelId);
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
                    _pageUrl = GetRedirectUrl(SiteId, ChannelId) + $"&isTaxisDESC={DdlTaxis.SelectedValue}&state={DdlState.SelectedValue}&dateFrom={TbDateFrom.Text}&dateTo={TbDateTo.Text}&keyword={TbKeyword.Text}&page={Utils.ToInt(Request.QueryString["page"], 1)}";
                }
                return _pageUrl;
            }
        }
    }
}
