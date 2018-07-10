using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class ModalApplyTranslate : PageBase
    {
        public Literal LtlMessage;

        public DropDownList ddlNodeID;
        protected TextBox tbTranslateRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private int _channelId;
        private List<int> _idArrayList;

	    public static string GetOpenWindowString(int siteId, int channelId)
	    {
            return LayerUtils.GetOpenScript("转移办件", $"{nameof(ModalApplyTranslate)}.aspx?siteId={siteId}&channelId={channelId}", 450, 320);
	    }

	    public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _idArrayList = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);

            if (!IsPostBack)
            {
                foreach (var nodeInfo in ChannelInfoList)
                {
                    if (nodeInfo.Id != _channelId)
                    {
                        var listItem = new ListItem(nodeInfo.ChannelName, nodeInfo.Id.ToString());
                        ddlNodeID.Items.Add(listItem);
                    }
                }

                ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(AuthRequest.AdminInfo.DepartmentId);
                ltlUserName.Text = AuthRequest.AdminInfo.DisplayName;
            }
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            var isChanged = false;

            try
            {
                var translateNodeID = Utils.ToInt(ddlNodeID.SelectedValue);
                if (translateNodeID == 0)
                {
                    LtlMessage.Text = Utils.GetMessageHtml("转移失败，必须选择转移目标！", false);
                    return;
                }

                var chananelInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, _channelId);

                foreach (int contentID in _idArrayList)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, contentID);
                    contentInfo.Set(ContentAttribute.TranslateFromChannelId, contentInfo.ChannelId.ToString());
                    contentInfo.ChannelId = translateNodeID;

                    Main.Instance.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

                    if (!string.IsNullOrEmpty(tbTranslateRemark.Text))
                    {
                        var remarkInfo = new RemarkInfo(0, SiteId, contentInfo.ChannelId, contentID, ERemarkTypeUtils.GetValue(ERemarkType.Translate), tbTranslateRemark.Text, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                        Main.Instance.RemarkDao.Insert(remarkInfo);
                    }

                    ApplyManager.LogTranslate(SiteId, contentInfo.ChannelId, contentID, chananelInfo.ChannelName, AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
                isChanged = false;
            }

            if (isChanged)
            {
                LayerUtils.Close(Page);
            }
        }
	}
}
