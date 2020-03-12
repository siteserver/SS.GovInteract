using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers
{
    public class ApplicationController : ApiController
    {
        private const string Route = "";
        private const string RouteQuery = "actions/query";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");

                var settings = ApplicationUtils.GetSettings(siteId);
                var dataInfo = new DataInfo();
                var departmentInfoList = DepartmentManager.GetDepartmentInfoList(siteId);

                return Ok(new
                {
                    Value = dataInfo,
                    DepartmentInfoList = departmentInfoList,
                    Settings = settings
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Apply()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");

                var authCode = request.GetPostString("authCode");
                var code = CookieUtils.GetCookie(CaptchaController.CookieName);
                if (string.IsNullOrEmpty(code) || CacheUtils.Exists($"{CaptchaController.CookieName}.{code}"))
                {
                    return BadRequest("验证码已超时，请点击刷新验证码！");
                }
                CookieUtils.Erase(CaptchaController.CookieName);
                CacheUtils.InsertMinutes($"{CaptchaController.CookieName}.{code}", true, 10);
                if (!StringUtils.EqualsIgnoreCase(code, authCode))
                {
                    return BadRequest("验证码不正确，请重新输入！");
                }

                var categoryId = request.GetPostInt("categoryId");
                var departmentId = request.GetPostInt("departmentId");
                var categoryInfo = CategoryManager.GetCategoryInfo(siteId, categoryId);
                var departmentInfo = DepartmentManager.GetDepartmentInfo(siteId, departmentId);

                var dataInfo = new DataInfo
                {
                    Id = 0,
                    SiteId = siteId,
                    AddDate = DateTime.Now,
                    QueryCode = StringUtils.GetShortGuid(true),
                    CategoryId = categoryInfo?.Id ?? 0,
                    DepartmentId = departmentInfo?.Id ?? 0,
                    IsCompleted = false,
                    State = DataState.New.Value,
                    DenyReason = string.Empty,
                    RedoComment = string.Empty,
                    ReplyContent = string.Empty,
                    IsReplyFiles = false,
                    ReplyDate = DateTime.Now,
                    Name = request.GetPostString("name"),
                    Gender = request.GetPostString("gender"),
                    Phone = request.GetPostString("phone"),
                    Email = request.GetPostString("email"),
                    Address = request.GetPostString("address"),
                    Zip = request.GetPostString("zip"),
                    Title = request.GetPostString("title"),
                    Content = request.GetPostString("content"),
                    CategoryName = categoryInfo == null ? string.Empty : categoryInfo.CategoryName,
                    DepartmentName = departmentInfo == null ? string.Empty : departmentInfo.DepartmentName
                };

                Main.DataRepository.Insert(dataInfo);

                return Ok(new
                {
                    Value = dataInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(RouteQuery)]
        public IHttpActionResult Query()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");

                var name = request.GetPostString("name");
                var queryCode = request.GetPostString("queryCode");

                var dataInfo = Main.DataRepository.Query(siteId, name, queryCode);
                if (dataInfo == null) return NotFound();

                IList<FileInfo> fileInfoList = new List<FileInfo>();
                if (dataInfo.IsReplyFiles)
                {
                    fileInfoList = Main.FileRepository.GetFileInfoList(siteId, dataInfo.Id);
                }

                return Ok(new
                {
                    Value = dataInfo,
                    FileInfoList = fileInfoList
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
