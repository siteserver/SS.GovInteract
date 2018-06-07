using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class ModalConfigDepartments : PageBase
    {
        public Literal LtlDepartmentTree; 

        private int channelId;

        public static string GetOpenWindowString(int siteId, int channelId)
        {
            return LayerUtils.GetOpenScript("负责部门设置", $"{nameof(ModalConfigDepartments)}.aspx?siteId={siteId}&channelId={channelId}", 700, 0);
        } 

        public void Page_Load(object sender, EventArgs e)
        {
            channelId = Utils.ToInt(Request.QueryString["channelId"]); 

            if (!IsPostBack && channelId > 0)
            {
                var channelInfo = Main.ChannelDao.GetChannelInfo(SiteId, channelId);
                LtlDepartmentTree.Text = GetDepartmentTreeHtml(channelInfo);
            }
        }

        private string GetDepartmentTreeHtml(ChannelInfo channelInfo)
        { 
            var htmlBuilder = new StringBuilder();
            if (channelInfo == null)
            {
                return htmlBuilder.ToString();
            }
            var departmentIdList =InteractManager.GetDepartmentIdList(channelInfo);  
            var treeDirectoryUrl = Main.Instance.PluginApi.GetPluginUrl("assets/tree");
            htmlBuilder.Append("<span id='DepartmentSelectControl'>");
            var allDepartmentIdList = DepartmentManager.GetDepartmentIdList();
            var isLastNodeArray = new bool[allDepartmentIdList.Count];
            foreach (var theDepartmentId in allDepartmentIdList)
            {
                var departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentId);
                htmlBuilder.Append(GetTitle(departmentInfo, treeDirectoryUrl, isLastNodeArray, departmentIdList));
                htmlBuilder.Append("<br/>");
            }
            htmlBuilder.Append("</span>");
            return htmlBuilder.ToString();
        }
         
        private string GetTitle(DepartmentInfo departmentInfo, string treeDirectoryUrl, bool[] isLastNodeArray, IList departmentIdList)
        {
            var itemBuilder = new StringBuilder(); 
            if (departmentInfo.IsLastNode == false)
            {
                isLastNodeArray[departmentInfo.ParentsCount] = false;
            }
            else
            {
                isLastNodeArray[departmentInfo.ParentsCount] = true;
            }
            for (var i = 0; i < departmentInfo.ParentsCount; i++)
            {
                itemBuilder.Append($"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_empty.gif\"/>");
            }
            if (departmentInfo.IsLastNode)
            {
                itemBuilder.Append(departmentInfo.ChildrenCount > 0
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/minus.png\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_empty.gif\"/>");
            }
            else
            {
                itemBuilder.Append(departmentInfo.ChildrenCount > 0
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/minus.png\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_empty.gif\"/>");
            }

            var check = "";
            if (departmentIdList.Contains(departmentInfo.Id))
            {
               check = "checked";
            } 

            itemBuilder.Append($@"
<span class=""checkbox checkbox-primary"" style=""padding-left: 0px;"">
    <input type=""checkbox"" id=""DepartmentIDCollection_{departmentInfo.Id}"" name=""DepartmentIDCollection"" value=""{departmentInfo.Id}"" {check} />
    <label for=""DepartmentIDCollection_{departmentInfo.Id}""> {departmentInfo.DepartmentName} </label>
</span>
");

            return itemBuilder.ToString();
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (Page.IsPostBack && Page.IsValid)
            {
                channelId = Utils.ToInt(Request.QueryString["channelId"]);
                var channelInfo = Main.ChannelDao.GetChannelInfo(SiteId, channelId);
                channelInfo.DepartmentIdCollection = Request.Form["DepartmentIDCollection"];
                Main.ChannelDao.Update(channelInfo);
                LayerUtils.Close(Page);
            }
        }
    }
}
