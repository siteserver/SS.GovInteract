using System;
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

        public PlaceHolder phAccept;
        public HyperLink hlAccept;
        public HyperLink hlDeny;
        public PlaceHolder phCheck;
        public HyperLink hlCheck;
        public HyperLink hlRedo;
        public PlaceHolder phSwitchToTranslate;
        public HyperLink hlSwitchTo;
        public HyperLink hlTranslate;
        public PlaceHolder phComment;
        public HyperLink hlComment;
        public PlaceHolder phDelete;
        public HyperLink hlDelete;
        public HyperLink hlExport;

        public Literal ltlTotalCount;

        public Repeater rptContents;
        public SqlPager spContents;

        protected int _channelId;
        private bool isPermissionReply = false;
        private bool isPermissionEdit = false;
        private ConfigInfo _configInfo;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _configInfo = Main.Instance.GetConfigInfo(SiteId);

            _channelId = Utils.ToInt(Request.QueryString["channelId"]);

            isPermissionReply = InteractManager.IsPermission(SiteId, _channelId, Permissions.GovInteractReply);
            isPermissionEdit = InteractManager.IsPermission(SiteId, _channelId, Permissions.GovInteractEdit);

            if (!string.IsNullOrEmpty(Request.QueryString["delete"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    foreach (var contentId in list)
                    {
                        Main.Instance.ContentApi.Delete(SiteId, _channelId, contentId);
                    }
                    LtlScript.Text = AlertUtils.Success("删除成功！", "");
                }
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["accept"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                foreach (var contentId in list)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, contentId);
                    var state = EGovInteractStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));
                    if (state == EGovInteractState.New || state == EGovInteractState.Denied)
                    {
                        contentInfo.Set(ContentAttribute.State, EGovInteractStateUtils.GetValue(EGovInteractState.Accepted));
                        Main.Instance.ContentApi.Update(SiteId, _channelId, contentInfo);
                    }
                }
                LtlScript.Text = AlertUtils.Success("受理申请成功！", "");
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["deny"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                foreach (var contentId in list)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, contentId);
                    var state = EGovInteractStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));
                    if (state == EGovInteractState.New || state == EGovInteractState.Accepted)
                    {
                        contentInfo.Set(ContentAttribute.State, EGovInteractStateUtils.GetValue(EGovInteractState.Denied));
                        Main.Instance.ContentApi.Update(SiteId, _channelId, contentInfo);
                    }
                }
                LtlScript.Text = AlertUtils.Success("拒绝受理申请成功！", "");
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["check"]))
            {
                var list = Utils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                foreach (var contentId in list)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, _channelId, contentId);
                    var state = EGovInteractStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));
                    if (state == EGovInteractState.Replied)
                    {
                        contentInfo.Set(ContentAttribute.State, EGovInteractStateUtils.GetValue(EGovInteractState.Checked));
                        Main.Instance.ContentApi.Update(SiteId, _channelId, contentInfo);
                    }
                }
                LtlScript.Text = AlertUtils.Success("审核申请成功！", "");
            }

            spContents.ControlToPaginate = rptContents;
            spContents.ItemsPerPage = 25;
            spContents.SelectCommand = GetSelectString();
            spContents.SortField = nameof(IContentInfo.Taxis);
            spContents.SortMode = "DESC";
            rptContents.ItemDataBound += rptContents_ItemDataBound;

            if (!IsPostBack)
            {
                spContents.DataBind();
                ltlTotalCount.Text = spContents.TotalCount.ToString();

                if (phAccept != null)
                {
                    phAccept.Visible = InteractManager.IsPermission(SiteId, _channelId, Permissions.GovInteractAccept);
                    hlAccept?.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Accept=True", "IDCollection", "IDCollection", "请选择需要受理的申请！", "此操作将受理所选申请，确定吗？"));
                    hlDeny?.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Deny=True", "IDCollection", "IDCollection", "请选择需要拒绝的申请！", "此操作将拒绝受理所选申请，确定吗？"));
                }
                if (phCheck != null)
                {
                    phCheck.Visible = InteractManager.IsPermission(SiteId, _channelId, Permissions.GovInteractCheck);
                    hlCheck?.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Check=True", "IDCollection", "IDCollection", "请选择需要审核的申请！", "此操作将审核所选申请，确定吗？"));
                    hlRedo?.Attributes.Add("onclick", ModalApplyRedo.GetOpenWindowString(SiteId, _channelId));
                }
                if (phSwitchToTranslate != null)
                {
                    phSwitchToTranslate.Visible = InteractManager.IsPermission(SiteId, _channelId, Permissions.GovInteractSwitchToTranslate);
                    hlSwitchTo?.Attributes.Add("onclick", ModalApplySwitchTo.GetOpenWindowString(SiteId, _channelId));
                    hlTranslate?.Attributes.Add("onclick", ModalApplyTranslate.GetOpenWindowString(SiteId, _channelId));
                }
                if (phComment != null)
                {
                    phComment.Visible = InteractManager.IsPermission(SiteId, _channelId, Permissions.GovInteractComment);
                    hlComment.Attributes.Add("onclick", ModalApplyComment.GetOpenWindowString(SiteId, _channelId));
                }
                if (phDelete != null)
                {
                    phDelete.Visible = InteractManager.IsPermission(SiteId, _channelId, Permissions.GovInteractDelete) && _configInfo.ApplyIsDeleteAllowed;
                    hlDelete.Attributes.Add("onclick", Utils.GetRedirectStringWithCheckBoxValueAndAlert(PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的申请！", "此操作将删除所选申请，确定吗？"));
                }
                //hlExport?.Attributes.Add("onclick", ModalContentExport.GetOpenWindowString(SiteId, _channelId));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var contentInfo = Main.Instance.ContentApi.NewInstance();
                contentInfo.Load(e.Item.DataItem);

                var ltlTr = e.Item.FindControl("ltlTr") as Literal;
                var ltlID = e.Item.FindControl("ltlID") as Literal;
                var ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                var ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                var ltlRemark = e.Item.FindControl("ltlRemark") as Literal;
                var ltlDepartment = e.Item.FindControl("ltlDepartment") as Literal;
                var ltlLimit = e.Item.FindControl("ltlLimit") as Literal;
                var ltlState = e.Item.FindControl("ltlState") as Literal;
                var ltlFlowUrl = e.Item.FindControl("ltlFlowUrl") as Literal;
                var ltlViewUrl = e.Item.FindControl("ltlViewUrl") as Literal;
                var ltlReplyUrl = e.Item.FindControl("ltlReplyUrl") as Literal;
                var ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                var limitType = ELimitType.Normal;
                ltlTr.Text = @"<tr>";
                var state = EGovInteractStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State));
                if (state == EGovInteractState.Denied || state == EGovInteractState.Checked)
                {
                    ltlTr.Text = @"<tr class=""success"">";
                }
                else
                {
                    limitType = ApplyManager.GetLimitType(SiteId, contentInfo);
                    if (limitType == ELimitType.Alert)
                    {
                        ltlTr.Text = @"<tr class=""info"">";
                    }
                    else if (limitType == ELimitType.Yellow)
                    {
                        ltlTr.Text = @"<tr class=""warning"">";
                    }
                    else if (limitType == ELimitType.Red)
                    {
                        ltlTr.Text = @"<tr class=""error"">";
                    }
                }

                ltlID.Text = contentInfo.Id.ToString();

                var title = contentInfo.Title;
                if (string.IsNullOrEmpty(title))
                {
                    title = Utils.MaxLengthText(contentInfo.GetString(ContentAttribute.Content), 30);
                }
                if (string.IsNullOrEmpty(title))
                {
                    title = contentInfo.GetString(ContentAttribute.QueryCode);
                }

                var target = _configInfo.ApplyIsOpenWindow ? @"target=""_blank""" : string.Empty;
                if (state == EGovInteractState.Accepted || state == EGovInteractState.Redo)
                {
                    ltlTitle.Text =
                        $@"<a href=""{PageReply.GetRedirectUrl(SiteId,
                            contentInfo.ChannelId, contentInfo.Id, PageUrl)}"" {target}>{title}</a>";
                }
                else if (state == EGovInteractState.Checked || state == EGovInteractState.Replied)
                {
                    ltlTitle.Text =
                        $@"<a href=""{PageCheck.GetRedirectUrl(SiteId,
                            contentInfo.ChannelId, contentInfo.Id, PageUrl)}"" {target}>{title}</a>";
                }
                else if (state == EGovInteractState.Denied || state == EGovInteractState.New)
                {
                    ltlTitle.Text =
                        $@"<a href=""{PageAccept.GetRedirectUrl(SiteId,
                            contentInfo.ChannelId, contentInfo.Id, PageUrl)}"" {target}>{title}</a>";
                }
                
                var departmentId = contentInfo.GetInt(ContentAttribute.DepartmentId);
                var departmentName = DepartmentManager.GetDepartmentName(departmentId);
                if (departmentId > 0 && departmentName != contentInfo.GetString(ContentAttribute.DepartmentName))
                {
                    ltlTitle.Text += "<span style='color:red'>【转办】</span>";
                }
                else if (Utils.ToInt(contentInfo.GetString(ContentAttribute.TranslateFromChannelId)) > 0)
                {
                    ltlTitle.Text += "<span style='color:red'>【转移】</span>";
                }
                ltlAddDate.Text = Utils.GetDateAndTimeString(contentInfo.AddDate);
                ltlRemark.Text = ApplyManager.GetApplyRemark(SiteId, contentInfo.Id);
                ltlDepartment.Text = departmentName;
                ltlLimit.Text = ELimitTypeUtils.GetText(limitType);
                ltlState.Text = EGovInteractStateUtils.GetText(EGovInteractStateUtils.GetEnumType(contentInfo.GetString(ContentAttribute.State)));
                if (state == EGovInteractState.New)
                {
                    ltlState.Text = $"<span style='color:red'>{ltlState.Text}</span>";
                }
                else if (state == EGovInteractState.Redo)
                {
                    ltlState.Text = $"<span style='color:red'>{ltlState.Text}</span>";
                }
                ltlFlowUrl.Text =
                    $@"<a href=""javascript:;"" onclick=""{ModalApplyFlow.GetOpenWindowString(
                        SiteId, contentInfo.ChannelId, contentInfo.Id)}"">轨迹</a>";
                ltlViewUrl.Text =
                    $@"<a href=""javascript:;"" onclick=""{ModalApplyView.GetOpenWindowString(
                        SiteId, contentInfo.ChannelId, contentInfo.Id)}"">查看</a>";
                if (isPermissionReply)
                {
                    ltlReplyUrl.Text =
                        $@"<a href=""javascript:;"" onclick=""{ModalApplyReply.GetOpenWindowString(
                            SiteId, contentInfo.ChannelId, contentInfo.Id)}"">办理</a>";
                }
                if (isPermissionEdit)
                {
                    var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, contentInfo.ChannelId);
                    //ltlEditUrl.Text =
                    //    $@"<a href=""{WebUtils.GetContentAddEditUrl(SiteId, nodeInfo, contentInfo.Id,
                    //        PageUrl)}"">编辑</a>";
                }
            }
        }

        protected virtual string GetSelectString() { return string.Empty;}

        protected virtual string PageUrl => string.Empty;
    }
}
