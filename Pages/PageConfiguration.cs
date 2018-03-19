using System;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageConfiguration : PageBase
    {
        public Literal LtlMessage;
        public TextBox TbApplyDateLimit; // 办理时限
        public DropDownList DdlApplyAlertDateIsAfter; // 办理时限预警  前 or 后
        public TextBox TbApplyAlertDate; // 办理时限预警
        public TextBox TbApplyYellowAlertDate; // 黄牌预警
        public TextBox TbApplyRedAlertDate; // 红牌预警
        public DropDownList DdlApplyIsDeleteAllowed; // 办件是否可删除
        public DropDownList DdlApplyIsOpenWindow; // 办件是否新窗口打开

        private ConfigInfo _configInfo;

        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageConfiguration)}.aspx?siteId={siteId}";
        }

        public void Page_Load(object sender, EventArgs e)
        { 
            _configInfo = Main.Instance.GetConfigInfo(SiteId);

            if (!IsPostBack)
            {
                TbApplyDateLimit.Text = _configInfo.ApplyDateLimit.ToString();
                Utils.SelectListItems(DdlApplyAlertDateIsAfter, (_configInfo.ApplyAlertDate > 0).ToString());
                TbApplyAlertDate.Text = _configInfo.ApplyAlertDate > 0 ? _configInfo.ApplyAlertDate.ToString() :(-_configInfo.ApplyAlertDate).ToString();
                TbApplyYellowAlertDate.Text = _configInfo.ApplyYellowAlertDate.ToString();
                TbApplyRedAlertDate.Text = _configInfo.ApplyRedAlertDate.ToString();
                Utils.SelectListItems(DdlApplyIsDeleteAllowed, _configInfo.ApplyIsDeleteAllowed.ToString());
                Utils.SelectListItems(DdlApplyIsOpenWindow, _configInfo.ApplyIsOpenWindow.ToString());
            }
        } 

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (Page.IsPostBack && Page.IsValid)
            {
                _configInfo.ApplyDateLimit = Utils.ToInt(TbApplyDateLimit.Text);

                _configInfo.ApplyAlertDate = Utils.ToInt(TbApplyAlertDate.Text);
                // 确保预警时限为正
                if (_configInfo.ApplyAlertDate < 0)
                    _configInfo.ApplyAlertDate = -_configInfo.ApplyAlertDate;
                // 如果是选择办理时限前，则再把预警时限换成负
                if (!Utils.ToBool(DdlApplyAlertDateIsAfter.SelectedValue) )
                {
                    _configInfo.ApplyAlertDate = -_configInfo.ApplyAlertDate;
                } 

                _configInfo.ApplyYellowAlertDate = Utils.ToInt(TbApplyYellowAlertDate.Text);
                _configInfo.ApplyRedAlertDate = Utils.ToInt(TbApplyRedAlertDate.Text); 
                _configInfo.ApplyIsDeleteAllowed = Utils.ToBool(DdlApplyIsDeleteAllowed.SelectedValue);
                _configInfo.ApplyIsOpenWindow = Utils.ToBool(DdlApplyIsOpenWindow.SelectedValue);

                Main.Instance.ConfigApi.SetConfig(SiteId, _configInfo);
                LtlMessage.Text = Utils.GetMessageHtml("办件预警设置修改成功！", true);
            } 
        }
    }
}