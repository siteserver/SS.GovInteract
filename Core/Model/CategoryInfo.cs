using Datory;

namespace SS.GovInteract.Core.Model
{
    [Table("ss_gov_interact_category")]
    public class CategoryInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public string CategoryName { get; set; }

        [TableColumn(Text = true)]
        public string UserNames { get; set; }

        [TableColumn]
        public int Taxis { get; set; }
    }
}
