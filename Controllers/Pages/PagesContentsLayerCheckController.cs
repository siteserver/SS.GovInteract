﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Provider;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers.Pages
{
    [RoutePrefix("pages/contentsLayerCheck")]
    public class PagesContentsLayerCheckController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetConfig()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var contentIdList = TranslateUtils.StringCollectionToIntList(request.GetQueryString("contentIds"));

                var dataInfoList = new List<DataInfo>();
                foreach (var contentId in contentIdList)
                {
                    var contentInfo = Main.DataRepository.GetDataInfo(contentId);
                    if (contentInfo == null || contentInfo.State != DataState.Replied.Value) continue;
                    dataInfoList.Add(contentInfo);
                }

                var departmentInfoList = DepartmentManager.GetDepartmentInfoList(siteId);

                return Ok(new
                {
                    Value = dataInfoList,
                    DepartmentInfoList = departmentInfoList
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin) return Unauthorized();

                var contentIdList = TranslateUtils.StringCollectionToIntList(request.GetPostString("contentIds"));

                foreach (var contentId in contentIdList)
                {
                    Main.DataRepository.UpdateState(siteId, contentId, DataState.Checked);

                    LogManager.Check(siteId, contentId, request.AdminId);
                }

                return Ok(new
                {
                    Value = contentIdList
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
