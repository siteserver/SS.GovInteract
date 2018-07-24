using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Controls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageBaseList : PageBase
    {
        public Literal LtlScript;

        public PlaceHolder PhAccept;
        public HyperLink HlAccept;
        public HyperLink HlDeny;
        public PlaceHolder PhCheck;
        public HyperLink HlCheck;
        public HyperLink HlRedo;
        public PlaceHolder PhSwitchToTranslate;
        public HyperLink HlSwitchTo;
        public HyperLink HlTranslate;
        public PlaceHolder PhComment;
        public HyperLink HlComment;
        public PlaceHolder PhDelete;
        public HyperLink HlDelete;
        public HyperLink HlExport;

        public Literal LtlTotalCount;

        public Repeater RptContents;
        public SqlPager SpContents;

        protected int ChannelId;
        private bool _isPermissionReply;
        private bool _isPermissionEdit;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ChannelId = Utils.ToInt(Request.QueryString["channelId"]);

            _isPermissionReply = InteractManager.IsPermission(SiteId, ChannelId, Permissions.Reply);
            _isPermissionEdit = InteractManager.IsPermission(SiteId, ChannelId, Permissions.Edit);

            if (!string.IsNullOrEmpty(Request.QueryString["delete"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    foreach (var contentId in list)
                    {
                        Main.Instance.ContentApi.Delete(SiteId, ChannelId, contentId);
                    }
                    LtlScript.Text = AlertUtils.Success("删除成功！", "");
                }
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["accept"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                foreach (var contentId in list)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, ChannelId, contentId);
                    var state = EStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));
                    if (state == EState.New || state == EState.Denied)
                    {
                        contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Accepted));
                        Main.Instance.ContentApi.Update(SiteId, ChannelId, contentInfo);
                    }
                }
                LtlScript.Text = AlertUtils.Success("受理申请成功！", "");
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["deny"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                foreach (var contentId in list)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, ChannelId, contentId);
                    var state = EStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));
                    if (state == EState.New || state == EState.Accepted)
                    {
                        contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Denied));
                        Main.Instance.ContentApi.Update(SiteId, ChannelId, contentInfo);
                    }
                }
                LtlScript.Text = AlertUtils.Success("拒绝受理申请成功！", "");
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["check"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                foreach (var contentId in list)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, ChannelId, contentId);
                    var state = EStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));
                    if (state == EState.Replied)
                    {
                        contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(EState.Checked));
                        Main.Instance.ContentApi.Update(SiteId, ChannelId, contentInfo);
                    }
                }
                LtlScript.Text = AlertUtils.Success("审核申请成功！", "");
            }

            SpContents.ControlToPaginate = RptContents;
            SpContents.ItemsPerPage = 25;
            SpContents.SelectCommand = GetSelectString();
            SpContents.SortField = nameof(IContentInfo.Taxis);
            SpContents.SortMode = "DESC";
            RptContents.ItemDataBound += RptContents_ItemDataBound;

            if (!IsPostBack)
            {
                SpContents.DataBind();
                LtlTotalCount.Text = SpContents.TotalCount.ToString();

                if (PhAccept != null)
                {
                    PhAccept.Visible = InteractManager.IsPermission(SiteId, ChannelId, Permissions.Accept);
                    HlAccept?.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Accept=True", "IDCollection", "IDCollection", "请选择需要受理的申请！", "此操作将受理所选申请，确定吗？"));
                    HlDeny?.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Deny=True", "IDCollection", "IDCollection", "请选择需要拒绝的申请！", "此操作将拒绝受理所选申请，确定吗？"));
                }
                if (PhCheck != null)
                {
                    PhCheck.Visible = InteractManager.IsPermission(SiteId, ChannelId, Permissions.Check);
                    HlCheck?.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Check=True", "IDCollection", "IDCollection", "请选择需要审核的申请！", "此操作将审核所选申请，确定吗？"));
                    HlRedo?.Attributes.Add("onclick", ModalApplyRedo.GetOpenWindowString(SiteId, ChannelId));
                }
                if (PhSwitchToTranslate != null)
                {
                    PhSwitchToTranslate.Visible = InteractManager.IsPermission(SiteId, ChannelId, Permissions.SwitchToTranslate);
                    HlSwitchTo?.Attributes.Add("onclick", ModalApplySwitchTo.GetOpenWindowString(SiteId, ChannelId));
                    HlTranslate?.Attributes.Add("onclick", ModalApplyTranslate.GetOpenWindowString(SiteId, ChannelId));
                }
                if (PhComment != null)
                {
                    PhComment.Visible = InteractManager.IsPermission(SiteId, ChannelId, Permissions.Comment);
                    HlComment.Attributes.Add("onclick", ModalApplyComment.GetOpenWindowString(SiteId, ChannelId));
                }
                if (PhDelete != null)
                {
                    PhDelete.Visible = InteractManager.IsPermission(SiteId, ChannelId, Permissions.Delete) && ConfigInfo.ApplyIsDeleteAllowed;
                    HlDelete.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的申请！", "此操作将删除所选申请，确定吗？"));
                }
                //hlExport?.Attributes.Add("onclick", ModalContentExport.GetOpenWindowString(SiteId, _channelId));
            }
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var contentInfo = Main.Instance.ContentApi.NewInstance(SiteId, ChannelId);
            var rowView = (DataRowView) e.Item.DataItem;
            contentInfo.Load(rowView.Row);

            var ltlTr = (Literal)e.Item.FindControl("ltlTr");
            var ltlId = (Literal)e.Item.FindControl("ltlID");
            var ltlTitle = (Literal)e.Item.FindControl("ltlTitle");
            var ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
            var ltlRemark = (Literal)e.Item.FindControl("ltlRemark");
            var ltlDepartment = (Literal)e.Item.FindControl("ltlDepartment");
            var ltlLimit = (Literal)e.Item.FindControl("ltlLimit");
            var ltlState = (Literal)e.Item.FindControl("ltlState");
            var ltlFlowUrl = (Literal)e.Item.FindControl("ltlFlowUrl");
            var ltlViewUrl = (Literal)e.Item.FindControl("ltlViewUrl");
            var ltlReplyUrl = (Literal)e.Item.FindControl("ltlReplyUrl");
            var ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");

            var limitType = ELimitType.Normal;
            ltlTr.Text = @"<tr>";
            var state = EStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));

            var textClass = string.Empty;
            if (state != EState.Denied && state != EState.Checked)
            {
                limitType = ApplyManager.GetLimitType(SiteId, contentInfo);
                if (limitType == ELimitType.Alert)
                {
                    ltlTr.Text = @"<tr class=""bg-info text-white"">";
                    textClass = "text-white";
                }
                else if (limitType == ELimitType.Yellow)
                {
                    ltlTr.Text = @"<tr class=""bg-warning text-white"">";
                    textClass = "text-white";
                }
                else if (limitType == ELimitType.Red)
                {
                    ltlTr.Text = @"<tr class=""bg-danger text-white"">";
                    textClass = "text-white";
                }
            }

            ltlId.Text = contentInfo.Id.ToString();

            var title = contentInfo.Title;
            if (string.IsNullOrEmpty(title))
            {
                title = Utils.MaxLengthText(contentInfo.GetString(ContentAttribute.Content), 30);
            }
            if (string.IsNullOrEmpty(title))
            {
                title = contentInfo.GetString(ContentAttribute.QueryCode);
            }

            var target = ConfigInfo.ApplyIsOpenWindow ? @"target=""_blank""" : string.Empty;
            if (state == EState.Accepted || state == EState.Redo)
            {
                ltlTitle.Text =
                    $@"<a class=""{textClass}"" href=""{PageContentReply.GetRedirectUrl(SiteId,
                        contentInfo.ChannelId, contentInfo.Id, PageUrl)}"" {target}>{title}</a>";
            }
            else if (state == EState.Checked || state == EState.Replied)
            {
                ltlTitle.Text =
                    $@"<a class=""{textClass}"" href=""{PageContentCheck.GetRedirectUrl(SiteId,
                        contentInfo.ChannelId, contentInfo.Id, PageUrl)}"" {target}>{title}</a>";
            }
            else if (state == EState.Denied || state == EState.New)
            {
                ltlTitle.Text =
                    $@"<a class=""{textClass}"" href=""{PageContentAccept.GetRedirectUrl(SiteId,
                        contentInfo.ChannelId, contentInfo.Id, PageUrl)}"" {target}>{title}</a>";
            }
                
            var departmentId = contentInfo.GetInt(ContentAttribute.DepartmentId);
            var departmentName = DepartmentManager.GetDepartmentName(departmentId);
            if (departmentId > 0 && departmentName != contentInfo.GetString(ContentAttribute.DepartmentName))
            {
                ltlTitle.Text += "【转办】";
            }
            else if (Utils.ToInt(contentInfo.GetString(ContentAttribute.TranslateFromChannelId)) > 0)
            {
                ltlTitle.Text += "【转移】";
            }
            ltlAddDate.Text = Utils.GetDateString(contentInfo.AddDate, EDateFormatType.Day);
            ltlRemark.Text = ApplyManager.GetApplyRemark(SiteId, contentInfo.Id);
            ltlDepartment.Text = departmentName;
            ltlLimit.Text = ELimitTypeUtils.GetText(limitType);
            ltlState.Text = EStateUtils.GetText(EStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State)));
            ltlFlowUrl.Text =
                $@"<a href=""javascript:;"" class=""{textClass}"" onclick=""{ModalApplyFlow.GetOpenWindowString(
                    SiteId, contentInfo.ChannelId, contentInfo.Id)}"">轨迹</a>";
            ltlViewUrl.Text =
                $@"<a href=""javascript:;"" class=""{textClass}"" onclick=""{ModalApplyView.GetOpenWindowString(
                    SiteId, contentInfo.ChannelId, contentInfo.Id)}"">查看</a>";
            if (_isPermissionReply)
            {
                ltlReplyUrl.Text =
                    $@"<a href=""javascript:;"" class=""{textClass}"" onclick=""{ModalApplyReply.GetOpenWindowString(
                        SiteId, contentInfo.ChannelId, contentInfo.Id)}"">办理</a>";
            }
            if (_isPermissionEdit)
            {
                ltlEditUrl.Text =
                    $@"<a class=""{textClass}"" href=""{Main.Instance.UtilsApi.GetAdminDirectoryUrl(
                        $"cms/pageContentAdd.aspx?siteId={SiteId}&channelId={contentInfo.ChannelId}&id={contentInfo.Id}&returnUrl={HttpUtility.UrlEncode(Main.Instance.PluginApi.GetPluginUrl(PageUrl))}")}"">编辑</a>";
            }
        }

        protected virtual string GetSelectString() { return string.Empty;}

        protected virtual string PageUrl => string.Empty;
    }
}
