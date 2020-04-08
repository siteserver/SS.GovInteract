using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/view")]
    public class PagesViewController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var dataId = request.GetQueryInt("dataId");
                
                var dataInfo = Main.DataRepository.GetDataInfo(dataId);
                
                IList<FileInfo> fileInfoList = new List<FileInfo>();
                if (dataInfo.IsReplyFiles)
                {
                    fileInfoList = Main.FileRepository.GetFileInfoList(siteId, dataId);
                }

                var logInfoList = Main.LogRepository.GetLogInfoList(siteId, dataId);
                var settings = ApplicationUtils.GetSettings(siteId);

                return Ok(new
                {
                    Value = dataInfo,
                    LogInfoList = logInfoList,
                    FileInfoList = fileInfoList,
                    Settings = settings
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
