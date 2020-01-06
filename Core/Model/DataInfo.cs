using System;
using Datory;

namespace SS.GovInteract.Core.Model
{
    [Table("ss_gov_interact_data")]
    public class DataInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public DateTime? AddDate { get; set; }

        [TableColumn]
        public string QueryCode { get; set; }

        [TableColumn]
        public int CategoryId { get; set; }

        [TableColumn]
        public int DepartmentId { get; set; }

        [TableColumn]
        public bool IsCompleted { get; set; }

        [TableColumn]
        public string State { get; set; }

        [TableColumn(Text = true)]
        public string DenyReason { get; set; }

        [TableColumn(Text = true)]
        public string RedoComment { get; set; }

        [TableColumn(Text = true)]
        public string ReplyContent { get; set; }

        [TableColumn]
        public bool IsReplyFiles { get; set; }

        [TableColumn]
        public DateTime? ReplyDate { get; set; }

        [TableColumn]
        public string Name { get; set; }

        [TableColumn]
        public string Gender { get; set; }

        [TableColumn]
        public string Phone { get; set; }

        [TableColumn]
        public string Email { get; set; }

        [TableColumn]
        public string Address { get; set; }

        [TableColumn]
        public string Zip { get; set; }

        [TableColumn]
        public string Title { get; set; }

        [TableColumn(Text = true)]
        public string Content { get; set; }

        [TableColumn]
        public string CategoryName { get; set; }

        [TableColumn]
        public string DepartmentName { get; set; }
    }
}
