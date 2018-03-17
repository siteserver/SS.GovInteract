<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.PageListAll" %>
  <%@ Register TagPrefix="ctrl" Namespace="SS.GovInteract.Controls" Assembly="SS.GovInteract" %>
    <!DOCTYPE html>
    <html>

    <head>
      <meta charset="utf-8">
      <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
      <link href="assets/css/ionicons.min.css" rel="stylesheet" type="text/css" />
      <link href="assets/css/siteserver.min.css" rel="stylesheet" type="text/css" />
    </head>

    <body>
      <form class="m-l-15 m-r-15 m-t-15" runat="server">

        <div class="card-box">
          <h4 class="m-t-0 m-b-20 header-title">
            所有办件(办件数：
            <asp:Literal id="ltlTotalCount" runat="server" />)
          </h4>

          <div class="form-inline" role="form">
            <div class="form-group">
              <label for="ddlTaxis" class="mr-sm-2">排序</label>
              <asp:DropDownList ID="ddlTaxis" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
            </div>

            <div class="form-group m-l-10">
              <label for="ddlState" class="mr-sm-2">办理状态</label>
              <asp:DropDownList ID="ddlState" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
            </div>

            <div class="form-group m-l-10">
              <label for="tbDateFrom" class="mr-sm-2">时间</label>
              <ctrl:DateTimeTextBox ID="tbDateFrom" class="form-control" Columns="12" runat="server" />
              <label for="tbDateTo" class="m-sm-2">至</label>
              <ctrl:DateTimeTextBox ID="tbDateTo" class="form-control" Columns="12" runat="server" />
            </div>

            <div class="form-group m-l-10">
              <label for="tbKeyword" class="mr-sm-2">关键字</label>
              <asp:TextBox ID="tbKeyword" class="form-control" runat="server"></asp:TextBox>
            </div>

            <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn btn-success m-l-10 btn-md" runat="server"></asp:Button>
          </div>

          <div class="btn-toolbar mt-4 mb-4" role="toolbar">
            <div class="btn-group">

              <asp:PlaceHolder ID="phAccept" runat="server">
                <asp:HyperLink ID="hlAccept" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="受 理"></asp:HyperLink>
                <asp:HyperLink ID="hlDeny" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="拒 绝"></asp:HyperLink>
              </asp:PlaceHolder>
              <asp:PlaceHolder ID="phCheck" runat="server">
                <asp:HyperLink ID="hlCheck" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="审 核"></asp:HyperLink>
                <asp:HyperLink ID="hlRedo" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="要求返工"></asp:HyperLink>
              </asp:PlaceHolder>
              <asp:PlaceHolder ID="phSwitchToTranslate" runat="server">
                <asp:HyperLink ID="hlSwitchTo" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="转 办"></asp:HyperLink>
              </asp:PlaceHolder>
              <asp:PlaceHolder ID="phComment" runat="server">
                <asp:HyperLink ID="hlComment" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="批 示"></asp:HyperLink>
              </asp:PlaceHolder>
              <asp:PlaceHolder ID="phDelete" runat="server">
                <asp:HyperLink ID="hlDelete" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="删 除"></asp:HyperLink>
              </asp:PlaceHolder>
              <asp:HyperLink ID="hlExport" class="btn btn-light text-secondary" NavigateUrl="javascript:;" runat="server" Text="导 出"></asp:HyperLink>

            </div>
          </div>

          <div class="table-responsive" data-pattern="priority-columns">
            <table class="table table-striped">
              <thead>
                <tr>
                  <th>编号</th>
                  <th>办件标题(点击进入操作界面) </th>
                  <th>提交日期</th>
                  <th>意见</th>
                  <th>办理部门</th>
                  <th>期限</th>
                  <th>状态</th>
                  <th>流动轨迹</th>
                  <th>快速查看</th>
                  <th>回复办件</th>
                  <th></th>
                  <th width="20" class="text-center">
                    <input onclick="_checkFormAll(this.checked)" type="checkbox" />
                  </th>
                </tr>
              </thead>
              <tbody>
                <asp:Repeater ID="rptContents" runat="server">
                  <itemtemplate>
                    <asp:Literal ID="ltlTr" runat="server"></asp:Literal>
                    <td class="center">
                      <asp:Literal ID="ltlID" runat="server"></asp:Literal>
                    </td>
                    <td>
                      <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                    </td>
                    <td>
                      <asp:Literal ID="ltlRemark" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlDepartment" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlLimit" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlState" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlFlowUrl" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlViewUrl" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlReplyUrl" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                      <input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
                    </td>
                    </tr>
                  </itemtemplate>
                </asp:Repeater>
              </tbody>
            </table>
          </div>

          <ctrl:sqlPager id="spContents" runat="server" class="table table-pager" />

        </div>

      </form>
    </body>

    </html>