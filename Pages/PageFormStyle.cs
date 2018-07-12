using System;
using System.Text;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;
using SS.GovInteract.Provider;

namespace SS.GovInteract.Pages
{
    public class PageFormStyle : PageBase
    {
        public PlaceHolder PhConfig;
        public DropDownList DdlType;
        public DropDownList DdlChannelId;
        public TextBox TbApiKey;

        public PlaceHolder PhCode;
        public TextBox TbCode;

        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageFormStyle)}.aspx?siteId={siteId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DdlType.Items.Add(new ListItem("提交样式", "Add"));
                DdlType.Items.Add(new ListItem("查询样式", "Query"));

                foreach (var channelInfo in ChannelInfoList)
                {
                    var listItem = new ListItem(channelInfo.ChannelName, channelInfo.Id.ToString());
                    DdlChannelId.Items.Add(listItem);
                }
            }
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (Page.IsPostBack && Page.IsValid)
            {
                PhConfig.Visible = false;

                var type = DdlType.SelectedValue;
                var channelId = Utils.ToInt(DdlChannelId.SelectedValue);
                var apiKey = TbApiKey.Text;

                PhCode.Visible = true;

                if (type == "Add")
                {
                    var departmentBuilder = new StringBuilder();
                    var departmentInfoList = Main.Instance.DepartmentDao.GetDepartmentInfoList();

                    foreach (var departmentInfo in departmentInfoList)
                    {
                        departmentBuilder.Append(
                            $@"<option value=""{departmentInfo.Id}"">{departmentInfo.DepartmentName}</option>").AppendLine();
                    }

                    var typeBuilder = new StringBuilder();
                    var typeInfoList = Main.Instance.TypeDao.GetTypeInfoList(channelId);
                    foreach (var typeInfo in typeInfoList)
                    {
                        typeBuilder.Append(
                            $@"<option value=""{typeInfo.Id}"">{typeInfo.TypeName}</option>").AppendLine();
                    }

                    TbCode.Text = $@"<script src=""https://unpkg.com/vue/dist/vue.min.js""></script>
<script src=""https://unpkg.com/axios/dist/axios.min.js""></script>

<style>
  #govInteractAdd table {{
    width: 100%;
    height: 100%;
    border-left: 1px solid silver;
    border-top: 1px solid silver
  }}

  #govInteractAdd td {{
    height: 30px;
    padding: 5px;
    margin: 5px;
    text-align: right;
    padding-right: 8px;
    width: 90px;
    background: #F8F8F8;
    border-bottom: 1px solid silver;
    border-right: 1px solid silver
  }}

  #govInteractAdd td.field {{
    text-align: left;
  }}

  #govInteractAdd input {{
    padding-left: 5px;
    margin-left: 8px;
    width: 200px;
    height: 24px;
    line-height: 20px;
    border: 1px solid #9AABBB
  }}

  #govInteractAdd select {{
    margin-left: 8px;
    height: 24px;
    line-height: 20px;
    border: 1px solid #9AABBB
  }}

  #govInteractAdd textarea {{
    padding-left: 5px;
    margin-left: 8px;
    border: 1px solid #9AABBB;
    line-height: 20px;
    width: 90%;
    height: 180px;
  }}

  #govInteractAdd .error {{
    border: 1px solid #F00 !important;
  }}

  #govInteractAdd .buttons {{
    margin: 10px auto;
    text-align: center;
  }}

  #govInteractAdd .button {{
    cursor: pointer;
    background-color: #4CAF50;
    border: none;
    color: white;
    padding: 8px 22px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
  }}

  #govInteractAdd a.button {{
    text-decoration: none;
  }}

  #govInteractAdd .success {{
    margin: 0 auto;
    font-size: 16px;
    color: #090 !important;
    padding: 10px;
    text-align: left;
    line-height: 160%;
    font-weight: 700
  }}

  #govInteractAdd .failure {{
    margin: 0 auto;
    font-size: 16px;
    color: #C00 !important;
    padding: 10px;
    text-align: left;
    line-height: 160%;
    font-weight: 700
  }}
</style>


<div id=""govInteractAdd"">
  <template v-if=""elType === 'success'"">
    <div class=""success"">
      您的信息已经提交成功，相关部门将会尽快处理
      <br /> 您可以通过查询号查询到该信息的办理情况
      <br /> 请您记住查询号：{{{{ content.queryCode }}}}
    </div>
  </template>

  <template v-if=""elType === 'failure'"">
    <div class=""failure"">
      提交失败，请确认提交内容是否正确。
    </div>
  </template>

  <template v-if=""elType === 'form' || elType === 'failure'"">

    <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
      <tbody>
        <tr>
          <td class=""attribute"">姓名</td>
          <td class=""field"">
            <input v-model=""content.realName"" v-bind:class=""{{ error: isSubmit && !content.realName }}"" type=""text"" />
          </td>
          <td class=""attribute"">工作单位</td>
          <td class=""field"">
            <input v-model=""content.organization"" v-bind:class=""{{ error: isSubmit && !content.organization }}"" type=""text"" />
          </td>
        </tr>
        <tr>
          <td class=""attribute"">证件名称</td>
          <td class=""field"">
            <select id=""CardType"" name=""CardType"">
              <option value=""身份证"" selected>身份证</option>
              <option value=""学生证"">学生证</option>
              <option value=""军官证"">军官证</option>
              <option value=""工作证"">工作证</option>
            </select>
          </td>
          <td class=""attribute"">证件号码</td>
          <td class=""field"">
            <input v-model=""content.cardNo"" v-bind:class=""{{ error: isSubmit && !content.cardNo }}"" type=""text"" />
          </td>
        </tr>
        <tr>
          <td class=""attribute"">联系电话</td>
          <td class=""field"">
            <input v-model=""content.phone"" v-bind:class=""{{ error: isSubmit && !content.phone }}"" type=""text"" />
          </td>
          <td class=""attribute"">邮政编码</td>
          <td class=""field"">
            <input v-model=""content.postCode"" v-bind:class=""{{ error: isSubmit && !content.postCode }}"" type=""text"" />
          </td>
        </tr>
        <tr>
          <td class=""attribute"">联系地址</td>
          <td colspan=""3"" class=""field"">
            <input v-model=""content.address"" v-bind:class=""{{ error: isSubmit && !content.address }}"" type=""text"" value="""" style=""width:380px;""
            />
          </td>
        </tr>
        <tr>
          <td class=""attribute"">电子邮件</td>
          <td class=""field"">
            <input v-model=""content.email"" v-bind:class=""{{ error: isSubmit && !content.email }}"" type=""text"" />
          </td>
          <td class=""attribute"">传真</td>
          <td class=""field"">
            <input v-model=""content.fax"" type=""text"" />
          </td>
        </tr>
        <tr>
          <td class=""attribute"">类型</td>
          <td class=""field"">
            <select v-model=""content.typeId"" v-bind:class=""{{ error: isSubmit && content.typeId === 0 }}"">
              <option value=""0"">&lt;&lt;请选择&gt;&gt;</option>
              {typeBuilder}
            </select>
          </td>
          <td class=""attribute"">是否公开</td>
          <td class=""field"">
            <select v-model=""content.isPublic"">
              <option value=""true"">公开</option>
              <option value=""false"">不公开</option>
            </select>
          </td>
        </tr>
        <tr>
          <td class=""attribute"">标题</td>
          <td colspan=""3"" class=""field"">
            <input v-model=""content.title"" v-bind:class=""{{ error: isSubmit && !content.title }}"" type=""text"" style=""width:380px;"" />
          </td>
        </tr>
        <tr>
          <td class=""attribute"">内容</td>
          <td colspan=""3"" class=""field"">
            <textarea v-model=""content.content"" v-bind:class=""{{ error: isSubmit && !content.content }}""></textarea>
          </td>
        </tr>
        <tr>
          <td class=""attribute"">提交对象</td>
          <td colspan=""3"" class=""field"">
            <select v-model=""content.departmentId"" v-bind:class=""{{ error: isSubmit && content.departmentId === 0 }}"">
              <option value=""0"">&lt;&lt;请选择&gt;&gt;</option>
              {departmentBuilder}
            </select>
          </td>
        </tr>
      </tbody>
    </table>
    <div class=""buttons"">
      <a href=""javascript:;"" class=""button"" v-on:click=""submit()"">提 交</a>
    </div>

  </template>
</div>

<script type=""text/javascript"">
  var govInteractAdd = new Vue({{
    el: '#govInteractAdd',
    data: {{
      elType: 'form',
      isSubmit: false,
      content: {{
        cardType: '身份证',
        isPublic: true,
        typeId: 0,
        departmentId: 0
      }}
    }},
    methods: {{
      s4: function () {{
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
      }},
      queryCode: function () {{
        return this.s4().toUpperCase() + this.s4().toUpperCase() + this.s4().toUpperCase() + this.s4().toUpperCase();
      }},
      submit: function () {{
        var $this = this;

        this.isSubmit = true;
        var content = this.content;

        if (!content.realName ||
          !content.organization ||
          !content.cardNo ||
          !content.phone ||
          !content.postCode ||
          !content.address ||
          !content.email ||
          content.typeId === 0 ||
          !content.title ||
          !content.content ||
          content.departmentId === 0) {{
          return;
        }}

        content.queryCode = this.queryCode();

        axios.post('/api/v1/contents/{SiteId}/{channelId}', content, {{
            withCredentials: true,
            headers: {{
              'X-SS-API-KEY': '{apiKey}'
            }},
          }})
          .then(function (response) {{
            if (response.data) {{
              $this.elType = 'success';
            }}
          }})
          .catch(function (error) {{
            $this.elType = 'failure';
          }});

      }}
    }}
  }})
</script>
";
                }
                else
                {
                    TbCode.Text = $@"<script src=""https://unpkg.com/vue/dist/vue.min.js""></script>
<script src=""https://unpkg.com/axios/dist/axios.min.js""></script>

<style>
#govInteractQuery table {{
    width: 100%;
    height: 100%;
    border-left: 1px solid silver;
    border-top: 1px solid silver
}}

#govInteractQuery td {{
    height: 30px;
    padding: 5px;
    margin: 5px;
    text-align: right;
    padding-right: 8px;
    width: 90px;
    background: #F8F8F8;
    border-bottom: 1px solid silver;
    border-right: 1px solid silver
}}

#govInteractQuery td.field {{
    text-align: left;
}}

#govInteractQuery input {{
    padding-left: 5px;
    margin-left: 8px;
    width: 200px;
    height: 24px;
    line-height: 20px;
    border: 1px solid #9AABBB
}}

#govInteractQuery select {{
    margin-left: 8px;
    height: 24px;
    line-height: 20px;
    border: 1px solid #9AABBB
}}

#govInteractQuery textarea {{
    padding-left: 5px;
    margin-left: 8px;
    border: 1px solid #9AABBB;
    line-height: 20px;
    width: 90%;
    height: 180px;
}}

#govInteractQuery .error {{
    border: 1px solid #F00 !important;
}}

#govInteractQuery .buttons {{
    margin: 10px auto;
    text-align: center;
}}

#govInteractQuery .button {{
    cursor: pointer;
    background-color: #4CAF50;
    border: none;
    color: white;
    padding: 8px 22px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
}}

#govInteractQuery a.button {{
    text-decoration: none;
}}

#govInteractQuery .success {{
    margin: 0 auto;
    font-size: 16px;
    color: #090 !important;
    padding: 10px;
    text-align: left;
    line-height: 160%;
    font-weight: 700
}}

#govInteractQuery .failure {{
    margin: 0 auto;
    font-size: 16px;
    color: #C00 !important;
    padding: 10px;
    text-align: left;
    line-height: 160%;
    font-weight: 700
}}
</style>

<div id=""govInteractQuery"">
<template v-if=""elType === 'success'"">
    <div class=""success"">
    <table border=""0"" cellspacing=""0"" cellpadding=""0"">
        <tr>
        <td class=""label"">姓名</td>
        <td class=""field"">{{{{ content.realName }}}}</td>
        <td class=""label"">工作单位</td>
        <td class=""field"">{{{{ content.organization }}}}</td>
        </tr>
        <tr>
        <td class=""label"">证件名称</td>
        <td class=""field"">{{{{ content.cardType }}}}</td>
        <td class=""label"">证件号码</td>
        <td class=""field"">{{{{ content.cardNo }}}}</td>
        </tr>
        <tr>
        <td class=""label"">联系电话</td>
        <td class=""field"">{{{{ content.phone }}}}</td>
        <td class=""label"">邮政编码</td>
        <td class=""field"">{{{{ content.postCode }}}}</td>
        </tr>
        <tr>
        <td class=""label"">联系地址</td>
        <td colspan=""3"" class=""field"">{{{{ content.address }}}}</td>
        </tr>
        <tr>
        <td class=""label"">电子邮件</td>
        <td class=""field"">{{{{ content.email }}}}</td>
        <td class=""label"">传真</td>
        <td class=""field"">{{{{ content.fax }}}}</td>
        </tr>
        <tr>
        <td class=""label"">标题</td>
        <td colspan=""3"" class=""field"">{{{{ content.title }}}}</td>
        </tr>
        <tr>
        <td class=""label"">内容</td>
        <td colspan=""3"" class=""field"">{{{{ content.content }}}}</td>
        </tr>
        <tr>
        <td class=""label"">处理状态</td>
        <td class=""field"" colspan=""3"">{{{{ getState() }}}}</td>
        </tr>
        <tr v-if=""isReply()"">
        <td class=""label"">办理部门</td>
        <td class=""field"" colspan=""3"">{{{{ content.replyDepartmentName }}}}</td>
        </tr>
        <tr v-if=""isReply()"">
        <td class=""label"">办理人员</td>
        <td class=""field"" colspan=""3"">{{{{ content.replyUserName }}}}</td>
        </tr>
        <tr v-if=""isReply()"">
        <td class=""label"">办理日期</td>
        <td class=""field"" colspan=""3"">{{{{ content.replyAddDate }}}}</td>
        </tr>
        <tr v-if=""isReply()"">
        <td class=""label"">回复内容</td>
        <td height=""150"" class=""field"" colspan=""3"">{{{{ content.replyContent }}}}</td>
        </tr>
    </table>
    </div>
</template>

<template v-if=""elType === 'failure'"">
    <div class=""failure"">
    提交失败，请确认姓名及查询号是否正确。
    </div>
</template>

<template v-if=""elType === 'form' || elType === 'failure'"">

    <table border=""0"" cellpadding=""0"" cellspacing=""0"">
    <tbody>
        <tr>
        <td>姓名：</td>
        <td class=""field"">
            <input v-model=""content.realName"" v-bind:class=""{{ error: isSubmit && !content.realName }}"" type=""text"">
        </td>
        </tr>
        <tr>
        <td>查询号：</td>
        <td class=""field"">
            <input v-model=""content.queryCode"" v-bind:class=""{{ error: isSubmit && !content.queryCode }}"" type=""text"">
        </td>
        </tr>
    </tbody>
    </table>
    <div class=""buttons"">
    <a href=""javascript:;"" class=""button"" v-on:click=""submit()"">查 询</a>
    </div>

</template>
</div>

<script type=""text/javascript"">
var govInteractQuery = new Vue({{
    el: '#govInteractQuery',
    data: {{
    elType: 'form',
    isSubmit: false,
    content: {{}}
    }},
    methods: {{
    getState: function () {{
        if (this.content.state == 'Denied') {{
        return ""拒绝受理"";
        }}
        if (this.content.state == 'Checked') {{
        return ""办理完毕"";
        }}
        return ""办理中"";
    }},
    isReply: function() {{
        return this.content.state == 'Checked';
    }},
    submit: function () {{
        var $this = this;

        this.isSubmit = true;
        var content = this.content;

        if (!content.realName ||
        !content.queryCode) {{
        return;
        }}

        axios.get('/api/v1/contents/{SiteId}/{channelId}', {{
            params: {{
            realName: content.realName,
            queryCode: content.queryCode
            }},
            headers: {{
            'X-SS-API-KEY': '{apiKey}'
            }}
        }})
        .then(function (response) {{
            console.log(response.data.value);
            if (response.data.value && response.data.value.length > 0) {{
            $this.content = response.data.value[0];
            $this.elType = 'success';
            }} else {{
            $this.elType = 'failure';
            }}
        }})
        .catch(function (error) {{
            $this.elType = 'failure';
        }});

    }}
    }}
}})
</script>";
                }
            } 
        }
    }
}