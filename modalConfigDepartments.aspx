<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.ModalConfigDepartments" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/responsive.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/core.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/menu.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/components.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/pages.css" rel="stylesheet" type="text/css" />

    <script src="assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/layer/layer.min.js" type="text/javascript"></script>
  </head>

  <body>
    <div style="padding: 20px 0;">
      <div class="container">
        <asp:Literal id="LtlMessage" runat="server" />
        <form class="m-l-15 m-r-15" runat="server">
          <div class="card-box">
            <div class="form-group">
              <small class="form-text text-muted">
                请从下边选择负责部门，所选部门下的所有分类都属于负责范围。
              </small>
              <div class="m-3">
                <asp:Literal ID="LtlDepartmentTree" runat="server"></asp:Literal>
              </div>
              <hr />
              <asp:Button class="btn btn-primary m-l-10" id="BtnAdd" text="确 定" OnClick="Submit_OnClick" runat="server" />
            </div>
          </div>
        </form>
      </div>
    </div>
  </body>

  </html>