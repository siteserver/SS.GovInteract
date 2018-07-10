using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.GovInteract.Controls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class PageAnalysis : PageBase
	{
        public DateTimeTextBox TbStartDate;
        public DateTimeTextBox TbEndDate;
        public DropDownList DdlChannelId;
        public Repeater RptContents;

        private int _nodeId;

        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageAnalysis)}.aspx?siteId={siteId}";
        }

		public void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            TbStartDate.Text = string.Empty;
            TbEndDate.Now = true;

            var nodeInfoList = InteractManager.GetInteractChannelInfoList(SiteId);

            var listItem = new ListItem("<<全部>>", "0");
            DdlChannelId.Items.Add(listItem);
            foreach (var nodeInfo in nodeInfoList)
            {
                listItem = new ListItem(nodeInfo.ChannelName, nodeInfo.Id.ToString());
                DdlChannelId.Items.Add(listItem);
            }

            BindGrid();
        }

	    private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var departmentId = (int)e.Item.DataItem;

            var departmentInfo = DepartmentManager.GetDepartmentInfo(departmentId);

            var ltlTrHtml = (Literal)e.Item.FindControl("ltlTrHtml");
            var ltlTarget = (Literal)e.Item.FindControl("ltlTarget");
            var ltlTotalCount = (Literal)e.Item.FindControl("ltlTotalCount");
            var ltlDoCount = (Literal)e.Item.FindControl("ltlDoCount");
            var ltlUndoCount = (Literal)e.Item.FindControl("ltlUndoCount");
            var ltlBar = (Literal)e.Item.FindControl("ltlBar");

            ltlTrHtml.Text =
                $@"<tr>";
            ltlTarget.Text = departmentInfo.DepartmentName;

            int totalCount;
            int doCount;
            if (_nodeId == 0)
            {
                totalCount = Main.Instance.ContentDao.GetCountByDepartmentId(SiteId, departmentId, TbStartDate.DateTime, TbEndDate.DateTime);
                doCount = Main.Instance.ContentDao.GetCountByDepartmentIdAndState(SiteId, departmentId, EState.Checked, TbStartDate.DateTime, TbEndDate.DateTime);
            }
            else
            {
                totalCount = Main.Instance.ContentDao.GetCountByDepartmentId(SiteId, departmentId, _nodeId, TbStartDate.DateTime, TbEndDate.DateTime);
                doCount = Main.Instance.ContentDao.GetCountByDepartmentIdAndState(SiteId, departmentId, _nodeId, EState.Checked, TbStartDate.DateTime, TbEndDate.DateTime);
            }
            var unDoCount = totalCount - doCount;

            ltlTotalCount.Text = totalCount.ToString();
            ltlDoCount.Text = doCount.ToString();
            ltlUndoCount.Text = unDoCount.ToString();

            ltlBar.Text = $@"<div class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {GetBarWidth(doCount, totalCount)}%""></div>
          </div>";
        }

        private double GetBarWidth(int doCount, int totalCount)
        {
            double width = 0;
            if (totalCount > 0)
            {
                width = Convert.ToDouble(doCount) / Convert.ToDouble(totalCount);
                width = Math.Round(width, 2) * 100;
            }
            return width;
        }

        public void Analysis_OnClick(object sender, EventArgs e)
        {
            BindGrid();
        }

        public void BindGrid()
        {
            _nodeId = Utils.ToInt(DdlChannelId.SelectedValue);

            var departmentIdList = new List<int>();

            if (_nodeId > 0)
            {
                var channelInfo = Main.Instance.ChannelDao.GetChannelInfo(SiteId, _nodeId);

                departmentIdList = Main.Instance.DepartmentDao.GetDepartmentIdListByFirstDepartmentIdList(InteractManager.GetDepartmentIdList(channelInfo));
            }

            if (departmentIdList.Count == 0)
            {
                departmentIdList = DepartmentManager.GetDepartmentIdList();
            }

            RptContents.DataSource = departmentIdList;
            RptContents.ItemDataBound += RptContents_ItemDataBound;
            RptContents.DataBind();
        }
    }
}
