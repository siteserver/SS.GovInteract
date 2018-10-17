using System;
using System.Web;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageContentAccept : PageBaseContent
    {
        public TextBox TbAcceptRemark;
        public TextBox TbDenyReply;

        private int _channelId;
        private int _contentId;
        private string _returnUrl;
        private IAdministratorInfo _adminInfo;

        public static string GetRedirectUrl(int siteId, int channelId, int contentId, string listPageUrl)
        {
            return $"{nameof(PageContentAccept)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}&returnUrl={HttpUtility.UrlEncode(listPageUrl)}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _contentId = Utils.ToInt(Request.QueryString["contentId"]);
            _returnUrl = Request.QueryString["returnUrl"];
            _adminInfo = Main.AdminApi.GetAdminInfoByUserId(AuthRequest.AdminId);
        }

        public void Accept_OnClick(object sender, EventArgs e)
        {
            var contentInfo = Main.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

            var remarkInfo = new RemarkInfo(0, SiteId, contentInfo.ChannelId, contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.Accept), TbAcceptRemark.Text, _adminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
            Main.RemarkDao.Insert(remarkInfo);

            ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Accept), AuthRequest.AdminName, _adminInfo.DepartmentId);

            contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Accepted));
            Main.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

            LtlMessage.Text = Utils.GetMessageHtml("申请受理成功", true);

            var configInfo = Main.GetConfigInfo(SiteId);

            if (!configInfo.ApplyIsOpenWindow)
            {
                Utils.Redirect(_returnUrl);
            }
        }

        public void Deny_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TbDenyReply.Text))
            {
                LtlMessage.Text = Utils.GetMessageHtml("拒绝失败，必须填写拒绝理由", false);
                return;
            }

            var contentInfo = Main.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

            Main.ReplyDao.DeleteByContentId(SiteId, contentInfo.Id);

            var replyInfo = new ReplyInfo(0, SiteId, contentInfo.ChannelId, contentInfo.Id, TbDenyReply.Text,
                string.Empty, _adminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
            Main.ReplyDao.Insert(replyInfo);

            ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Deny), AuthRequest.AdminName, _adminInfo.DepartmentId);

            contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Denied));
            Main.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

            LtlMessage.Text = Utils.GetMessageHtml("拒绝申请成功", true);

            var configInfo = Main.GetConfigInfo(SiteId);

            if (!configInfo.ApplyIsOpenWindow)
            {
                Utils.Redirect(_returnUrl);
            }
        }
	}
}
