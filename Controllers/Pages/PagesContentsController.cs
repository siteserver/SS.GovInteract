using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/contents")]
    public class PagesContentsController : ApiController
    {
        private const string Route = "";
        //private const string RouteActionsExport = "actions/export";
        //private const string RouteActionsVisible = "actions/visible";

        [HttpGet, Route(Route)]
        public IHttpActionResult List()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var pageType = request.GetQueryString("pageType");
                var stateList = new List<DataState>();
                if (StringUtils.EqualsIgnoreCase(pageType, ApplicationUtils.PageTypeAccept))
                {
                    stateList.Add(DataState.New);
                }
                else if (StringUtils.EqualsIgnoreCase(pageType, ApplicationUtils.PageTypeReply))
                {
                    stateList.Add(DataState.Accepted);
                    stateList.Add(DataState.Redo);
                }
                else if (StringUtils.EqualsIgnoreCase(pageType, ApplicationUtils.PageTypeCheck))
                {
                    stateList.Add(DataState.Replied);
                }
                else
                {
                    var state = request.GetQueryString("state");
                    if (!string.IsNullOrEmpty(state))
                    {
                        stateList.Add(DataState.ToDataState(state));
                    }
                }
                
                var page = request.GetQueryInt("page", 1);
                var isSearch = request.GetQueryBool("isSearch");
                var keyword = request.GetQueryString("keyword");
                var departmentId = request.GetQueryInt("departmentId");

                int totalCount;
                var isSiteAdmin = request.AdminPermissions.IsSiteAdmin(siteId);
                var userDepartmentIdList = new List<int>();

                if (isSiteAdmin)
                {
                    if (isSearch)
                    {
                        totalCount = Main.DataRepository.GetCount(siteId, stateList, keyword, departmentId);
                    }
                    else
                    {
                        keyword = string.Empty;
                        departmentId = 0;
                        totalCount = DataCountManager.GetCount(siteId, stateList);
                    }
                }
                else
                {
                    userDepartmentIdList = DepartmentManager.GetDepartmentIdList(siteId, request.AdminName);
                    totalCount = Main.DataRepository.GetUserCount(siteId, stateList, keyword, departmentId, userDepartmentIdList);
                }

                var pages = Convert.ToInt32(Math.Ceiling((double)totalCount / ApplicationUtils.PageSize));
                if (pages == 0) pages = 1;
                if (page > pages) page = pages;

                IList<DataInfo> dataInfoList;
                if (totalCount == 0)
                {
                    dataInfoList = new List<DataInfo>();
                }
                else
                {
                    if (isSiteAdmin)
                    {
                        if (totalCount <= ApplicationUtils.PageSize)
                        {
                            dataInfoList = Main.DataRepository.GetDataInfoList(siteId, stateList, keyword, departmentId, 0, totalCount);
                        }
                        else
                        {
                            if (page == 0) page = 1;
                            var offset = (page - 1) * ApplicationUtils.PageSize;
                            var limit = totalCount - offset > ApplicationUtils.PageSize
                                ? ApplicationUtils.PageSize
                                : totalCount - offset;
                            dataInfoList = Main.DataRepository.GetDataInfoList(siteId, stateList, keyword, departmentId, offset, limit);
                        }
                    }
                    else
                    {
                        if (page == 0) page = 1;
                        var offset = (page - 1) * ApplicationUtils.PageSize;
                        var limit = totalCount - offset > ApplicationUtils.PageSize
                            ? ApplicationUtils.PageSize
                            : totalCount - offset;
                        dataInfoList = Main.DataRepository.GetUserDataInfoList(siteId, stateList, keyword, departmentId, userDepartmentIdList, offset, limit);
                    }
                }

                List<DepartmentInfo> departmentInfoList = null;
                Dictionary<string, int> countDict = null;
                Settings settings = null;

                if (page == 1)
                {
                    if (isSiteAdmin)
                    {
                        departmentInfoList = DepartmentManager.GetDepartmentInfoList(siteId);
                    }
                    else
                    {
                        departmentInfoList = new List<DepartmentInfo>();
                        foreach (var userDepartmentId in userDepartmentIdList)
                        {
                            departmentInfoList.Add(DepartmentManager.GetDepartmentInfo(siteId, userDepartmentId));
                        }
                    }

                    countDict = new Dictionary<string, int>
                    {
                        {"All", DataCountManager.GetTotalCount(siteId)}
                    };
                    foreach (var state in DataState.AllStates)
                    {
                        countDict.Add(state.Value, DataCountManager.GetCount(siteId, state));
                    }

                    settings = ApplicationUtils.GetSettings(siteId);
                }

                return Ok(new
                {
                    Value = dataInfoList,
                    Count = totalCount,
                    Pages = pages,
                    Page = page,
                    DepartmentInfoList = departmentInfoList,
                    CountDict = countDict,
                    Settings = settings
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[HttpPost, Route(RouteActionsExport)]
        //public IHttpActionResult Export()
        //{
        //    try
        //    {
        //        var request = Context.AuthenticatedRequest;
        //        var siteId = request.GetPostInt("siteId");
        //        if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

        //        var state = request.GetPostString("state");
        //        var count = DataCountManager.GetCount(siteId, state);
        //        var logs = DataDao.GetDataInfoList(siteId, state, string.Empty, 0, 0, count);

        //        var head = new List<string> { "序号" };
        //        head.Add("添加时间");

        //        var rows = new List<List<string>>();

        //        var index = 1;
        //        foreach (var log in logs)
        //        {
        //            var row = new List<string>
        //            {
        //                index++.ToString()
        //            };
        //            row.Add(log.AddDate.ToString("yyyy-MM-dd HH:mm"));

        //            rows.Add(row);
        //        }

        //        var relatedPath = "表单数据.csv";

        //        CsvUtils.Export(Context.PluginApi.GetPluginPath(ApplicationUtils.PluginId, relatedPath), head, rows);
        //        var downloadUrl = Context.PluginApi.GetPluginUrl(ApplicationUtils.PluginId, relatedPath);

        //        return Ok(new
        //        {
        //            Value = downloadUrl
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        //[HttpPost, Route(RouteActionsVisible)]
        //public IHttpActionResult Visible()
        //{
        //    try
        //    {
        //        var request = Context.AuthenticatedRequest;
        //        var siteId = request.GetPostInt("siteId");
        //        if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

        //        var attributeName = request.GetPostString("attributeName");

        //        return Ok(new
        //        {
        //            Value = attributeName
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
    }
}
