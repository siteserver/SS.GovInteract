using System;
using System.Text;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;
using SS.GovInteract.Provider;

namespace SS.GovInteract.Pages
{
    public class ModalDepartmentSelectSingle : PageBase
    {
        public Literal LtlDepartmentTree; 

        private int _channelId;

        public static string GetOpenWindowString(int siteId, int channelId)
        {
            return LayerUtils.GetOpenScript("负责部门设置", $"{nameof(ModalDepartmentSelectSingle)}.aspx?siteId={siteId}&channelId={channelId}", 700, 0);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]); 

            if (!IsPostBack && _channelId > 0)
            {
                var channelInfo = ChannelDao.GetChannelInfo(SiteId, _channelId);
                LtlDepartmentTree.Text = GetDepartmentTreeHtml(channelInfo);
            }
        }

        private static string GetDepartmentTreeHtml(ChannelInfo channelInfo)
        { 
            var htmlBuilder = new StringBuilder();
            if (channelInfo == null)
            {
                return htmlBuilder.ToString();
            }
            var departmentIdList =InteractManager.GetDepartmentIdList(channelInfo);  

            foreach (var departmentId in departmentIdList)
            {
                var departmentInfo = DepartmentManager.GetDepartmentInfo(departmentId);
                if (departmentInfo == null) continue;

                htmlBuilder.Append($@"
<span class=""radio radio-primary"" style=""padding-left: 0px;"">
    <input type=""radio"" id=""departmentId_{departmentInfo.Id}"" name=""departmentId"" value=""{departmentInfo.Id}"" />
    <label for=""departmentId_{departmentInfo.Id}""> {departmentInfo.DepartmentName} </label>
</span>
");
                htmlBuilder.Append("<br/>");
            }

            return htmlBuilder.ToString();
        }
         
        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (Page.IsPostBack && Page.IsValid)
            {
                var departmentId = Utils.ToInt(Request.Form["departmentId"]);
                var departmentName = DepartmentManager.GetDepartmentName(departmentId);
                LayerUtils.CloseWithoutRefresh(Page, $"parent.departmentSelect('{departmentName}', {departmentId})");
            }
        }
    }
}
