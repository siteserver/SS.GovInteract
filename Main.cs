using System;
using System.Collections.Generic;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;
using SS.GovInteract.Pages;
using SS.GovInteract.Provider;
using System.Collections.Specialized;

namespace SS.GovInteract
{
    public class Main : PluginBase
    {
        public static AdministratorDao AdministratorDao { get; private set; }
        public static ChannelDao ChannelDao { get; private set; }
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
            DepartmentDao = new DepartmentDao();
            LogDao = new LogDao();
            PermissionsDao = new PermissionsDao();
            RemarkDao = new RemarkDao();
            ReplyDao = new ReplyDao();
            TypeDao = new TypeDao();

            service.AddContentModel(ContentDao.TableName, ContentDao.Columns)  // 插件对应的内容模型表
                   .AddDatabaseTable(ChannelDao.TableName, ChannelDao.Columns) // 插件需要用到的其他表结构
                   .AddDatabaseTable(LogDao.TableName, LogDao.Columns)
                   .AddDatabaseTable(PermissionsDao.TableName, PermissionsDao.Columns)
                   .AddDatabaseTable(RemarkDao.TableName, RemarkDao.Columns)
                   .AddDatabaseTable(ReplyDao.TableName, ReplyDao.Columns)
                   .AddDatabaseTable(TypeDao.TableName, TypeDao.Columns)
                   .AddSiteMenu(MenuManager.SiteMenu);   // 插件站点菜单  

            service.ContentFormSubmit += Service_ContentFormSubmited; // 页面提交处理函数
            service.ContentFormLoad += Service_ContentFormLoad; // 页面加载处理函数

        }

        private string Service_ContentFormLoad(object sender, ContentFormLoadEventArgs e)
        {
            if (e.AttributeName == ContentAttribute.DepartmentId)
            {
                var departmentId = e.Form.GetString(nameof(ContentAttribute.DepartmentId));

                return $@"
                    <div class=""form-group"">
                        <label class=""col-sm-1 control-label"">提交对象</label>
                        <div class=""col-sm-6"">
                            {ContentDao.GetDepartmentsHtml(e.SiteId, e.ChannelId, e.Form)}
                        </div>
                        <div class=""col-sm-5"">
                        </div>
                    </div> 
                    ";
            }
            else
            {
                return string.Empty;
            }
        }

        private void Service_ContentFormSubmited(object sender, ContentFormSubmitEventArgs e)
        {
            ContentDao.ContentFormSubmited(e.SiteId, e.ChannelId, e.ContentInfo, e.Form);
        } 
    }
}