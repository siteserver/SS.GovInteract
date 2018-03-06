<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.ModalTypeAdd" %>
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
                        <div class="row">
                        <label class="col-sm-2 control-label text-right">办件类型名称<asp:RequiredFieldValidator ControlToValidate="TbTypeName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
                            /></label>
                        <div class="col-sm-10 form-group form-inline">
                            <asp:TextBox id="TbTypeName" class="form-control"  runat="server" /> 
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
            </form>
        </div>
    </div> 
</div>

    </div> 
    </div>
    </div>
  </body>

  </html>