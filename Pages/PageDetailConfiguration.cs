using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageDetailConfiguration : PageBase
    {
        public Literal LtlMessage;
        public Repeater RptContents; // 分类列表 
         
        private ConfigInfo _configInfo;

        public static string GetRedirectUrl(int siteId)
        {
            return Main.Instance.PluginApi.GetPluginUrl($"{nameof(PageDetailConfiguration)}.aspx?siteId={siteId}");
        }

        public void Page_Load(object sender, EventArgs e)
        { 
            _configInfo = Main.Instance.GetConfigInfo(SiteId);

            if (!IsPostBack)
            {
                var channelIdList = Main.Instance.ChannelApi.GetChannelIdList(SiteId);
                var nodeInfoList = new ArrayList();
                foreach (var channelId in channelIdList)
                { 
                    var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, channelId);
                    if (nodeInfo != null & nodeInfo.ContentModelPluginId == Main.Instance.Id)
                    {
                        nodeInfoList.Add(nodeInfo);
                    }
                }

                RptContents.DataSource = nodeInfoList; 
                RptContents.ItemDataBound += RptContents_ItemDataBound;
                RptContents.DataBind();
            }
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            { 
                var nodeInfo = (IChannelInfo)e.Item.DataItem;
                var ltlName = (Literal)e.Item.FindControl("ltlName");
                var ltlAction = (Literal)e.Item.FindControl("ltlAction");
                ltlName.Text = nodeInfo.ChannelName;
                ltlAction.Text = $@"<a href='javascript:;' onclick=""{ModalDepartmentSelect.GetOpenWindowString(SiteId, nodeInfo.Id)}"">负责部门设置</a> &nbsp;&nbsp;&nbsp;&nbsp;
                                    <a href='javascript:;' onclick=""{ModalAdinistrators.GetOpenWindowString(SiteId, nodeInfo.Id)}"">负责人员设置</a> &nbsp;&nbsp;&nbsp;&nbsp;
                                    <a href='#'>邮件/短信发送</a> &nbsp;&nbsp;&nbsp;&nbsp;
                                    <a href='javascript:;' onclick=""{ModalTypeList.GetOpenWindowStringToList(SiteId, nodeInfo.Id)}"">办件类型管理</a> 
                                    ";  
            }  
        }
    }
}