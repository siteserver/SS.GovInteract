<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.PageFormStyle" %>
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
          前台页面标签
        </h4>

        <div class="form" role="form">

          <asp:PlaceHolder id="PhConfig" runat="server">
            <div class="row">
              <label class="col-2 control-label text-right">类型</label>
              <div class="col-10 form-group form-inline">
                <asp:DropDownList ID="DdlType" class="form-control" runat="server"></asp:DropDownList>
              </div>
            </div>

            <div class="row">
              <label class="col-2 control-label text-right">对应栏目</label>
              <div class="col-10 form-group form-inline">
                <asp:DropDownList ID="DdlChannelId" class="form-control" runat="server"></asp:DropDownList>
              </div>
            </div>

            <div class="row">
              <label class="col-2 control-label text-right">ApiKey</label>
              <div class="col-10 form-group form-inline">
                <asp:TextBox ID="TbApiKey" style="width: 350px" class="form-control" runat="server"></asp:TextBox>
              </div>
            </div>

            <hr />

            <div class="text-center">
              <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
            </div>
          </asp:PlaceHolder>

          <asp:PlaceHolder id="PhCode" runat="server" visible="false">
            <div class="row">
              <label class="col-2 control-label text-right">前台页面标签</label>
              <div class="col-10 form-group">
                <asp:TextBox ID="TbCode" style="height: 450px;" TextMode="MultiLine" class="form-control" runat="server"></asp:TextBox>
              </div>
            </div>
          </asp:PlaceHolder>

        </div>

      </div>

    </form>
  </body>

  </html>