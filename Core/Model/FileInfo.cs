using Datory;

namespace SS.GovInteract.Core.Model
{
    [Table("ss_gov_interact_file")]
    public class FileInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public int DataId { get; set; }

        [TableColumn]
        public string FileName { get; set; }

        [TableColumn]
        public string FileUrl { get; set; }

        [TableColumn]
        public int Length { get; set; }
    }
}
