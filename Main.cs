using System.Collections.Generic;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Parser;
using SS.GovInteract.Core.Provider;
using SS.GovInteract.Core.Utils;
using Menu = SiteServer.Plugin.Menu;

namespace SS.GovInteract
{
    public class Main : PluginBase
    {
        public static DataRepository DataRepository { get; private set; }
        public static CategoryRepository CategoryRepository { get; private set; }
        public static DepartmentRepository DepartmentRepository { get; private set; }
        public static FileRepository FileRepository { get; private set; }
        public static LogRepository LogRepository { get; private set; }

        public override void Startup(IService service)
        {
            DataRepository = new DataRepository();
            CategoryRepository = new CategoryRepository();
            DepartmentRepository = new DepartmentRepository();
            FileRepository = new FileRepository();
            LogRepository = new LogRepository();

            service
                .AddSiteMenu(siteId =>
                {
                    //var request = Context.AuthenticatedRequest;
                    //if (!request.AdminPermissions.IsSiteAdmin(siteId))
                    //{
                    //    var userDepartmentIdList = DepartmentManager.GetDepartmentIdList(siteId, request.AdminName);
                    //    if (userDepartmentIdList.Count == 0) return null;

                    //    var acceptCount = DataRepository.GetUserCount(siteId, new List<DataState>
                    //    {
                    //        DataState.New
                    //    }, string.Empty, 0, userDepartmentIdList);
                    //    var replyCount = DataRepository.GetUserCount(siteId, new List<DataState>
                    //    {
                    //        DataState.Accepted,
                    //        DataState.Redo
                    //    }, string.Empty, 0, userDepartmentIdList);

                    //    return new Menu
                    //    {
                    //        Text = "互动交流",
                    //        IconClass = "fa fa-envelope",
                    //        Menus = new List<Menu>
                    //        {
                    //            new Menu
                    //            {
                    //                Text = $"待受理信件 ({acceptCount})",
                    //                Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeAccept}"
                    //            },
                    //            new Menu
                    //            {
                    //                Text = $"待办理信件 ({replyCount})",
                    //                Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeReply}"
                    //            },
                    //            new Menu
                    //            {
                    //                Text = "新增信件",
                    //                Href = "pages/add.html"
                    //            }
                    //        }
                    //    };
                    //}
                    //else
                    //{
                        var acceptCount = DataCountManager.GetCount(siteId, new List<DataState>
                        {
                            DataState.New
                        });
                        var replyCount = DataCountManager.GetCount(siteId, new List<DataState>
                        {
                            DataState.Accepted,
                            DataState.Redo
                        });
                        var checkCount = DataCountManager.GetCount(siteId, new List<DataState>
                        {
                            DataState.Replied
                        });
                        var totalCount = DataCountManager.GetTotalCount(siteId);

                        return new Menu
                        {
                            Text = "互动交流",
                            IconClass = "fa fa-envelope",
                            Menus = new List<Menu>
                            {
                                new Menu
                                {
                                    Id = ApplicationUtils.PageTypeAccept,
                                    Text = $"待受理信件 ({acceptCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeAccept}"
                                },
                                new Menu
                                {
                                    Id = ApplicationUtils.PageTypeReply,
                                    Text = $"待办理信件 ({replyCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeReply}"
                                },
                                new Menu
                                {
                                    Id = ApplicationUtils.PageTypeCheck,
                                    Text = $"待审核信件 ({checkCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeCheck}"
                                },
                                new Menu
                                {
                                    Id = "all",
                                    Text = $"所有信件 ({totalCount})",
                                    Href = "pages/contents.html"
                                },
                                new Menu
                                {
                                    Id = "add",
                                    Text = "新增信件",
                                    Href = "pages/add.html"
                                },
                                new Menu
                                {
                                    Id = "categories",
                                    Text = "分类设置",
                                    Href = "pages/categories.html"
                                },
                                new Menu
                                {
                                    Id = "departments",
                                    Text = "部门设置",
                                    Href = "pages/departments.html"
                                },
                                new Menu
                                {
                                    Id = "settings",
                                    Text = "互动交流设置",
                                    Href = "pages/settings.html"
                                },
                                new Menu
                                {
                                    Id = "templates",
                                    Text = "互动交流模板",
                                    Href = "pages/templates.html"
                                }
                            }
                        };
                    //}
                })
                .AddDatabaseTable(DataRepository.TableName, DataRepository.TableColumns)
                .AddDatabaseTable(CategoryRepository.TableName, CategoryRepository.TableColumns)
                .AddDatabaseTable(DepartmentRepository.TableName, DepartmentRepository.TableColumns)
                .AddDatabaseTable(LogRepository.TableName, LogRepository.TableColumns)
                .AddDatabaseTable(FileRepository.TableName, FileRepository.TableColumns)
                .AddStlElementParser(StlGovInteract.ElementName, StlGovInteract.Parse)
                ;
        }
    }
}