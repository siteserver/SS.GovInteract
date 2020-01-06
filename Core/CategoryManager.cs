using System.Collections.Generic;
using System.Linq;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Core
{
    public static class CategoryManager
    {
        private static class CategoryManagerCache
        {
            private static readonly object LockObject = new object();
            private static string GetCacheKey(int siteId)
            {
                return $"SS.GovInteract.Core.CategoryManager.{siteId}";
            }

            public static List<CategoryInfo> GetCategoryInfoListByCache(int siteId)
            {
                var cacheKey = GetCacheKey(siteId);
                var departmentInfoList = CacheUtils.Get<List<CategoryInfo>>(cacheKey);
                if (departmentInfoList != null) return departmentInfoList;

                lock (LockObject)
                {
                    departmentInfoList = CacheUtils.Get<List<CategoryInfo>>(cacheKey);
                    if (departmentInfoList == null)
                    {
                        departmentInfoList = Main.CategoryRepository.GetCategoryInfoList(siteId);

                        CacheUtils.InsertHours(cacheKey, departmentInfoList, 12);
                    }
                }

                return departmentInfoList;
            }

            public static void Clear(int siteId)
            {
                var cacheKey = GetCacheKey(siteId);
                CacheUtils.Remove(cacheKey);
            }
        }

        public static List<CategoryInfo> GetCategoryInfoList(int siteId)
        {
            return CategoryManagerCache.GetCategoryInfoListByCache(siteId);
        }

        public static List<int> GetCategoryIdList(int siteId, string userName)
        {
            var departmentIdList = new List<int>();
            var departmentInfoList = CategoryManagerCache.GetCategoryInfoListByCache(siteId);
            foreach (var departmentInfo in departmentInfoList)
            {
                if (StringUtils.In(departmentInfo.UserNames, userName))
                {
                    departmentIdList.Add(departmentInfo.Id);
                }
            }

            return departmentIdList;
        }

        public static CategoryInfo GetCategoryInfo(int siteId, int departmentId)
        {
            var entries = CategoryManagerCache.GetCategoryInfoListByCache(siteId);

            return entries.FirstOrDefault(x => x != null && x.Id == departmentId);
        }

        public static string GetCategoryName(int siteId, int departmentId)
        {
            var departmentInfo = GetCategoryInfo(siteId, departmentId);
            return departmentInfo?.CategoryName;
        }

        public static void ClearCache(int siteId)
        {
            CategoryManagerCache.Clear(siteId);
        }
    }
}
