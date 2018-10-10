using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class ModalApplyComment : PageBase
    {
        public Literal LtlMessage;

        protected TextBox tbCommentRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private int _channelId;
        private List<int> _idArrayList;
        private IAdministratorInfo _adminInfo;

        public static string GetOpenWindowString(int siteId, int channelId)
        {
            return LayerUtils.GetOpenScript("批示", $"{nameof(ModalApplyComment)}.aspx?siteId={siteId}&channelId={channelId}", 450, 320);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _idArrayList = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
            _adminInfo = Main.Instance.AdminApi.GetAdminInfoByUserId(AuthRequest.AdminId);

            if (!IsPostBack)
			{
                ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(_adminInfo.DepartmentId);
                ltlUserName.Text = _adminInfo.DisplayName;
			}
		}

        public void Submit_OnClick(object sender, EventArgs e)
        {
			var isChanged = false;
				
            try
            {
                if (string.IsNullOrEmpty(tbCommentRemark.Text))
                {
                    LtlMessage.Text = Utils.GetMessageHtml("批示失败，必须填写意见！", false);
                    return;
                }

                foreach (int contentID in _idArrayList)
                {
                    var remarkInfo = new RemarkInfo(0, SiteId, _channelId, contentID, ERemarkTypeUtils.GetValue(ERemarkType.Comment), tbCommentRemark.Text, _adminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                    Main.Instance.RemarkDao.Insert(remarkInfo);

                    ApplyManager.Log(SiteId, _channelId, contentID, ELogTypeUtils.GetValue(ELogType.Comment), AuthRequest.AdminName, _adminInfo.DepartmentId);
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
