<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.PageConfiguration" %>
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

      <ul class="nav nav-tabs tabs-bordered mb-20">
        <li class="nav-item">
          <a class="nav-link active" href="pageConfiguration.aspx?siteId=<%=SiteId%>">通用设置</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" href="pageConfigurationChannel.aspx?siteId=<%=SiteId%>">分类设置</a>
        </li>
      </ul>

      <div class="bg-white p-3">

        <asp:Literal id="LtlMessage" runat="server" />

        <div class="row">
          <label class="col-sm-2 control-label text-right">办理时限</label>
          <div class="col-sm-10 form-group form-inline">
            <asp:TextBox id="TbApplyDateLimit" class="form-control" MaxLength="4" Text="0" runat="server" />
            <label for="exampleInputEmail2">日</label>
          </div>
        </div>

        <div class="row">
          <label class="col-sm-2 control-label text-right">预警</label>
          <div class="col-sm-10 form-group">
            <div class="row col-sm-12 form-inline">
              <div class="form-group m-r-10">
                <label class="mr-1">办理时限</label>
                <asp:DropDownList ID="DdlApplyAlertDateIsAfter" class="form-control" runat="server">
                  <asp:ListItem Text="前" Value="False" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="后" Value="True"></asp:ListItem>
                </asp:DropDownList>
              </div>
              <div class="form-group m-r-10">
                <asp:TextBox ID="TbApplyAlertDate" class="form-control" MaxLength="4" Text="0" runat="server" />
                <label for="exampleInputEmail2">日</label>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <label class="col-sm-2 control-label text-right">黄牌</label>
          <div class="col-sm-10 form-group">
            <div class="row col-sm-12 form-inline">
              <div class="form-group m-r-10">
                <label class="mr-1">预警后</label>
                <asp:TextBox ID="TbApplyYellowAlertDate" class="form-control" MaxLength="4" Text="0" runat="server" />
                <label for="exampleInputEmail2">日</label>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <label class="col-sm-2 control-label text-right">红牌</label>
          <div class="col-sm-10 form-group">
            <div class="row col-sm-12 form-inline">
              <div class="form-group m-r-10">
                <label class="mr-1">黄牌后</label>
                <asp:TextBox ID="TbApplyRedAlertDate" class="form-control" MaxLength="4" Text="0" runat="server" />
                <label for="exampleInputEmail2">日</label>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <label class="col-sm-2 control-label text-right">办件是否可删除</label>
          <div class="col-sm-10 form-group">
            <div class="row col-sm-12 form-inline">
              <div class="form-group m-r-10">
                <asp:DropDownList ID="DdlApplyIsDeleteAllowed" class="form-control" runat="server">
                  <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="是" Value="True"></asp:ListItem>
                </asp:DropDownList>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <label class="col-sm-2 control-label text-right">办件是否新窗口打开</label>
          <div class="col-sm-10 form-group">
            <div class="row col-sm-12 form-inline">
              <div class="form-group m-r-10">
                <asp:DropDownList ID="DdlApplyIsOpenWindow" class="form-control" runat="server">
                  <asp:ListItem Text="是" Value="True"></asp:ListItem>
                  <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
                </asp:DropDownList>
              </div>
            </div>
          </div>
        </div>

        <hr />

        <div class="text-center">
          <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
        </div>

      </div>

    </form>
  </body>

  </html>