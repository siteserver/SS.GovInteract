<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.PageDetailConfiguration" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
		<script src="assets/js/jquery.min.js" type="text/javascript"></script>
		<script src="assets/layer/layer.min.js" type="text/javascript"></script>
  </head>

  <body>
    <div style="padding: 20px 0;">

      <div class="container"> 
          <div class="row">
            <div class="card-box">
              <ul class="nav nav-pills"> 
                <li class="nav-item">
                  <a class="nav-link" href="pageCommonConfiguration.aspx?siteId=<%=SiteId%>">公共设置</a>
                </li>
                <li class="nav-item active">
                  <a class="nav-link" href="pageDetailConfiguration.aspx?siteId=<%=SiteId%>">具体设置</a>
                </li>
              </ul>
            </div>
          </div>

          <asp:Literal id="LtlMessage" runat="server" />

          <div class="row"> 
                <div class="card-box">

                    <h4 class="m-t-0 header-title"><b>互动交流分类列表</b></h4>
                    <p class="text-muted font-13">
                        可以针对每个分类进行单独设置相应的属性。
                    </p>

                    <div class="tablesaw-bar mode-stack"></div><table class="tablesaw m-t-20 table m-b-0 tablesaw-stack" data-tablesaw-mode="stack" id="table-4068">
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
                                  <td><asp:Literal ID="ltlName" runat="server"></asp:Literal></td>
                                  <td><asp:Literal ID="ltlAction" runat="server"></asp:Literal></td> 
                                </tr>
                              </ItemTemplate>
                        </asp:Repeater> 
                        </tbody>
                    </table>
                </div> 
        </div> 

            </div>
          </div> 

      </div>

    </div> 
    </div>
    </div>
  </body>

  </html>