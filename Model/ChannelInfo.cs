namespace SS.GovInteract.Model
{
	public class ChannelInfo
    {
        public int Id { get; set; }

        public int ChannelId { get; set; }

        public int SiteId { get; set; }

        public int ApplyStyleId { get; set; }

        public int QueryStyleId { get; set; }

        public string DepartmentIdCollection { get; set; }

        public string Summary { get; set; }

        public ChannelInfo()
        {
            Id = 0;
            ChannelId = 0;
            SiteId = 0;
            ApplyStyleId = 0;
            QueryStyleId = 0;
            DepartmentIdCollection = string.Empty;
            Summary = string.Empty; 
        }

        public ChannelInfo(int id, int channelId, int siteId, int applyStyleId, int queryStyleId, string departmentIdCollection, string summary)
        {
            Id = id;
            ChannelId = channelId;
            SiteId = siteId;
            ApplyStyleId = applyStyleId;
            QueryStyleId = queryStyleId;
            DepartmentIdCollection = departmentIdCollection;
            Summary = summary; 
        }
    }
}
