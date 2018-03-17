using SS.GovInteract.Core;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class ModalApplyFlow : PageBase
	{
        public Literal LtlMessage;
        public Literal ltlFlows;

        private int _channelId;
        private int _contentId;

        public static string GetOpenWindowString(int siteId, int channelId, int contentId)
        {
            return LayerUtils.GetOpenScript("流动轨迹", $"{nameof(ModalApplyFlow)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}", 300, 600);
        }

		public void Page_Load(object sender, EventArgs e)
        {
            _channelId = Utils.ToInt(Request.QueryString["channelId"]);
            _contentId = Utils.ToInt(Request.QueryString["contentId"]);

            if (!IsPostBack)
			{
                if (_contentId > 0)
                {
                    var logInfoArrayList = Main.LogDao.GetLogInfoArrayList(SiteId, _contentId);
                    var builder = new StringBuilder();

                    var count = logInfoArrayList.Count;
                    var i = 1;
                    foreach (LogInfo logInfo in logInfoArrayList)
                    {
                        if (logInfo.DepartmentId > 0)
                        {
                            builder.Append(
                                $@"<tr class=""info""><td class=""center""> {DepartmentManager.GetDepartmentName(
                                    logInfo.DepartmentId)} {ELogTypeUtils.GetText(ELogTypeUtils.GetEnumType(logInfo.LogType))}<br />{Utils
                                    .GetDateAndTimeString(logInfo.AddDate)} </td></tr>");
                        }
                        else
                        {
                            builder.Append(
                                $@"<tr class=""info""><td class=""center""> {ELogTypeUtils.GetText(
                                    ELogTypeUtils.GetEnumType(logInfo.LogType))}<br />{Utils.GetDateAndTimeString(logInfo.AddDate)} </td></tr>");
                        }
                        if (i++ < count) builder.Append(@"<tr><td class=""center""><img src=""../pic/flow.gif"" /></td></tr>");
                    }
                    ltlFlows.Text = builder.ToString();
                }

				
			}
		}
	}
}
