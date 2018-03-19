using System;

namespace SS.GovInteract.Model
{
    public class ReplyInfo
    {
        public ReplyInfo(int id, int siteId, int channelId, int contentId, string reply, string fileUrl, int departmentId, string userName, DateTime addDate)
        {
            Id = id;
            SiteId = siteId;
            ChannelId = channelId;
            ContentId = contentId;
            Reply = reply;
            FileUrl = fileUrl;
            DepartmentId = departmentId;
            UserName = userName;
            AddDate = addDate;
        }

        public int Id { get; set; }

        public int SiteId { get; set; }

        public int ChannelId { get; set; }

        public int ContentId { get; set; }

        public string Reply { get; set; }

        public string FileUrl { get; set; }

        public int DepartmentId { get; set; }

        public string UserName { get; set; }

        public DateTime AddDate { get; set; }
    }
}
