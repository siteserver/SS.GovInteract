<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.PageConfigurationChannel" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/siteserver.min.css" rel="stylesheet" type="text/css" />
    <script src="assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/layer/layer.min.js" type="text/javascript"></script>
  </head>

  <body>
    <form class="m-l-15 m-r-15 m-t-15" runat="server">

      <ul class="nav nav-tabs tabs-bordered mb-20">
        <li class="nav-item">
          <a class="nav-link" href="pageConfiguration.aspx?siteId=<%=SiteId%>">通用设置</a>
        </li>
        <li class="nav-item">
          <a class="nav-link active" href="pageConfigurationChannel.aspx?siteId=<%=SiteId%>">分类设置</a>
        </li>
      </ul>

      <div class="bg-white p-3">

        <asp:Literal id="LtlMessage" runat="server" />

        <div class="card-box">

          <h4 class="m-t-0 header-title">
            <b>互动交流分类列表</b>
          </h4>
          <p class="text-muted font-13">
            可以针对每个分类进行单独设置相应的属性。
          </p>

          <div class="tablesaw-bar mode-stack"></div>
          <table class="tablesaw m-t-20 table m-b-0 tablesaw-stack">
            <thead>
              <tr>
                <th scope="col" data-tablesaw-sortable-col="" data-tablesaw-priority="persist">互动交流分类名称</th>
                <th scope="col" data-tablesaw-sortable-col="" data-tablesaw-sortable-default-col="" data-tablesaw-priority="3">操作</th>
              </tr>
            </thead>
            <tbody>
              <asp:Repeater ID="RptContents" runat="server">
                <ItemTemplate>
                  <tr>
                    <td>
                      <asp:Literal ID="ltlName" runat="server"></asp:Literal>
                    </td>
                    <td>
                      <asp:Literal ID="ltlAction" runat="server"></asp:Literal>
                    </td>
                  </tr>
                </ItemTemplate>
              </asp:Repeater>
            </tbody>
          </table>
        </div>

      </div>

    </form>
  </body>

  </html>