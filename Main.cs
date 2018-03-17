using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;
using SS.GovInteract.Pages;
using SS.GovInteract.Provider;
using Menu = SiteServer.Plugin.Menu;

namespace SS.GovInteract
{
    public class Main : PluginBase
    {
        public static AdministratorDao AdministratorDao { get; private set; }
        public static ChannelDao ChannelDao { get; private set; }
        public static ContentDao ContentDao { get; private set; }
        public static DepartmentDao DepartmentDao { get; private set; }
        public static LogDao LogDao { get; private set; }
        public static PermissionsDao PermissionsDao { get; private set; }
        public static RemarkDao RemarkDao { get; private set; }
        public static ReplyDao ReplyDao { get; private set; }
        public static TypeDao TypeDao { get; private set; }

        private static readonly Dictionary<int, ConfigInfo> ConfigInfoDict = new Dictionary<int, ConfigInfo>();

        // 插件部分简单的设置，在不新建表的前提下存放在系统特定的位置
        public ConfigInfo GetConfigInfo(int siteId)
        {
            if (!ConfigInfoDict.ContainsKey(siteId))
            {
                ConfigInfoDict[siteId] = ConfigApi.GetConfig<ConfigInfo>(siteId) ?? new ConfigInfo();
            }
            return ConfigInfoDict[siteId];
        }

        public static Main Instance { get; private set; }

        // 插件被激活时初始化工作
        public override void Startup(IService service)
        {
            Instance = this;

            AdministratorDao = new AdministratorDao();
            ChannelDao = new ChannelDao();
            ContentDao = new ContentDao();
            DepartmentDao = new DepartmentDao();
            LogDao = new LogDao();
            PermissionsDao = new PermissionsDao();
            RemarkDao = new RemarkDao();
            ReplyDao = new ReplyDao();
            TypeDao = new TypeDao();

            service.AddContentModel(ContentDao.TableName, ContentDao.Columns) // 插件对应的内容模型表
                .AddDatabaseTable(ChannelDao.TableName, ChannelDao.Columns) // 插件需要用到的其他表结构
                .AddDatabaseTable(LogDao.TableName, LogDao.Columns)
                .AddDatabaseTable(PermissionsDao.TableName, PermissionsDao.Columns)
                .AddDatabaseTable(RemarkDao.TableName, RemarkDao.Columns)
                .AddDatabaseTable(ReplyDao.TableName, ReplyDao.Columns)
                .AddDatabaseTable(TypeDao.TableName, TypeDao.Columns)
                .AddSiteMenu(siteId => new Menu
                {
                    Text = "互动交流",
                    IconClass = "ion-chatbox-working",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text = "待受理办件",
                            Href = PageMain.GetRedirectUrl(siteId, PageListAccept.GetRedirectUrl(siteId, 0))
                        },
                        new Menu
                        {
                            Text = "待办理办件",
                            Href = PageMain.GetRedirectUrl(siteId, PageListReply.GetRedirectUrl(siteId, 0))
                        },
                        new Menu
                        {
                            Text = "待审核办件",
                            Href = PageMain.GetRedirectUrl(siteId, PageListCheck.GetRedirectUrl(siteId, 0))
                        },
                        new Menu
                        {
                            Text = "所有办件",
                            Href = PageMain.GetRedirectUrl(siteId, PageListAll.GetRedirectUrl(siteId, 0))
                        },
                        new Menu
                        {
                            Text = "新增办件",
                            Href = PageMain.GetRedirectUrl(siteId, FilesApi.GetAdminDirectoryUrl(
                                    $"cms/pageContentAdd.aspx?siteId={siteId}"))
                        },
                        new Menu
                        {
                            Text = "互动交流设置",
                            Href = PageInit.GetRedirectUrl(siteId, PageCommonConfiguration.GetRedirectUrl(siteId))
                        },
                        new Menu
                        {
                            Text = "数据统计分析",
                            Href = PageInit.GetRedirectUrl(siteId, PageAnalysis.GetRedirectUrl(siteId))
                        }
                    }
                });   // 插件站点菜单  

            service.ContentFormSubmit += Service_ContentFormSubmited; // 页面提交处理函数
            service.ContentFormLoad += Service_ContentFormLoad; // 页面加载处理函数

        }

        private string Service_ContentFormLoad(object sender, ContentFormLoadEventArgs e)
        {
            if (e.AttributeName == ContentAttribute.DepartmentId)
            {
                var departmentId = e.Form.GetString(nameof(ContentAttribute.DepartmentId));

                var ddlDepartmentId = new DropDownList
                {
                    ID = ContentAttribute.DepartmentId,
                    CssClass = "form-control"
                };

                var departmentInfoList = DepartmentDao.GetDepartmentInfoList();

                foreach (var departmentInfo in departmentInfoList)
                {
                    var listItem = new ListItem(departmentInfo.DepartmentName, departmentInfo.Id.ToString());
                    ddlDepartmentId.Items.Add(listItem);
                }
                Utils.SelectSingleItem(ddlDepartmentId, departmentId);

                return $@"
<div class=""form-group form-row"">
    <label class=""col-sm-1 col-form-label text-right"">提交部门</label>
    <div class=""col-sm-6"">
        {Utils.GetControlRenderHtml(ddlDepartmentId)}
    </div>
    <div class=""col-sm-5"">

    </div>
</div>";
            }

            return string.Empty;
        }

        private void Service_ContentFormSubmited(object sender, ContentFormSubmitEventArgs e)
        {
            var departmentId = e.Form.GetInt(ContentAttribute.DepartmentId);
            if (departmentId == 0)
            {
                throw new Exception("请选择正确的提交部门");
            }
            e.ContentInfo.Set(nameof(ContentAttribute.DepartmentId), departmentId.ToString());
        }
    }
}