using System;

namespace SS.GovInteract.Model
{
    public class ReplyInfo
    {
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
