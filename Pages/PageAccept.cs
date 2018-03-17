using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageAccept : PageBase
	{
        public Literal LtlMessage;
        public TextBox tbAcceptRemark;
        public TextBox tbDenyReply;

        private int _channelId;
        private int _contentId;
        private string _listPageUrl;

        public static string GetRedirectUrl(int siteId, int channelId, int contentId, string listPageUrl)
        {
            return $"{nameof(PageAccept)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}&returnUrl={HttpUtility.UrlEncode(listPageUrl)}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _contentId = Utils.ToInt(Request.QueryString["contentId"]);
            _listPageUrl = Request.QueryString["listPageUrl"];
        }

        public void Accept_OnClick(object sender, EventArgs e)
        {
            var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

            var remarkInfo = new RemarkInfo(0, SiteId, contentInfo.ChannelId, contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.Accept), tbAcceptRemark.Text, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
            Main.RemarkDao.Insert(remarkInfo);

            ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Accept), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);

            contentInfo.Set(ContentAttribute.State, EGovInteractStateUtils.GetValue(EGovInteractState.Accepted));
            Main.Instance.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

            LtlMessage.Text = Utils.GetMessageHtml("申请受理成功", true);

            var configInfo = Main.Instance.GetConfigInfo(SiteId);

            if (!configInfo.ApplyIsOpenWindow)
            {
                Response.Redirect(_listPageUrl);
            }
        }

        public void Deny_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbDenyReply.Text))
            {
                LtlMessage.Text = Utils.GetMessageHtml("拒绝失败，必须填写拒绝理由", false);
                return;
            }

            var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, _contentId);

            Main.ReplyDao.DeleteByContentId(SiteId, contentInfo.Id);

            var replyInfo = new ReplyInfo
            {
                SiteId = SiteId,
                ChannelId = contentInfo.ChannelId,
                ContentId = contentInfo.Id,
                Reply = tbDenyReply.Text,
                DepartmentId = AuthRequest.AdminInfo.DepartmentId,
                UserName = AuthRequest.AdminName,
                AddDate = DateTime.Now
            };
            Main.ReplyDao.Insert(replyInfo);

            ApplyManager.Log(SiteId, contentInfo.ChannelId, contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Deny), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);

            contentInfo.Set(ContentAttribute.State, EGovInteractStateUtils.GetValue(EGovInteractState.Denied));
            Main.Instance.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

            LtlMessage.Text = Utils.GetMessageHtml("拒绝申请成功", true);

            var configInfo = Main.Instance.GetConfigInfo(SiteId);

            if (!configInfo.ApplyIsOpenWindow)
            {
                Response.Redirect(_listPageUrl);
            }
        }
	}
}
