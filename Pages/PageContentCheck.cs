using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;
using SS.GovInteract.Provider;

namespace SS.GovInteract.Pages
{
	public class PageContentCheck : PageBaseContent
    {
        public TextBox TbRedoRemark;

        private int _channelId;
        private int _contentId;
	    private string _returnUrl;
        private IAdministratorInfo _adminInfo;

        public static string GetRedirectUrl(int siteId, int channelId, int contentId, string listPageUrl)
        {
            return $"{nameof(PageContentCheck)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}&returnUrl={HttpUtility.UrlEncode(listPageUrl)}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _contentId = Utils.ToInt(Request.QueryString["contentId"]);
            _returnUrl = Request.QueryString["returnUrl"];
            _adminInfo = Main.AdminApi.GetAdminInfoByUserId(AuthRequest.AdminId);
        }

        public void Redo_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TbRedoRemark.Text))
            {
                LtlMessage.Text = Utils.GetMessageHtml("要求返工失败，必须填写意见！", false);
                return;
            }
            try
            {
                var contentInfo = Main.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

                var remarkInfo = new RemarkInfo(0, SiteId, contentInfo.ChannelId, contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.Redo), TbRedoRemark.Text, _adminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                RemarkDao.Insert(remarkInfo);

                ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Redo), AuthRequest.AdminName, _adminInfo.DepartmentId);

                contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Redo));
                Main.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

                LtlMessage.Text = Utils.GetMessageHtml("要求返工成功", true);

                var configInfo = Main.GetConfigInfo(SiteId);

                if (!configInfo.ApplyIsOpenWindow)
                {
                    Utils.Redirect(_returnUrl);
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
                var contentInfo = Main.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

                ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Check), AuthRequest.AdminName, _adminInfo.DepartmentId);

                contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Checked));
                Main.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

                LtlMessage.Text = Utils.GetMessageHtml("审核申请成功", true);

                var configInfo = Main.GetConfigInfo(SiteId);

                if (!configInfo.ApplyIsOpenWindow)
                {
                    Utils.Redirect(_returnUrl);
                }
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
            }
        }
	}
}
