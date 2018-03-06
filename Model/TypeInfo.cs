using System;

namespace SS.GovInteract.Model
{
	public class TypeInfo
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public int ChannelId { get; set; }

        public int SiteId { get; set; }

        public int Taxis { get; set; }

        public TypeInfo()
        {
            Id = 0;
            TypeName = string.Empty;
            ChannelId = 0;
            SiteId = 0;
            Taxis = 0;
        }

        public TypeInfo(int typeID, string typeName, int channelId, int siteId, int taxis)
        {
            Id = typeID;
            TypeName = typeName;
            ChannelId = channelId;
            SiteId = siteId;
            Taxis = taxis;
        }
    }
}
