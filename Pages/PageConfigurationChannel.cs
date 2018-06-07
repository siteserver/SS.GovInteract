using System;
using System.Collections;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageConfigurationChannel : PageBase
    {
        public Literal LtlMessage;
        public Repeater RptContents; // 分类列表 
         
        private ConfigInfo _configInfo;

        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageConfigurationChannel)}.aspx?siteId={siteId}";
        }

        public void Page_Load(object sender, EventArgs e)
        { 
            _configInfo = Main.Instance.GetConfigInfo(SiteId);

            if (!IsPostBack)
            {
                var channelIdList = Main.Instance.ChannelApi.GetChannelIdList(SiteId);
                var channelInfoList = new ArrayList();
                foreach (var channelId in channelIdList)
                { 
                    var channelInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, channelId);
                    if (channelInfo != null & channelInfo.ContentModelPluginId == Main.Instance.Id)
                    {
                        channelInfoList.Add(channelInfo);
                    }
                }

                RptContents.DataSource = channelInfoList; 
                RptContents.ItemDataBound += RptContents_ItemDataBound;
                RptContents.DataBind();
            }
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            { 
                var channelInfo = (IChannelInfo)e.Item.DataItem;
                var ltlName = (Literal)e.Item.FindControl("ltlName");
                var ltlAction = (Literal)e.Item.FindControl("ltlAction");
                ltlName.Text = channelInfo.ChannelName;
                ltlAction.Text = $@"
<a href='javascript:;' onclick=""{ModalConfigDepartments.GetOpenWindowString(SiteId, channelInfo.Id)}"">
负责部门设置</a>
&nbsp;&nbsp;&nbsp;&nbsp;
<a href='javascript:;' onclick=""{ModalConfigAdministrators.GetOpenWindowString(SiteId, channelInfo.Id)}"">
负责人员设置</a>
&nbsp;&nbsp;&nbsp;&nbsp;
<a href='javascript:;' onclick=""{ModalConfigTypes.GetOpenWindowStringToList(SiteId, channelInfo.Id)}"">
办件类型管理</a> 
";
            }  
        }
    }
}