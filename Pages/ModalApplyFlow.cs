using SS.GovInteract.Core;
using System;
using System.Text;
using System.Web.UI.WebControls;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class ModalApplyFlow : PageBase
	{
        public Literal LtlFlows;

        private int _contentId;

        public static string GetOpenWindowString(int siteId, int channelId, int contentId)
        {
            return LayerUtils.GetOpenScript("流动轨迹", $"{nameof(ModalApplyFlow)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}", 300, 600);
        }

		public void Page_Load(object sender, EventArgs e)
        {
            _contentId = Utils.ToInt(Request.QueryString["contentId"]);

            if (IsPostBack) return;

            if (_contentId > 0)
            {
                var logInfoList = Main.LogDao.GetLogInfoList(SiteId, _contentId);
                var builder = new StringBuilder();

                var count = logInfoList.Count;
                var i = 1;
                foreach (var logInfo in logInfoList)
                {
                    if (logInfo.DepartmentId > 0)
                    {
                        builder.Append(
                            $@"<tr class=""info""><td class=""text-center""> {DepartmentManager.GetDepartmentName(
                                logInfo.DepartmentId)} {ELogTypeUtils.GetText(ELogTypeUtils.GetEnumType(logInfo.LogType))}<br />{Utils
                                .GetDateAndTimeString(logInfo.AddDate)} </td></tr>");
                    }
                    else
                    {
                        builder.Append(
                            $@"<tr class=""info""><td class=""text-center""> {ELogTypeUtils.GetText(
                                ELogTypeUtils.GetEnumType(logInfo.LogType))}<br />{Utils.GetDateAndTimeString(logInfo.AddDate)} </td></tr>");
                    }
                    if (i++ < count) builder.Append(@"<tr><td class=""text-center""><img src=""assets/images/flow.gif"" /></td></tr>");
                }
                LtlFlows.Text = builder.ToString();
            }
        }
	}
}
