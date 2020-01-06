namespace SS.GovInteract.Core.Model
{
    public class Settings
    {
        public bool IsClosed { get; set; }

        public int DaysWarning { get; set; }

        public int DaysDeadline { get; set; }

        public bool IsDeleteAllowed { get; set; }

        public bool IsSelectCategory { get; set; }

        public bool IsSelectDepartment { get; set; }
    }
}
