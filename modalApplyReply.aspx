<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.ModalApplyReply" %>
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
        <table class="table">
          <tr>
            <td colspan="2">
              <div class="alert alert-primary">办理办件后信息将变为待审核状态</div>
            </td>
          </tr>
          <tr>
            <td class="text-center" width="120">答复内容：</td>
            <td>
              <asp:TextBox id="TbReply" class="form-control" runat="server" TextMode="MultiLine" style="width:80%;height:120px;"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td class="text-center">答复部门：</td>
            <td>
              <asp:Literal ID="LtlDepartmentName" runat="server"></asp:Literal>
            </td>
          </tr>
          <tr>
            <td class="text-center">答复人：</td>
            <td>
              <asp:Literal ID="LtlUserName" runat="server"></asp:Literal>
            </td>
          </tr>
          <tr>
            <td class="text-center">附件上传：</td>
            <td>
              <input id="HtmlFileUrl" runat="server" type="file" class="form-control" />
            </td>
          </tr>
        </table>

        <hr />

        <div class="text-center">
          <asp:Button class="btn btn-primary" OnClick="Submit_OnClick" Text="提 交" runat="server"></asp:Button>
          &nbsp;&nbsp;
          <input type="button" value="取 消" onClick="window.parent.layer.closeAll()" class="btn" />
        </div>

      </div>

    </form>
  </body>

  </html>