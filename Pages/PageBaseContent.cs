using System;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class PageBaseContent : PageBase
    {
        public Literal LtlTitle;
        public Literal LtlApplyAttributes;
        public Literal LtlContent;
        public Literal LtlQueryCode;
        public Literal LtlState;
        public Literal LtlDepartmentName;

        public PlaceHolder PhReply;
        public Literal LtlDepartmentAndUserName;
        public Literal LtlReplyAddDate;
        public Literal LtlReply;
        public Literal LtlReplyFileUrl;

        public PlaceHolder PhBtnAccept;
        public PlaceHolder PhBtnSwitchToTranslate;
        public PlaceHolder PhBtnReply;
        public PlaceHolder PhBtnCheck;
        public PlaceHolder PhBtnComment;
        public PlaceHolder PhBtnReturn;

        public TextBox TbReply;
        public HtmlInputFile HifFileUrl;
        public TextBox TbSwitchToRemark;
        public HtmlControl DivAddDepartment;
        public Literal LtlScript;
        public DropDownList DdlTranslateChannelId;
        public TextBox TbTranslateRemark;
        public TextBox TbCommentRemark;

        public PlaceHolder PhRemarks;
        public Repeater RptRemarks;
        public Repeater RptLogs;

        public string MyDepartment => DepartmentManager.GetDepartmentName(AuthRequest.AdminInfo.DepartmentId);
        public string MyDisplayName => AuthRequest.AdminInfo.DisplayName;

        private IContentInfo _contentInfo;
        private string _returnUrl;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var channelId = AuthRequest.GetQueryInt("channelId");
            var contentId = AuthRequest.GetQueryInt("contentId");
            _returnUrl = AuthRequest.GetQueryString("returnUrl");

            _contentInfo = Main.Instance.ContentApi.GetContentInfo(SiteId, channelId, contentId);
            var state = EStateUtils.GetEnumType(_contentInfo.GetString(ContentAttribute.State));

            if (IsPostBack) return;

            if (PhBtnAccept != null)
            {
                PhBtnAccept.Visible = InteractManager.IsPermission(SiteId, _contentInfo.ChannelId, Permissions.Accept);
            }
            if (PhBtnSwitchToTranslate != null)
            {
                PhBtnSwitchToTranslate.Visible = InteractManager.IsPermission(SiteId, _contentInfo.ChannelId, Permissions.SwitchToTranslate);
            }
            if (PhBtnReply != null)
            {
                PhBtnReply.Visible = InteractManager.IsPermission(SiteId, _contentInfo.ChannelId, Permissions.Reply);
            }
            if (PhBtnCheck != null)
            {
                PhBtnCheck.Visible = state != EState.Checked && InteractManager.IsPermission(SiteId, _contentInfo.ChannelId, Permissions.Check);
            }
            if (PhBtnComment != null)
            {
                PhBtnComment.Visible = state != EState.Checked && InteractManager.IsPermission(SiteId, _contentInfo.ChannelId, Permissions.Comment);
            }
            if (PhBtnReturn != null)
            {
                PhBtnReturn.Visible = !ConfigInfo.ApplyIsOpenWindow;
            }
            
            var tableColumns = Main.Instance.ContentApi.GetTableColumns(SiteId, _contentInfo.ChannelId);
            var isSingle = true;

            var builder = new StringBuilder();
            foreach (var tableColumn in tableColumns)
            {
                if (tableColumn.InputStyle == null ||
                    Utils.EqualsIgnoreCase(tableColumn.AttributeName, ContentAttribute.DepartmentId) ||
                    Utils.EqualsIgnoreCase(tableColumn.AttributeName, ContentAttribute.Content)) continue;

                var value = _contentInfo.GetString(tableColumn.AttributeName);
                if (Utils.EqualsIgnoreCase(tableColumn.AttributeName, ContentAttribute.TypeId))
                {
                    value = InteractManager.GetTypeName(Utils.ToInt(value));
                }
                else if (Utils.EqualsIgnoreCase(tableColumn.AttributeName, ContentAttribute.IsPublic))
                {
                    value = Utils.ToBool(value) ? "公开" : "不公开";
                }
                else if (Utils.EqualsIgnoreCase(tableColumn.AttributeName, ContentAttribute.FileUrl))
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        value =
                            $@"<a href=""{value}"" target=""_blank"">{value}</a>";
                    }
                }

                if (isSingle)
                {
                    builder.Append("<tr>");
                }

                builder.Append(
                    $@"<th>{tableColumn.InputStyle.DisplayName}</th><td>{value}</td>");

                if (!isSingle)
                {
                    builder.Append("</tr>");
                }

                isSingle = !isSingle;
            }
            if (!isSingle)
            {
                builder.Append("</tr>");
            }

            LtlTitle.Text = _contentInfo.Title;
            LtlApplyAttributes.Text = builder.ToString();

            LtlContent.Text = _contentInfo.GetString(ContentAttribute.Content);
            LtlQueryCode.Text = _contentInfo.GetString(ContentAttribute.QueryCode);
            LtlState.Text = EStateUtils.GetText(state);
            LtlDepartmentName.Text = _contentInfo.GetString(ContentAttribute.DepartmentName);

            if (PhReply != null)
            {
                if (state == EState.Denied || state == EState.Replied || state == EState.Redo || state == EState.Checked)
                {
                    var replyInfo = Main.ReplyDao.GetReplyInfoByContentId(SiteId, _contentInfo.Id);
                    if (replyInfo != null)
                    {
                        PhReply.Visible = true;
                        LtlDepartmentAndUserName.Text =
                            $"{DepartmentManager.GetDepartmentName(replyInfo.DepartmentId)}({replyInfo.UserName})";
                        LtlReplyAddDate.Text = Utils.GetDateAndTimeString(replyInfo.AddDate);
                        LtlReply.Text = replyInfo.Reply;
                        if (!string.IsNullOrEmpty(replyInfo.FileUrl))
                        {
                            LtlReplyFileUrl.Text =
                                $@"<a href=""{replyInfo.FileUrl}"" target=""_blank"">{replyInfo.FileUrl}</a>";
                        }
                    }
                }
            }

            if (DivAddDepartment != null)
            {
                var departmentId = _contentInfo.GetInt(ContentAttribute.DepartmentId);
                DivAddDepartment.Attributes.Add("onclick", ModalDepartmentSelect.GetOpenWindowString(SiteId, _contentInfo.ChannelId));
                var scriptBuilder = new StringBuilder();
                if (departmentId > 0)
                {
                    var departmentName = DepartmentManager.GetDepartmentName(departmentId);
                    scriptBuilder.Append(
                        $@"<script>showCategoryDepartment('{departmentName}', '{departmentId}');</script>");
                }
                LtlScript.Text = scriptBuilder.ToString();
            }

            if (DdlTranslateChannelId != null)
            {
                var nodeInfoList = InteractManager.GetInteractChannelInfoList(SiteId);
                foreach (var nodeInfo in nodeInfoList)
                {
                    if (nodeInfo.Id != _contentInfo.ChannelId)
                    {
                        var listItem = new ListItem(nodeInfo.ChannelName, nodeInfo.Id.ToString());
                        DdlTranslateChannelId.Items.Add(listItem);
                    }
                }
            }

            RptRemarks.DataSource = Main.RemarkDao.GetDataSourceByContentId(SiteId, _contentInfo.Id);
            RptRemarks.ItemDataBound += RptRemarks_ItemDataBound;
            RptRemarks.DataBind();

            if (RptLogs != null)
            {
                RptLogs.DataSource = Main.LogDao.GetDataSourceByContentId(SiteId, _contentInfo.Id);
                RptLogs.ItemDataBound += RptLogs_ItemDataBound;
                RptLogs.DataBind();
            }
        }

        public void Reply_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TbReply.Text))
            {
                LtlMessage.Text = Utils.GetMessageHtml("回复失败，必须填写答复内容", false);
                return;
            }
            try
            {
                Main.ReplyDao.DeleteByContentId(SiteId, _contentInfo.Id);
                var fileUrl = UploadFile(HifFileUrl.PostedFile);
                var replyInfo = new ReplyInfo(0, SiteId, _contentInfo.ChannelId, _contentInfo.Id, TbReply.Text, fileUrl, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                Main.ReplyDao.Insert(replyInfo);

                ApplyManager.Log(SiteId, _contentInfo.ChannelId, _contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Reply), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);
                if (AuthRequest.AdminInfo.DepartmentId > 0)
                {
                    Main.ContentDao.UpdateDepartmentId(SiteId, _contentInfo.ChannelId, _contentInfo.Id, AuthRequest.AdminInfo.DepartmentId);
                }
                Main.ContentDao.UpdateState(SiteId, _contentInfo.ChannelId, _contentInfo.Id, EState.Replied);

                LtlMessage.Text = Utils.GetMessageHtml("办件回复成功", true);

                if (!ConfigInfo.ApplyIsOpenWindow)
                {
                    Utils.SwalSuccess("办件回复成功", "", "确 认", $"location.href = '{ListPageUrl}'");
                }
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
            }
        }

        private string UploadFile(HttpPostedFile myFile)
        {
            var fileUrl = string.Empty;

            //if (myFile != null && !string.IsNullOrEmpty(myFile.FileName))
            //{
            //    var filePath = myFile.FileName;
            //    try
            //    {
            //        var fileExtName = PathUtils.GetExtension(filePath);
            //        var localDirectoryPath = PathUtility.GetUploadDirectoryPath(PublishmentSystemInfo, fileExtName);
            //        var localFileName = PathUtility.GetUploadFileName(PublishmentSystemInfo, filePath);

            //        var localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

            //        if (!PathUtility.IsFileExtenstionAllowed(PublishmentSystemInfo, fileExtName))
            //        {
            //            return string.Empty;
            //        }
            //        if (!PathUtility.IsFileSizeAllowed(PublishmentSystemInfo, myFile.ContentLength))
            //        {
            //            return string.Empty;
            //        }

            //        myFile.SaveAs(localFilePath);
            //        FileUtility.AddWaterMark(PublishmentSystemInfo, localFilePath);

            //        fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(PublishmentSystemInfo, localFilePath);
            //        fileUrl = PageUtility.GetVirtualUrl(PublishmentSystemInfo, fileUrl);
            //    }
            //    catch { }
            //}

            return fileUrl;
        }

        public void SwitchTo_OnClick(object sender, EventArgs e)
        {
            var switchToDepartmentId = Utils.ToInt(Request.Form["switchToDepartmentID"]);
            if (switchToDepartmentId == 0)
            {
                LtlMessage.Text = Utils.GetMessageHtml("转办失败，必须选择转办部门", false);
                return;
            }
            var switchToDepartmentName = DepartmentManager.GetDepartmentName(switchToDepartmentId);
            try
            {
                Main.ContentDao.UpdateDepartmentId(SiteId, _contentInfo.ChannelId, _contentInfo.Id, switchToDepartmentId);

                var remarkInfo = new RemarkInfo(0, SiteId, _contentInfo.ChannelId, _contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.SwitchTo), TbSwitchToRemark.Text, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                Main.RemarkDao.Insert(remarkInfo);

                ApplyManager.LogSwitchTo(SiteId, _contentInfo.ChannelId, _contentInfo.Id, switchToDepartmentName, AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);

                LtlMessage.Text = Utils.GetMessageHtml("办件转办成功", true);

                if (!ConfigInfo.ApplyIsOpenWindow)
                {
                    Utils.SwalSuccess("办件转办成功", "", "确 认", $"location.href = '{ListPageUrl}'");
                }
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
            }
        }

        public void Translate_OnClick(object sender, EventArgs e)
        {
            var translateChannelId = Utils.ToInt(DdlTranslateChannelId.SelectedValue);
            if (translateChannelId == 0)
            {
                LtlMessage.Text = Utils.GetMessageHtml("转移失败，必须选择转移目标", false);
                return;
            }
            try
            {
                _contentInfo.Set(ContentAttribute.TranslateFromChannelId, _contentInfo.ChannelId.ToString());
                _contentInfo.ChannelId = translateChannelId;
                Main.Instance.ContentApi.Update(SiteId, _contentInfo.ChannelId, _contentInfo);

                if (!string.IsNullOrEmpty(TbTranslateRemark.Text))
                {
                    var remarkInfo = new RemarkInfo(0, SiteId, _contentInfo.ChannelId, _contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.Translate), TbTranslateRemark.Text, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                    Main.RemarkDao.Insert(remarkInfo);
                }

                ApplyManager.LogTranslate(SiteId, _contentInfo.ChannelId, _contentInfo.Id, Main.Instance.ChannelApi.GetChannelName(SiteId, _contentInfo.ChannelId), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);

                LtlMessage.Text = Utils.GetMessageHtml("办件转移成功", true);

                if (!ConfigInfo.ApplyIsOpenWindow)
                {
                    Utils.SwalSuccess("办件转移成功", "", "确 认", $"location.href = '{ListPageUrl}'");
                }
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
            }
        }

        public void Comment_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TbCommentRemark.Text))
                {
                    LtlMessage.Text = Utils.GetMessageHtml("批示失败，必须填写意见", false);
                    return;
                }

                var remarkInfo = new RemarkInfo(0, SiteId, _contentInfo.ChannelId, _contentInfo.Id, ERemarkTypeUtils.GetValue(ERemarkType.Comment), TbCommentRemark.Text, AuthRequest.AdminInfo.DepartmentId, AuthRequest.AdminName, DateTime.Now);
                Main.RemarkDao.Insert(remarkInfo);

                ApplyManager.Log(SiteId, _contentInfo.ChannelId, _contentInfo.Id, ELogTypeUtils.GetValue(ELogType.Comment), AuthRequest.AdminName, AuthRequest.AdminInfo.DepartmentId);

                LtlMessage.Text = Utils.GetMessageHtml("办件批示成功", true);

                if (!ConfigInfo.ApplyIsOpenWindow)
                {
                    Utils.SwalSuccess("办件批示成功", "", "确 认", $"location.href = '{ListPageUrl}'");
                }
            }
            catch (Exception ex)
            {
                LtlMessage.Text = Utils.GetMessageHtml(ex.Message, false);
            }
        }

        private void RptRemarks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var ltlRemarkType = (Literal)e.Item.FindControl("ltlRemarkType");
            var ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
            var ltlDepartmentAndUserName = (Literal)e.Item.FindControl("ltlDepartmentAndUserName");
            var ltlRemark = (Literal)e.Item.FindControl("ltlRemark");

            var departmentId = Utils.EvalInt(e.Item.DataItem, "DepartmentID");
            var userName = Utils.EvalString(e.Item.DataItem, "UserName");
            var addDate = Utils.EvalDateTime(e.Item.DataItem, "AddDate");
            var remarkType = ERemarkTypeUtils.GetEnumType(Utils.EvalString(e.Item.DataItem, "RemarkType"));
            var remark = Utils.EvalString(e.Item.DataItem, "Remark");

            if (string.IsNullOrEmpty(remark))
            {
                e.Item.Visible = false;
            }
            else
            {
                PhRemarks.Visible = true;
                ltlRemarkType.Text = ERemarkTypeUtils.GetText(remarkType);
                ltlAddDate.Text = Utils.GetDateAndTimeString(addDate);
                ltlDepartmentAndUserName.Text = $"{DepartmentManager.GetDepartmentName(departmentId)}({userName})";
                ltlRemark.Text = remark;
            }
        }

        private void RptLogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var ltlDepartment = (Literal)e.Item.FindControl("ltlDepartment");
            var ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
            var ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
            var ltlSummary = (Literal)e.Item.FindControl("ltlSummary");

            var departmentId = Utils.EvalInt(e.Item.DataItem, "DepartmentID");
            var userName = Utils.EvalString(e.Item.DataItem, "UserName");
            var addDate = Utils.EvalDateTime(e.Item.DataItem, "AddDate");
            var summary = Utils.EvalString(e.Item.DataItem, "Summary");

            if (departmentId > 0)
            {
                ltlDepartment.Text = DepartmentManager.GetDepartmentName(departmentId);
            }
            ltlUserName.Text = userName;
            ltlAddDate.Text = Utils.GetDateAndTimeString(addDate);
            ltlSummary.Text = summary;
        }

        public string ListPageUrl => _returnUrl;
    }
}
