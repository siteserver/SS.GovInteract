using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class ContentDao
    {
        public const string TableName = "ss_govinteract_content";

        public static List<TableColumn> Columns => new List<TableColumn>
        {  
            new TableColumn
            {
                AttributeName = ContentAttribute.State,
                DataType = DataType.VarChar,
                DataLength = 10,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Radio,
                    DisplayName = "状态",
                    IsRequired = true,
                    ListItems = new List<ListItem>
                    {
                        new ListItem
                        {
                            Text= "保持不变",
                            Value = "-100",
                            Selected = true
                        },
                        new ListItem
                        {
                            Text= "草稿",
                            Value = "-99",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "待审核",
                            Value = "0",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "终审通过",
                            Value = "1",
                            Selected = false
                        }
                    }
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.IpAddress,
                DataType = DataType.VarChar,
                DataLength = 50,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "IP地址",
                }
            }, 
            new TableColumn
            {
                AttributeName = ContentAttribute.RealName,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "姓名"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Organization,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "工作单位"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.CardType,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.SelectOne,
                    DisplayName = "证件名称",
                    IsRequired = true,
                    ListItems = new List<ListItem>
                    {
                        new ListItem
                        {
                            Text= "身份证",
                            Value = "身份证",
                            Selected = true
                        },
                        new ListItem
                        {
                            Text= "学生证",
                            Value = "学生证",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "军官证",
                            Value = "军官证",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "工作证",
                            Value = "工作证",
                            Selected = false
                        }
                    }
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.CardNo,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "证件号码"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Phone,
                DataType = DataType.VarChar,
                DataLength = 50,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "联系电话"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.PostCode,
                DataType = DataType.VarChar,
                DataLength = 50,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "邮政编码"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Address,
                DataType = DataType.VarChar,
                DataLength = 500,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "联系地址"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Email,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName =  "电子邮件"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Fax,
                DataType = DataType.VarChar,
                DataLength = 50,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName ="传真"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.TypeId,
                DataType = DataType.Integer,

                InputStyle = new InputStyle
                {
                    InputType = InputType.SelectOne,
                    DisplayName =  "类型",
                    IsRequired = true,
                    ListItems = new List<ListItem>
                    {
                        new ListItem
                        {
                            Text= "求决",
                            Value = "15",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "举报",
                            Value = "16",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "投诉",
                            Value = "17",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "咨询",
                            Value = "18",
                            Selected = true
                        },
                        new ListItem
                        {
                            Text= "建议",
                            Value = "19",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "感谢",
                            Value = "20",
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "其他",
                            Value = "21",
                            Selected = false
                        }
                    }
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.IsPublic,
                DataType = DataType.VarChar,
                DataLength = 10,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Radio,
                     DisplayName = "是否公开",
                    IsRequired = true,
                    ListItems = new List<ListItem>
                    {
                        new ListItem
                        {
                            Text= "公开",
                            Value = true.ToString(),
                            Selected = true
                        },
                        new ListItem
                        {
                            Text= "不公开",
                            Value = false.ToString(),
                            Selected = false
                        }
                    }
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Content,
                DataType = DataType.Text,
                InputStyle = new InputStyle
                {
                    InputType = InputType.TextEditor,
                    DisplayName = "内容"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.FileUrl,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.File,
                    DisplayName = "附件",
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.DepartmentId,
                DataType = DataType.Integer,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Customize,
                    DisplayName = "提交对象",
                }
            }
        };

        public string ConnectionString { get; }
        public IDataApi Helper { get; }

        public ContentDao()
        {
            ConnectionString = Main.Instance.ConnectionString;
            Helper = Main.Instance.DataApi;
        }

        public static Dictionary<string, Func<int, int, IAttributes, string>> ContentFormCustomized
            => new Dictionary<string, Func<int, int, IAttributes, string>>
            {
                {
                    ContentAttribute.DepartmentId, GetDepartmentIdHtml
                }
            };

        public static string GetDepartmentIdHtml(int siteId, int channelId, IAttributes attributes)
        {
            var departmentId = attributes.GetString(nameof(ContentAttribute.DepartmentId));

            return $@"
<div class=""form-group"">
    <label class=""col-sm-1 control-label"">提交对象</label>
    <div class=""col-sm-6"">
        {GetDepartmentsHtml(siteId, channelId, attributes)}
    </div>
    <div class=""col-sm-5"">
    </div>
</div> 
";
        }

        public static string GetDepartmentsHtml(int siteId, int channelId, IAttributes attributes)
        {
            var pairList = new List<KeyValuePair<string, DropDownList>>();

            var ddlDepartmentId = new DropDownList
            {
                ID = "DepartmentID",
                CssClass = "form-control"
            };
              
            var departmentInfoList = Main.DepartmentDao.GetDepartmentInfoList();
            var isLastNodeArray = new bool[departmentInfoList.Count];
            foreach (var departmentInfo in departmentInfoList)
            {
                var listItem = new ListItem(Utils.GetSelectOptionText(departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode, isLastNodeArray),
                    departmentInfo.DepartmentId.ToString());
                ddlDepartmentId.Items.Add(listItem);
            }
            Utils.SelectSingleItem(ddlDepartmentId, attributes.GetString(nameof(ContentAttribute.DepartmentId)));
            pairList.Add(new KeyValuePair<string, DropDownList>("提交对象", ddlDepartmentId)); 

            var builder = new StringBuilder();
            builder.Append(@"<div class=""row"">");
            var count = 0;
            foreach (var keyValuePair in pairList)
            {
                if (count > 1)
                {
                    builder.Append(@"</div><div class=""row m-t-10"">");
                    count = 0;
                }
                builder.Append($@"
<div class=""col-xs-2 control-label"">{keyValuePair.Key}</div>
<div class=""col-xs-4"">
    {Utils.GetControlRenderHtml(keyValuePair.Value)}
</div>
");
                count++;
            }
            builder.Append("</div>"); 

            return builder.ToString();
        }


        public static void ContentFormSubmited(int siteId, int channelId, IContentInfo contentInfo, IAttributes form)
        {
            var departmentId = form.GetInt("DepartmentID"); 
            if (departmentId == 0)
            {
                throw new Exception("请选择正确的提交对象");
            } 
            contentInfo.Set(nameof(ContentAttribute.DepartmentId), departmentId.ToString()); 
        }
    }
}
