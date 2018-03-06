using System;

namespace SS.GovInteract.Model
{
    public class LogInfo
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int ChannelId { get; set; }

        public int ContentId { get; set; }

        public int DepartmentId { get; set; }

        public string UserName { get; set; }

        public string LogType { get; set; }

        public string IpAddress { get; set; }

        public DateTime AddDate { get; set; }

        public string Summary { get; set; }

        public LogInfo()
        {
            Id = 0;
            SiteId = 0;
            ChannelId = 0;
            ContentId = 0;
            DepartmentId = 0;
            UserName = string.Empty;
            LogType = string.Empty;
            IpAddress = string.Empty;
            AddDate = DateTime.Now; 
            Summary = string.Empty;
            Summary = string.Empty;
        }

        public LogInfo(int id, int siteId, int channelId, int contentId, int departmentId, string userName, string logType,string ipAddress, DateTime addDate, string summary)
        {
            Id = id;
            SiteId = siteId;
            ChannelId = channelId;
            ContentId = contentId;
            DepartmentId = departmentId;
            UserName = userName;
            LogType = logType;
            IpAddress = ipAddress;
            AddDate = addDate;
            Summary = summary;
        }
    }
}
