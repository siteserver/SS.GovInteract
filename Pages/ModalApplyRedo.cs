using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class ModalApplyRedo : PageBase
    {
        public Literal LtlMessage;

        protected TextBox tbRedoRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private int _channelId;
        private List<int> _idArrayList;

        public static string GetOpenWindowString(int siteId, int channelId)
        {
            return LayerUtils.GetOpenScript("要求返工", $"{nameof(ModalApplyRedo)}.aspx?siteId={siteId}&channelId={channelId}", 450, 320);
        }
        
		public void Page_Load(object sender, EventArgs e)
		{
		    _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _idArrayList = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(AuthRequest.AdminInfo.DepartmentId);
                ltlUserName.Text = AuthRequest.AdminInfo.DisplayName;
			}
		}

        public void Submit_OnClick(object sender, EventArgs e)
        {
			var isChanged = false;
				
            try
            {
                if (string.IsNullOrEmpty(tbRedoRemark.Text))
                {
                    LtlMessage.Text = Utils.GetMessageHtml("要求返工失败，必须填写意见！", false);
                    return;
                }

                foreach (int contentID in _idArrayList)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, contentID);
                    var state = EStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));

                    if (state == EState.Replied || state == EState.Redo)
                    {
                        var remarkInfo = new RemarkInfo(0, SiteId, contentInfo.ChannelId, contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.Redo), tbRedoRemark.Text, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                        Main.RemarkDao.Insert(remarkInfo);

                        ApplyManager.Log(SiteId, contentInfo.ChannelId, contentID, ELogTypeUtils.GetValue(ELogType.Redo), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);
                        contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Redo));
                        Main.Instance.ContentApi.Update(SiteId, contentInfo.ChannelId, contentInfo);
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
