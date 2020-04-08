using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/contentsLayerReply")]
    public class PagesContentsLayerReplyController : ApiController
    {
        private const string Route = "{contentId:int}";
        private const string RouteUpload = "actions/upload";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetConfig(int contentId)
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var contentInfo = Main.DataRepository.GetDataInfo(contentId);
                if (contentInfo == null || contentInfo.State != DataState.Accepted.Value &&
                    contentInfo.State != DataState.Redo.Value &&
                    contentInfo.State != DataState.Replied.Value) return NotFound();

                IList<FileInfo> fileInfoList = new List<FileInfo>();
                if (contentInfo.IsReplyFiles)
                {
                    fileInfoList = Main.FileRepository.GetFileInfoList(siteId, contentId);
                }

                return Ok(new
                {
                    Value = contentInfo,
                    FileInfoList = fileInfoList
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(RouteUpload)]
        public IHttpActionResult Upload()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var httpRequest = HttpContext.Current.Request;
                var fileName = httpRequest["fileName"];
                var fileCount = httpRequest.Files.Count;
                if (fileCount == 0) return NotFound();

                var file = httpRequest.Files[0];

                if (string.IsNullOrEmpty(fileName)) fileName = System.IO.Path.GetFileName(file.FileName);

                var filePath = Context.SiteApi.GetUploadFilePath(siteId, fileName);
                file.SaveAs(filePath);

                var fileInfo = new FileInfo
                {
                    FileName = fileName,
                    FileUrl = Context.SiteApi.GetSiteUrlByFilePath(filePath),
                    Length = file.ContentLength
                };

                return Ok(new
                {
                    Value = true,
                    FileInfo = fileInfo
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Value = false,
                    ex.Message
                });
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit(int contentId)
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var replyContent = request.GetPostString("replyContent");
                var fileInfoList = request.GetPostObject<List<FileInfo>>("fileInfoList");

                var fileInfoListDatabase = Main.FileRepository.GetFileInfoList(siteId, contentId);

                Main.DataRepository.UpdateStateAndReply(siteId, contentId, replyContent, fileInfoList.Count > 0);

                LogManager.Reply(siteId, contentId, request.AdminId);

                foreach (var fileInfo in fileInfoList)
                {
                    if (fileInfo.Id != 0) continue;
                    fileInfo.SiteId = siteId;
                    fileInfo.DataId = contentId;
                    Main.FileRepository.Insert(fileInfo);
                }

                foreach (var fileInfoDatabase in fileInfoListDatabase)
                {
                    if (fileInfoList.Exists(f => f.Id == fileInfoDatabase.Id)) continue;

                    var filePath = Context.SiteApi.GetUploadFilePath(siteId, fileInfoDatabase.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    Main.FileRepository.Delete(fileInfoDatabase.Id);
                }

                return Ok(new
                {
                    Value = true
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
