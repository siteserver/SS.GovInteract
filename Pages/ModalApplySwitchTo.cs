using SS.GovInteract.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class ModalApplySwitchTo : PageBase
    {
        public Literal LtlMessage;

        protected TextBox tbSwitchToRemark;
        public HtmlControl divAddDepartment;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private int _channelId;
        private List<int> _idArrayList;
        private IAdministratorInfo _adminInfo;

	    public static string GetOpenWindowString(int siteId, int channelId)
	    {
            return LayerUtils.GetOpenScript("转办办件", $"{nameof(ModalApplySwitchTo)}.aspx?siteId={siteId}&channelId={channelId}", 500, 500);
	    }

	    public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _idArrayList = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
            _adminInfo = Main.Instance.AdminApi.GetAdminInfoByUserId(AuthRequest.AdminId);

            if (!IsPostBack)
			{
                divAddDepartment.Attributes.Add("onclick", ModalConfigDepartments.GetOpenWindowString(SiteId, _channelId));
                ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(_adminInfo.DepartmentId);
                ltlUserName.Text = _adminInfo.DisplayName;
			}
		}

        public void Submit_OnClick(object sender, EventArgs e)
        {
			var isChanged = false;
				
            try
            {
                var switchToDepartmentID = Utils.ToInt(Request.Form["switchToDepartmentID"]);
                if (switchToDepartmentID == 0)
                {
                    LtlMessage.Text = Utils.GetMessageHtml("转办失败，必须选择转办部门！", false);
                    return;
                }
                var switchToDepartmentName = DepartmentManager.GetDepartmentName(switchToDepartmentID);

                foreach (int contentID in _idArrayList)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, contentID);
                    var state = EStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));

                    if (state != EState.Denied && state != EState.Checked)
                    {
                        contentInfo.Set(ContentAttribute.DepartmentId, switchToDepartmentID.ToString());
                        Main.Instance.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);

                        if (!string.IsNullOrEmpty(tbSwitchToRemark.Text))
                        {
                            var remarkInfo = new RemarkInfo(0, SiteId, contentInfo.ChannelId, contentID, ERemarkTypeUtils.GetValue(ERemarkType.SwitchTo), tbSwitchToRemark.Text, _adminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                            Main.Instance.RemarkDao.Insert(remarkInfo);
                        }

                        ApplyManager.LogSwitchTo(SiteId, contentInfo.ChannelId, contentID, switchToDepartmentName, AuthRequest.AdminName, _adminInfo.DepartmentId);
                    }
                }

                isChanged = true;
            }
			catch(Exception ex)
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
