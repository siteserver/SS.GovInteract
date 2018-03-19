namespace SS.GovInteract.Core
{
    public static class Permissions
    {
        public const string View = "wcm_govInteractView";
        public const string Add = "wcm_govInteractAdd";
        public const string Edit = "wcm_govInteractEdit";
        public const string Delete = "wcm_govInteractDelete";
        public const string SwitchToTranslate = "wcm_govInteractSwitchToTranslate";
        public const string Comment = "wcm_govInteractComment";
        public const string Accept = "wcm_govInteractAccept";
        public const string Reply = "wcm_govInteractReply";
        public const string Check = "wcm_govInteractCheck";

        public static string GetPermissionName(string permission)
        {
            var retval = string.Empty;
            if (permission == View)
            {
                retval = "浏览办件";
            }
            else if (permission == Add)
            {
                retval = "新增办件";
            }
            else if (permission == Edit)
            {
                retval = "编辑办件";
            }
            else if (permission == Delete)
            {
                retval = "删除办件";
            }
            else if (permission == SwitchToTranslate)
            {
                retval = "转办转移";
            }
            else if (permission == Comment)
            {
                retval = "批示办件";
            }
            else if (permission == Accept)
            {
                retval = "受理办件";
            }
            else if (permission == Reply)
            {
                retval = "办理办件";
            }
            else if (permission == Check)
            {
                retval = "审核办件";
            }
            return retval;
        }
    }
}
