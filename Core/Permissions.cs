namespace SS.GovInteract.Core
{
    public static class Permissions
    {
        public const string GovInteractView = "wcm_govInteractView";
        public const string GovInteractAdd = "wcm_govInteractAdd";
        public const string GovInteractEdit = "wcm_govInteractEdit";
        public const string GovInteractDelete = "wcm_govInteractDelete";
        public const string GovInteractSwitchToTranslate = "wcm_govInteractSwitchToTranslate";
        public const string GovInteractComment = "wcm_govInteractComment";
        public const string GovInteractAccept = "wcm_govInteractAccept";
        public const string GovInteractReply = "wcm_govInteractReply";
        public const string GovInteractCheck = "wcm_govInteractCheck";

        public static string GetPermissionName(string permission)
        {
            var retval = string.Empty;
            if (permission == GovInteractView)
            {
                retval = "浏览办件";
            }
            else if (permission == GovInteractAdd)
            {
                retval = "新增办件";
            }
            else if (permission == GovInteractEdit)
            {
                retval = "编辑办件";
            }
            else if (permission == GovInteractDelete)
            {
                retval = "删除办件";
            }
            else if (permission == GovInteractSwitchToTranslate)
            {
                retval = "转办转移";
            }
            else if (permission == GovInteractComment)
            {
                retval = "批示办件";
            }
            else if (permission == GovInteractAccept)
            {
                retval = "受理办件";
            }
            else if (permission == GovInteractReply)
            {
                retval = "办理办件";
            }
            else if (permission == GovInteractCheck)
            {
                retval = "审核办件";
            }
            return retval;
        }
    }
}
