<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.ModalConfigAdministrators" %>
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
          <asp:Literal id="LtlMessage" runat="server" /> 
          <form id="form" runat="server" class="form-horizontal">
          <div class="row"> 
                <div class="card-box">  
                    <div class="form-horizontal"> 
                        <asp:dataGrid id="DgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-hover" gridlines="none" runat="server">
                            <Columns>
                                <asp:TemplateColumn HeaderText="部门">
                                    <ItemTemplate>
                                        &nbsp;<asp:Literal ID="ltlDepartmentName" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="200" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="管理员账号">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="120" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="姓名">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="120" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="权限">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlPermissions" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="100" cssClass="center" />
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:dataGrid>  
                    </div>
                </div> 
            </div>  
        </form>
        </div>
    </div>  
  </body>

  </html>