<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.PageCommonConfiguration" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
  </head>

  <body>
    <div style="padding: 20px 0;">

      <div class="container">
        <form id="form" runat="server">
          <div class="row">
            <div class="card-box">
              <ul class="nav nav-pills"> 
                <li class="nav-item active">
                  <a class="nav-link" href="pageCommonConfiguration.aspx?siteId=<%=SiteId%>">公共设置</a>
                </li>
                <li class="nav-item">
                  <a class="nav-link" href="pageDetailConfiguration.aspx?siteId=<%=SiteId%>">具体设置</a>
                </li>
              </ul>
            </div>
          </div>

          <asp:Literal id="LtlMessage" runat="server" />

          <div class="row">
            <div class="card-box">

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
                          <label for="exampleInputName2">办理时限</label>
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
                      <label for="exampleInputName2">预警后</label>
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
                      <label for="exampleInputName2">黄牌后</label>
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

              <div class="row">
                <div class="col-sm-offset-3 col-sm-9">
                  <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
                </div>
              </div>

            </div>
          </div> 

      </div>

    </div>

    </form>
    </div>
    </div>
  </body>

  </html>