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
                AttributeName = ContentAttribute.RealName,
                DataType = DataType.VarChar,
                DataLength = 255,
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
                DataLength = 255,
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
                DataLength = 255,
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
                DataLength = 255,
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
                DataLength = 255,
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
                DataLength = 255,
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
                DataLength = 18,
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
                DataLength = 255,
                InputStyle = new InputStyle
                {
                    InputType = InputType.File,
                    DisplayName = "附件"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.DepartmentId,
                DataType = DataType.Integer,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Customize,
                    DisplayName = "提交部门"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.DepartmentName,
                DataType = DataType.VarChar,
                DataLength = 255,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Hidden,
                    DisplayName = "提交部门"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.QueryCode,
                DataType = DataType.VarChar,
                DataLength = 255,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Hidden,
                    DisplayName = "查询码"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.State,
                DataType = DataType.VarChar,
                DataLength = 50,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Hidden,
                    DisplayName = "状态"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.IpAddress,
                DataType = DataType.VarChar,
                DataLength = 50,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Hidden,
                    DisplayName = "IP地址"
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
        
//        public static string GetDepartmentsHtml(int siteId, int channelId, IAttributes attributes)
//        {
//            var pairList = new List<KeyValuePair<string, DropDownList>>();

//            var ddlDepartmentId = new DropDownList
//            {
//                ID = "DepartmentID",
//                CssClass = "form-control"
//            };
              
//            var departmentInfoList = Main.DepartmentDao.GetDepartmentInfoList();
//            var isLastNodeArray = new bool[departmentInfoList.Count];
//            foreach (var departmentInfo in departmentInfoList)
//            {
//                var listItem = new ListItem(Utils.GetSelectOptionText(departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode, isLastNodeArray),
//                    departmentInfo.Id.ToString());
//                ddlDepartmentId.Items.Add(listItem);
//            }
//            Utils.SelectSingleItem(ddlDepartmentId, attributes.GetString(nameof(ContentAttribute.DepartmentId)));
//            pairList.Add(new KeyValuePair<string, DropDownList>("提交部门", ddlDepartmentId));

//            var builder = new StringBuilder();
//            builder.Append(@"<div class=""row"">");
//            var count = 0;
//            foreach (var keyValuePair in pairList)
//            {
//                if (count > 1)
//                {
//                    builder.Append(@"</div><div class=""row m-t-10"">");
//                    count = 0;
//                }
//                builder.Append($@"
//<div class=""col-xs-2 control-label"">{keyValuePair.Key}</div>
//<div class=""col-xs-4"">
//    {Utils.GetControlRenderHtml(keyValuePair.Value)}
//</div>
//");
//                count++;
//            }
//            builder.Append("</div>"); 

//            return builder.ToString();
//        }

        //public static void ContentFormSubmited(int siteId, int channelId, IContentInfo contentInfo, IAttributes form)
        //{
        //    var departmentId = form.GetInt("DepartmentID"); 
        //    if (departmentId == 0)
        //    {
        //        throw new Exception("请选择正确的提交对象");
        //    } 
        //    contentInfo.Set(nameof(ContentAttribute.DepartmentId), departmentId.ToString()); 
        //}

        public void UpdateState(int siteId, int channelId, int contentId, EState state)
        {
            var contentInfo = Main.Instance.ContentApi.GetContentInfo(siteId, channelId, contentId);

            contentInfo.Set(ContentAttribute.State, EStateUtils.GetValue(state));
            contentInfo.CheckedLevel = 0;

            contentInfo.IsChecked = state == EState.Checked;
            Main.Instance.ContentApi.Update(siteId, channelId, contentInfo);
        }

        public void UpdateDepartmentId(int siteId, int channelId, int contentId, int departmentId)
        {
            var contentInfo = Main.Instance.ContentApi.GetContentInfo(siteId, channelId, contentId);
            contentInfo.Set(ContentAttribute.DepartmentId, departmentId.ToString());
            Main.Instance.ContentApi.Update(siteId, channelId, contentInfo);
        }

        public IContentInfo GetContentInfo(int siteId, int channelId, string queryCode)
        {
            var list = Main.Instance.ContentApi.GetContentInfoList(siteId, channelId, $"{ContentAttribute.QueryCode} = '{queryCode}'", string.Empty, 0, 0);
            if (list != null && list.Count > 0) return list[0];
            return null;
        }

        public string GetSelectStringByState(int siteId, int channelId, params EState[] states)
        {
            var builder = new StringBuilder($"SELECT * FROM {TableName} WHERE {nameof(IContentInfo.SiteId)} = {siteId} AND {nameof(IContentInfo.ChannelId)} = {channelId} AND (");
            
            foreach (var state in states)
            {
                builder.Append($" {ContentAttribute.State} = '{EStateUtils.GetValue(state)}' OR");
                if (state == EState.New)
                {
                    builder.Append($" {ContentAttribute.State} = '{SqlUtils.EmptyString}' OR");
                }
            }
            builder.Length -= 2;
            builder.Append(")");

            return builder.ToString();
        }

        public string GetSelectString(int siteId, int channelId)
        {
            return $"SELECT * FROM {TableName} WHERE {nameof(IContentInfo.SiteId)} = {siteId} AND {nameof(IContentInfo.ChannelId)} = {channelId}";
        }

        public string GetSelectString(int siteId, int channelId, string state, string dateFrom, string dateTo, string keyword)
        {
            var builder = new StringBuilder($"SELECT * FROM {TableName} WHERE {nameof(IContentInfo.SiteId)} = {siteId} AND {nameof(IContentInfo.ChannelId)} = {channelId}");

            if (!string.IsNullOrEmpty(state))
            {
                builder.Append($" AND ({ContentAttribute.State} = '{state}')");
            }
            if (!string.IsNullOrEmpty(dateFrom))
            {
                builder.Append($" AND ({nameof(IContentInfo.AddDate)} >= {Helper.ToDateSqlString(Utils.ToDateTime(dateFrom))})");
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                builder.Append($" AND ({nameof(IContentInfo.AddDate)} <= {Helper.ToDateSqlString(Utils.ToDateTime(dateTo))})");
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                var filterKeyword = Utils.FilterSql(keyword);
                builder.Append($" AND (Title LIKE '{filterKeyword}' OR Content LIKE '{filterKeyword}')");
            }

            return builder.ToString();
        }

        public int GetCountByDepartmentId(int siteId, int departmentId, DateTime begin, DateTime end)
        {
            string sqlString =
                $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND DepartmentId = {departmentId} AND (AddDate BETWEEN {Helper.ToDateSqlString(begin)} AND {Helper.ToDateSqlString(end.AddDays(1))})";
            return Dao.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentId(int siteId, int departmentId)
        {
            string sqlString =
                $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND DepartmentId = {departmentId}";
            return Dao.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentId(int siteId, int departmentId, int channelId, DateTime begin, DateTime end)
        {
            var sqlString = channelId == 0
                ? $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND DepartmentId = {departmentId} AND (AddDate BETWEEN {Helper.ToDateSqlString(begin)} AND {Helper.ToDateSqlString(end.AddDays(1))})"
                : $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND ChannelId = {channelId} AND DepartmentId = {departmentId} AND (AddDate BETWEEN {Helper.ToDateSqlString(begin)} AND {Helper.ToDateSqlString(end.AddDays(1))})";
            return Dao.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIdAndState(int siteId, int departmentId, EState state)
        {
            string sqlString =
                $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND DepartmentId = {departmentId} AND State = '{EStateUtils.GetValue(state)}'";
            return Dao.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIdAndState(int siteId, int departmentId, EState state, DateTime begin, DateTime end)
        {
            string sqlString =
                $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND DepartmentId = {departmentId} AND State = '{EStateUtils.GetValue(state)}' AND (AddDate BETWEEN {Helper.ToDateSqlString(begin)} AND {Helper.ToDateSqlString(end.AddDays(1))})";
            return Dao.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIdAndState(int siteId, int departmentId, int channelId, EState state, DateTime begin, DateTime end)
        {
            var sqlString = channelId == 0
                ? $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND DepartmentId = {departmentId} AND State = '{EStateUtils.GetValue(state)}' AND (AddDate BETWEEN {Helper.ToDateSqlString(begin)} AND {Helper.ToDateSqlString(end.AddDays(1))})"
                : $"SELECT COUNT(*) AS TotalNum FROM {TableName} WHERE SiteId = {siteId} AND DepartmentId = {departmentId} AND ChannelId = {channelId} AND State = '{EStateUtils.GetValue(state)}' AND (AddDate BETWEEN {Helper.ToDateSqlString(begin)} AND {Helper.ToDateSqlString(end.AddDays(1))})";
            return Dao.GetIntResult(sqlString);
        }
    }
}
