<%@ Page Language="C#" Inherits="SS.GovInteract.Pages.PageContentCheck" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/siteserver.min.css" rel="stylesheet" type="text/css" />
    <script src="assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/sweetalert/sweetalert.min.js" type="text/javascript"></script>
    <script src="assets/layer/layer.min.js" type="text/javascript"></script>
    <script>
      function showAction(divID) {
        $('.action').hide();
        $('#' + divID).show();
        $('html,body').animate({
          scrollTop: $('#' + divID).offset().top
        }, 1000);
      }

      function showOp(e) {
        $('#tabContent1').show();
        $('#tabContent2').hide();
        $('.nav-link').removeClass('active');
        $(e).addClass('active');
      }

      function showLogs(e) {
        $('#tabContent1').hide();
        $('#tabContent2').show();
        $('.nav-link').removeClass('active');
        $(e).addClass('active');
      }
    </script>
  </head>

  <body>
    <form class="m-l-15 m-r-15 m-t-15" runat="server">

      <ul class="nav nav-tabs tabs-bordered mb-20">
        <li class="nav-item">
          <a class="nav-link active" href="javascript:;" onclick="showOp(this)">待审核办件</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" href="javascript:;" onclick="showLogs(this)">流动轨迹（操作日志）</a>
        </li>
      </ul>

      <div id="tabContent1" class="bg-white p-3">

        <asp:Literal id="LtlMessage" runat="server" />

        <h3 class="text-center m-t-5 m-b-15">
          <asp:Literal ID="LtlTitle" runat="server"></asp:Literal>
        </h3>

        <table class="table table-striped">
          <tbody>
            <tr>
              <td colspan="6">
                <asp:Literal ID="LtlContent" runat="server"></asp:Literal>
              </td>
            </tr>
          </tbody>
        </table>

        <hr />

        <table class="table table-striped">
          <tbody>
            <asp:Literal ID="LtlApplyAttributes" runat="server"></asp:Literal>
          </tbody>
        </table>

        <hr />

        <table class="table">
          <asp:PlaceHolder id="PhRemarks" Visible="false" runat="server">
            <tr>
              <th width="120" class="text-center">意见：</th>
              <td>
                <table class="table">
                  <tr>
                    <td>类型</td>
                    <td>日期</td>
                    <td>人员</td>
                    <td>意见</td>
                  </tr>
                  <asp:Repeater ID="RptRemarks" runat="server">
                    <itemtemplate>
                      <tr>
                        <td>
                          <asp:Literal ID="ltlRemarkType" runat="server"></asp:Literal>
                        </td>
                        <td>
                          <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                        </td>
                        <td>
                          <asp:Literal ID="ltlDepartmentAndUserName" runat="server"></asp:Literal>
                        </td>
                        <td>
                          <asp:Literal ID="ltlRemark" runat="server"></asp:Literal>
                        </td>
                      </tr>
                    </itemtemplate>
                  </asp:Repeater>
                </table>
              </td>
            </tr>
          </asp:PlaceHolder>
          <asp:PlaceHolder id="PhReply" Visible="false" runat="server">
            <tr>
              <th width="120" class="text-center">办理回复：</th>
              <td>
                <table class="table">
                  <tr>
                    <td>办理人员</td>
                    <td>
                      <asp:Literal ID="LtlDepartmentAndUserName" runat="server"></asp:Literal>
                    </td>
                  </tr>
                  <tr>
                    <td>办理日期</td>
                    <td>
                      <asp:Literal ID="LtlReplyAddDate" runat="server"></asp:Literal>
                    </td>
                  </tr>
                  <tr>
                    <td>回复内容</td>
                    <td>
                      <asp:Literal ID="LtlReply" runat="server"></asp:Literal>
                    </td>
                  </tr>
                  <tr>
                    <td>附件</td>
                    <td>
                      <asp:Literal ID="LtlReplyFileUrl" runat="server"></asp:Literal>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </asp:PlaceHolder>
          <tr>
            <td colspan="2">
              <asp:PlaceHolder ID="PhBtnCheck" runat="server">
                <input type="button" value="审 核" onClick="showAction('divCheck');return false;" class="btn m-r-5 btn-success" />
                <input type="button" value="要求返工" onClick="showAction('divRedo');return false;" class="btn m-r-5" />
              </asp:PlaceHolder>
              <asp:PlaceHolder id="PhBtnComment" runat="server">
                <input type="button" value="批 示" onClick="showAction('divComment');return false;" class="btn m-r-5" />
              </asp:PlaceHolder>
              <asp:PlaceHolder id="PhBtnReturn" runat="server">
                <input type="button" value="返 回" onClick="javascript:location.href='<%=ListPageUrl%>';return false;" class="btn m-r-5" />
              </asp:PlaceHolder>
            </td>
          </tr>
        </table>

        <div id="divCheck" class="card-box m-t-10 action" style="display:none">
          <div class="card-title">
            <strong>审核办件</strong>
          </div>
          <div class="card-body">
            <table class="table">
              <tr>
                <td colspan="2">
                  <div class="alert alert-primary">审核办件后信息将变为已审核状态</div>
                </td>
              </tr>
              <tr>
                <td class="text-center" width="120">答复内容：</td>
                <td>
                  <asp:TextBox id="TbReply" class="form-control" runat="server" TextMode="MultiLine" style="width:80%;height:120px;"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td class="text-center" width="120">审核部门：</td>
                <td>
                  <%=MyDepartment%>
                </td>
              </tr>
              <tr>
                <td class="text-center">审核人：</td>
                <td>
                  <%=MyDisplayName%>
                </td>
              </tr>
            </table>

            <hr />

            <div class="text-center">
              <asp:Button class="btn btn-primary" OnClick="Check_OnClick" Text="提 交" runat="server"></asp:Button>
              &nbsp;&nbsp;
              <input type="button" value="取 消" onClick="$('#divCheck').hide();" class="btn" />
            </div>

          </div>
        </div>

        <div id="divRedo" class="card-box m-t-10 action" style="display:none">
          <div class="card-title">
            <strong>要求返工</strong>
          </div>
          <div class="card-body">
            <table class="table">
              <tr>
                <td class="text-center" width="120">返工意见：</td>
                <td>
                  <asp:TextBox ID="TbRedoRemark" class="form-control" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td class="text-center">办理部门：</td>
                <td>
                  <%=MyDepartment%>
                </td>
              </tr>
              <tr>
                <td class="text-center">办理人：</td>
                <td>
                  <%=MyDisplayName%>
                </td>
              </tr>
            </table>

            <hr />

            <div class="text-center">
              <asp:Button class="btn btn-primary" OnClick="Redo_OnClick" Text="提 交" runat="server"></asp:Button>
              &nbsp;&nbsp;
              <input type="button" value="取 消" onClick="$('#divRedo').hide();" class="btn" />
            </div>

          </div>
        </div>

        <div id="divSwitchTo" class="card-box m-t-10 action" style="display:none">
          <div class="card-title">
            <strong>转办办件</strong>
          </div>
          <div class="card-body">
            <table class="table">
              <tr>
                <td class="text-center" width="120">转办到：</td>
                <td>
                  <div class="fill_box" id="switchToDepartmentContainer" style="display:none">
                    <div class="addr_base addr_normal">
                      <b id="switchToDepartmentName"></b>
                      <a class="addr_del" href="javascript:;" onClick="showswitchToDepartment('', '0')"></a>
                      <input id="switchToDepartmentID" name="switchToDepartmentID" value="0" type="hidden">
                    </div>
                  </div>
                  <div ID="DivAddDepartment" class="btn_pencil" runat="server">
                    <button class="btn btn-success">选择</button>
                  </div>
                  <script language="javascript">
                    function showCategoryDepartment(departmentName, departmentID) {
                      $('#switchToDepartmentName').html(departmentName);
                      $('#switchToDepartmentID').val(departmentID);
                      if (departmentID == '0') {
                        $('#switchToDepartmentContainer').hide();
                      } else {
                        $('#switchToDepartmentContainer').show();
                      }
                    }
                  </script>
                  <asp:Literal ID="LtlScript" runat="server"></asp:Literal>
                </td>
              </tr>
              <tr>
                <td class="text-center">意见：</td>
                <td>
                  <asp:TextBox id="TbSwitchToRemark" class="form-control" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td class="text-center">操作部门：</td>
                <td>
                  <%=MyDepartment%>
                </td>
              </tr>
              <tr>
                <td class="text-center">操作人：</td>
                <td>
                  <%=MyDisplayName%>
                </td>
              </tr>
            </table>

            <hr />

            <div class="text-center">
              <asp:Button class="btn btn-primary" OnClick="SwitchTo_OnClick" Text="提 交" runat="server"></asp:Button>
              &nbsp;&nbsp;
              <input type="button" value="取 消" onClick="$('#divSwitchTo').hide();" class="btn" />
            </div>

          </div>
        </div>

        <div id="divTranslate" class="card-box m-t-10 action" style="display:none">
          <div class="card-title">
            <strong>转移办件</strong>
          </div>
          <div class="card-body">
            <table class="table">
              <tr>
                <td colspan="2">
                  <div class="alert alert-primary">注意：此操作将吧办件转移到对应的分类中</div>
                </td>
              </tr>
              <tr>
                <td class="text-center" width="120">转移到：</td>
                <td>
                  <asp:DropDownList ID="DdlTranslateNodeID" runat="server"></asp:DropDownList>
                </td>
              </tr>
              <tr>
                <td class="text-center">意见：</td>
                <td>
                  <asp:TextBox id="TbTranslateRemark" class="form-control" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td class="text-center">操作部门：</td>
                <td>
                  <%=MyDepartment%>
                </td>
              </tr>
              <tr>
                <td class="text-center">操作人：</td>
                <td>
                  <%=MyDisplayName%>
                </td>
              </tr>
            </table>

            <hr />

            <div class="text-center">
              <asp:Button class="btn btn-primary" OnClick="Translate_OnClick" Text="提 交" runat="server"></asp:Button>
              &nbsp;&nbsp;
              <input type="button" value="取 消" onClick="$('#divTranslate').hide();" class="btn" />
            </div>

          </div>
        </div>

        <div id="divComment" class="card-box m-t-10 action" style="display:none">
          <div class="card-title">
            <strong>批示办件</strong>
          </div>
          <div class="card-body">
            <table class="table">
              <tr>
                <td class="text-center" width="120">批示意见：</td>
                <td>
                  <asp:TextBox id="TbCommentRemark" class="form-control" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td class="text-center">批示部门：</td>
                <td>
                  <%=MyDepartment%>
                </td>
              </tr>
              <tr>
                <td class="text-center">批示人：</td>
                <td>
                  <%=MyDisplayName%>
                </td>
              </tr>
            </table>

            <hr />

            <div class="text-center">
              <asp:Button class="btn btn-primary" OnClick="Comment_OnClick" Text="提 交" runat="server"></asp:Button>
              &nbsp;&nbsp;
              <input type="button" value="取 消" onClick="$('#divComment').hide();" class="btn" />
            </div>

          </div>
        </div>

      </div>

      <div id="tabContent2" class="bg-white p-3" style="display: none">
        <table class="table table-striped">
          <tr class="info thead">
            <td>操作部门</td>
            <td>操作人</td>
            <td>操作时间</td>
            <td>操作内容</td>
          </tr>
          <asp:Repeater ID="RptLogs" runat="server">
            <itemtemplate>
              <tr>
                <td class="text-center">
                  <asp:Literal ID="ltlDepartment" runat="server"></asp:Literal>
                </td>
                <td class="text-center">
                  <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                </td>
                <td class="text-center">
                  <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                </td>
                <td>
                  <asp:Literal ID="ltlSummary" runat="server"></asp:Literal>
                </td>
              </tr>
            </itemtemplate>
          </asp:Repeater>
        </table>
      </div>

    </form>
  </body>

  </html>