using System;

namespace SS.GovInteract.Model
{
    public class RemarkInfo
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int ChannelId { get; set; }

        public int ContentId { get; set; }

        public string RemarkType { get; set; }

        public string Remark { get; set; }

        public int DepartmentId { get; set; }

        public string UserName { get; set; }

        public DateTime AddDate { get; set; }

        public RemarkInfo()
        {
            Id = 0;
            SiteId = 0;
            ChannelId = 0;
            ContentId = 0;
            RemarkType = string.Empty;
            Remark = string.Empty;
            DepartmentId = 0;
            UserName = string.Empty;
            AddDate = DateTime.Now;
        }

        public RemarkInfo(int remarkID, int siteId, int channelId, int contentID, string remarkType, string remark, int departmentID, string userName, DateTime addDate)
        {
            Id = remarkID;
            SiteId = siteId;
            ChannelId = channelId;
            ContentId = contentID;
            RemarkType = remarkType;
            Remark = remark;
            DepartmentId = departmentID;
            UserName = userName;
            AddDate = addDate;
        }
    }
}
