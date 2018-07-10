using System;
using System.Text;
using SS.GovInteract.Model;
using SiteServer.Plugin;

namespace SS.GovInteract.Core
{
	public class ApplyManager
	{       
        public static string GetQueryCode()
        {
            long i = 1;
            foreach (var b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return $"{i - DateTime.Now.Ticks:x}".ToUpper();
        }

        public static string GetApplyRemark(int siteId, int contentID)
        {
            var remarkBuilder = new StringBuilder();
            var remarkInfoArrayList = Main.Instance.RemarkDao.GetRemarkInfoArrayList(siteId, contentID);
            foreach (RemarkInfo remarkInfo in remarkInfoArrayList)
            {
                if (!string.IsNullOrEmpty(remarkInfo.Remark))
                {
                    if (remarkBuilder.Length > 0) remarkBuilder.Append("<br />");
                    remarkBuilder.Append(
                        $@"<span style=""color:gray;"">{ERemarkTypeUtils.GetText(ERemarkTypeUtils.GetEnumType(remarkInfo.RemarkType))}意见: </span>{Utils
                            .MaxLengthText(remarkInfo.Remark, 25)}");
                }
            }
            return remarkBuilder.ToString();
        }

        public static void LogNew(int siteId, int channelId, int contentID, string realName, string toDepartmentName)
        {
            var logInfo = new LogInfo(0, siteId, channelId, contentID, 0, string.Empty, ELogTypeUtils.GetValue(ELogType.New), Utils.GetIpAddress(), DateTime.Now,
                $"前台{realName}提交办件{toDepartmentName}");
            Main.Instance.LogDao.Insert(logInfo);
        }

        public static void LogSwitchTo(int siteId, int channelId, int contentID, string switchToDepartmentName, string administratorName, int departmentId)
        {
            var logInfo = new LogInfo(0, siteId, channelId, contentID, departmentId, administratorName, ELogTypeUtils.GetValue(ELogType.SwitchTo), Utils.GetIpAddress(), DateTime.Now,
                $"{DepartmentManager.GetDepartmentName(departmentId)}({administratorName})转办办件至{switchToDepartmentName} ");
            Main.Instance.LogDao.Insert(logInfo);
        }

        public static void LogTranslate(int siteId, int channelId, int contentID, string nodeName, string administratorName, int departmentId)
        {
            var logInfo = new LogInfo(0, siteId, channelId, contentID, departmentId, administratorName, ELogTypeUtils.GetValue(ELogType.Translate), Utils.GetIpAddress(), DateTime.Now,
                $"{DepartmentManager.GetDepartmentName(departmentId)}({administratorName})从分类“{nodeName}”转移办件至此 ");
            Main.Instance.LogDao.Insert(logInfo);
        }

        public static void Log(int siteId, int channelId, int contentID, string logType, string administratorName, int departmentId)
        {
            var logInfo = new LogInfo(0, siteId, channelId, contentID, departmentId, administratorName, logType, Utils.GetIpAddress(), DateTime.Now, string.Empty);

            var departmentName = DepartmentManager.GetDepartmentName(departmentId);

            ELogType eLogType = ELogTypeUtils.GetEnumType(logType);

            if (eLogType == ELogType.Accept)
            {
                logInfo.Summary = $"{departmentName}({administratorName})受理办件";
            }
            else if (eLogType == ELogType.Deny)
            {
                logInfo.Summary = $"{departmentName}({administratorName})拒绝受理办件";
            }
            else if (eLogType == ELogType.Reply)
            {
                logInfo.Summary = $"{departmentName}({administratorName})回复办件";
            }
            else if (eLogType == ELogType.Comment)
            {
                logInfo.Summary = $"{departmentName}({administratorName})批示办件";
            }
            else if (eLogType == ELogType.Redo)
            {
                logInfo.Summary = $"{departmentName}({administratorName})要求返工";
            }
            else if (eLogType == ELogType.Check)
            {
                logInfo.Summary = $"{departmentName}({administratorName})审核通过";
            }
            Main.Instance.LogDao.Insert(logInfo);
        }

        public static ELimitType GetLimitType(int siteId, IContentInfo contentInfo)
        {
            var configInfo = Main.Instance.GetConfigInfo(siteId); 

            var ts = new TimeSpan(DateTime.Now.Ticks - contentInfo.AddDate.Ticks);

            var alert = configInfo.ApplyDateLimit + configInfo.ApplyAlertDate;
            var yellow = alert + configInfo.ApplyYellowAlertDate;
            var red = yellow + configInfo.ApplyRedAlertDate;

            if (ts.Days >= red)
            {
                return ELimitType.Red;
            }
            else if (ts.Days >= yellow)
            {
                return ELimitType.Yellow;
            }
            else if (ts.Days >= alert)
            {
                return ELimitType.Alert;
            }
           
            return ELimitType.Normal;
        }
	}
}
