<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.ModalApplyView" %>
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

      <h4>
        <asp:Literal ID="LtlTitle" runat="server"></asp:Literal>
      </h4>

      <table class="table table-striped">
        <tbody>
          <asp:Literal ID="LtlApplyAttributes" runat="server"></asp:Literal>
          <tr>
            <th>提交时间</th>
            <td>
              <asp:Literal ID="LtlAddDate" runat="server"></asp:Literal>
            </td>
            <th>查询号</th>
            <td>
              <asp:Literal ID="LtlQueryCode" runat="server"></asp:Literal>
            </td>
          </tr>
          <tr>
            <th>状态</th>
            <td>
              <asp:Literal ID="LtlState" runat="server"></asp:Literal>
            </td>
            <th>提交部门</th>
            <td>
              <asp:Literal ID="LtlDepartmentName" runat="server"></asp:Literal>
            </td>
          </tr>
        </tbody>
      </table>

      <hr />

      <table class="table">
        <asp:PlaceHolder id="PhRemarks" Visible="false" runat="server">
          <tr>
            <th width="120" class="text-center">意见：</th>
            <td>
              <table class="table">
                <tr>
                  <td>类型</td>
                  <td>日期</td>
                  <td>人员</td>
                  <td>意见</td>
                </tr>
                <asp:Repeater ID="RptRemarks" runat="server">
                  <itemtemplate>
                    <tr>
                      <td>
                        <asp:Literal ID="ltlRemarkType" runat="server"></asp:Literal>
                      </td>
                      <td>
                        <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                      </td>
                      <td>
                        <asp:Literal ID="ltlDepartmentAndUserName" runat="server"></asp:Literal>
                      </td>
                      <td>
                        <asp:Literal ID="ltlRemark" runat="server"></asp:Literal>
                      </td>
                    </tr>
                  </itemtemplate>
                </asp:Repeater>
              </table>
            </td>
          </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder id="PhReply" Visible="false" runat="server">
          <tr>
            <th width="120" class="text-center">办理回复：</th>
            <td>
              <table class="table">
                <tr>
                  <td>办理人员</td>
                  <td>
                    <asp:Literal ID="LtlDepartmentAndUserName" runat="server"></asp:Literal>
                  </td>
                </tr>
                <tr>
                  <td>办理日期</td>
                  <td>
                    <asp:Literal ID="LtlReplyAddDate" runat="server"></asp:Literal>
                  </td>
                </tr>
                <tr>
                  <td>回复内容</td>
                  <td>
                    <asp:Literal ID="LtlReply" runat="server"></asp:Literal>
                  </td>
                </tr>
                <tr>
                  <td>附件</td>
                  <td>
                    <asp:Literal ID="LtlReplyFileUrl" runat="server"></asp:Literal>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </asp:PlaceHolder>
      </table>

    </form>
  </body>

  </html>