using System;
using System.Collections.Generic; 

using SiteServer.Plugin;
using SS.GovInteract.Model; 

namespace SS.GovInteract.Core
{
    public class InteractManager
    {
        public static void Initialize(int siteId)
        {
            var configInfo = Main.Instance.GetConfigInfo(siteId);
            if (configInfo.GovInteractChannelId > 0)
            { 
                var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(siteId, configInfo.GovInteractChannelId);
                if (nodeInfo == null || nodeInfo.ContentModelPluginId != Main.Instance.Id)
                {
                    configInfo.GovInteractChannelId = 0;
                } 
            }  

            if (configInfo.GovInteractChannelId == 0)
            {
                var nodeInfo = Main.Instance.ChannelApi.NewInstance(siteId);
                nodeInfo.ContentModelPluginId = Main.Instance.Id;
                nodeInfo.ChannelName = "互动交流";
                configInfo.GovInteractChannelId = Main.Instance.ChannelApi.Insert(siteId, nodeInfo);
                Main.Instance.ConfigApi.SetConfig(siteId, configInfo); 
            }
        }

        public static List<IChannelInfo> GetNodeInfoList(int siteId)
        {
            var nodeInfoList = new List<IChannelInfo>();
            var configInfo = Main.Instance.GetConfigInfo(siteId);
            if (configInfo.GovInteractChannelId > 0)
            {
                var channelIdList = Main.Instance.ChannelApi.GetChannelIdList(siteId);
                foreach (var channelId in channelIdList)
                {
                    var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(siteId, channelId);
                    if (nodeInfo.ContentModelPluginId == Main.Instance.Id)
                    {
                        nodeInfoList.Add(nodeInfo);
                    }
                }
            }
            return nodeInfoList;
        } 

        public static void AddDefaultTypeInfos(int siteId, int channelId)
        {
            var typeInfo = new TypeInfo(0, "求决", channelId, siteId, 0);
            Main.TypeDao.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "举报", channelId, siteId, 0);
            Main.TypeDao.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "投诉", channelId, siteId, 0);
            Main.TypeDao.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "咨询", channelId, siteId, 0);
            Main.TypeDao.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "建议", channelId, siteId, 0);
            Main.TypeDao.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "感谢", channelId, siteId, 0);
            Main.TypeDao.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "其他", channelId, siteId, 0);
            Main.TypeDao.Insert(typeInfo);
        }

        public static List<int> GetFirstDepartmentIdList(ChannelInfo channelInfo)
        {
            var list = new List<int>();
            if (channelInfo == null || string.IsNullOrEmpty(channelInfo.DepartmentIdCollection))
            {
                return list;
            }
            return Main.DepartmentDao.GetDepartmentIdListByDepartmentIdCollection(channelInfo.DepartmentIdCollection);
            //return string.IsNullOrEmpty(channelInfo?.DepartmentIdCollection) ? Main.DepartmentDao.GetDepartmentIdListByParentId(0) : Main.DepartmentDao.GetDepartmentIdListByDepartmentIdCollection(channelInfo.DepartmentIdCollection);
        }

        public static string GetTypeName(int typeId)
        {
            return typeId > 0 ? Main.TypeDao.GetTypeName(typeId) : string.Empty;
        }

        public static bool IsPermission(int siteId, int channelId, string permission)
        {
            //待实现
            //List<string> govInteractPermissionList = null;
            //if (ProductPermissionsManager.Current.GovInteractPermissionDict.ContainsKey(siteId))
            //{
            //    govInteractPermissionList = ProductPermissionsManager.Current.GovInteractPermissionDict[siteId];
            //}
            //if (govInteractPermissionList != null)
            //{
            //    return govInteractPermissionList.Contains(permission);
            //}
            //return false;
            return true;
        }
    }
}
