using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class PageCheck : PageBase
	{
	    public Literal LtlMessage;
        public TextBox tbRedoRemark;

        private int _channelId;
        private int _contentId;
	    private string _listPageUrl;

        public static string GetRedirectUrl(int siteId, int channelId, int contentId, string listPageUrl)
        {
            return $"{nameof(PageCheck)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}&returnUrl={HttpUtility.UrlEncode(listPageUrl)}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _contentId = Utils.ToInt(Request.QueryString["contentId"]);
            _listPageUrl = Request.QueryString["listPageUrl"];
        }

        public void Redo_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbRedoRemark.Text))
            {
                LtlMessage.Text = Utils.GetMessageHtml("要求返工失败，必须填写意见！", false);
                return;
            }
            try
            {
                var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

                var remarkInfo = new RemarkInfo(0, SiteId, contentInfo.ChannelId, contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.Redo), tbRedoRemark.Text, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                Main.RemarkDao.Insert(remarkInfo);

                ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Redo), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);

                contentInfo.Set(ContentAttribute.State, EGovInteractStateUtils.GetValue(EGovInteractState.Redo));
                Main.Instance.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

                LtlMessage.Text = Utils.GetMessageHtml("要求返工成功", true);

                var configInfo = Main.Instance.GetConfigInfo(SiteId);

                if (!configInfo.ApplyIsOpenWindow)
                {
                    Response.Redirect(_listPageUrl);
                }
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
            }
        }

        public void Check_OnClick(object sender, EventArgs e)
        {
            try
            {
                var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

                ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Check), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);

                contentInfo.Set(ContentAttribute.State, EGovInteractStateUtils.GetValue(EGovInteractState.Checked));
                Main.Instance.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

                LtlMessage.Text = Utils.GetMessageHtml("审核申请成功", true);

                var configInfo = Main.Instance.GetConfigInfo(SiteId);

                if (!configInfo.ApplyIsOpenWindow)
                {
                    Response.Redirect(_listPageUrl);
                }
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
            }
        }
	}
}
