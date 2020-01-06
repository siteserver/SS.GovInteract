using System;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;

namespace SS.GovInteract.Core
{
    public static class LogManager
    {
        public static void NewApplication(DataInfo dataInfo)
        {
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = dataInfo.SiteId,
                DataId = dataInfo.Id,
                UserId = 0,
                AddDate = DateTime.Now,
                Summary =
                    $"{(dataInfo.Name)}提交信件"
            });
        }

        public static void Translate(int siteId, int dataId, int userId, int departmentId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}转办信件至{DepartmentManager.GetDepartmentName(siteId, departmentId)}"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）转办信件至{DepartmentManager.GetDepartmentName(siteId, departmentId)}";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Accept(int siteId, int dataId, int userId, int departmentId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}受理信件"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）受理信件";
            if (departmentId > 0)
            {
                summary += $"，转办至 {DepartmentManager.GetDepartmentName(siteId, departmentId)}";
            }
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Deny(int siteId, int dataId, int userId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}拒绝受理信件"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）拒绝受理信件";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Check(int siteId, int dataId, int userId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}审核通过"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）审核通过";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Redo(int siteId, int dataId, int userId, string instruction)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}要求返工：{instruction}"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）要求返工：{instruction}";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Reply(int siteId, int dataId, int userId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}办理信件"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）办理信件";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }
    }
}
