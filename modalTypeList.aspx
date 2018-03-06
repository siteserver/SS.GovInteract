<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.ModalTypeList" %>
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
                        <asp:dataGrid id="DgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-hover"
                            gridlines="none" runat="server">
                            <Columns>
                                <asp:TemplateColumn HeaderText="办件类型">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlTypeName" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle cssClass="center" />
                                </asp:TemplateColumn> 
                                <asp:TemplateColumn HeaderText="上升">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlUpLinkButton" runat="server">
                                            上升
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" cssClass="center" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="下降">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlDownLinkButton" runat="server">
                                            下降
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" cssClass="center" />
                                </asp:TemplateColumn> 
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" cssClass="center" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" cssClass="center" />
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:dataGrid>

                        <hr />

                        <div class="form-group">
                            <div class="col-xs-12">
                                <asp:Button class="btn btn-primary m-l-10" id="BtnAdd" text="新增办件类型" runat="server" />
                            </div>
                        </div> 
                    </div>
                </div> 
            </div>  
        </form>
        </div>
    </div> 
</div>

    </div> 
    </div>
    </div>
  </body>

  </html>