using System.Collections.Generic; 
using SiteServer.Plugin;
using SS.GovInteract.Model; 

namespace SS.GovInteract.Core
{
    public class InteractManager
    {
        //public static void Initialize(int siteId)
        //{
        //    var channelInfoList = GetInteractChannelInfoList(siteId);

        //    if (channelInfoList.Count == 0)
        //    {
        //        var channelInfo = Main.ChannelApi.NewInstance(siteId);
        //        channelInfo.ParentId = siteId;
        //        channelInfo.ContentModelPluginId = Main.Id;
        //        channelInfo.ChannelName = "互动交流";
        //        Main.ChannelApi.Insert(siteId, channelInfo);
        //    }
        //}

        public static List<IChannelInfo> GetInteractChannelInfoList(int siteId)
        {
            var channelInfoList = new List<IChannelInfo>();

            var channelIdList = Main.ChannelApi.GetChannelIdList(siteId);
            foreach (var channelId in channelIdList)
            {
                var channelInfo = Main.ChannelApi.GetChannelInfo(siteId, channelId);
                if (channelInfo.ContentModelPluginId == Main.PluginId)
                {
                    channelInfoList.Add(channelInfo);
                }
            }

            return channelInfoList;
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

        public static List<int> GetDepartmentIdList(ChannelInfo channelInfo)
        {
            return Utils.StringCollectionToIntList(channelInfo?.DepartmentIdCollection);
            //var list = new List<int>();
            //if (string.IsNullOrEmpty(channelInfo?.DepartmentIdCollection))
            //{
            //    return list;
            //}
            //return Main.DepartmentDao.GetDepartmentIdListByDepartmentIdCollection(channelInfo.DepartmentIdCollection);
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
