using System.Collections.Generic;
using SS.GovInteract.Core.Provider;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Core
{
    public static class DataCountManager
    {
        private static class DataCountManagerCache
        {
            private static readonly object LockObject = new object();

            private static string GetCacheKey(int siteId)
            {
                return $"SS.GovInteract.Core.DataCountManager.{siteId}";
            }

            public static Dictionary<string, int> GetDataCountsCache(int siteId)
            {
                var cacheKey = GetCacheKey(siteId);
                var retval = CacheUtils.Get<Dictionary<string, int>>(cacheKey);
                if (retval != null) return retval;

                lock (LockObject)
                {
                    retval = CacheUtils.Get<Dictionary<string, int>>(cacheKey);
                    if (retval == null)
                    {
                        retval = Main.DataRepository.GetDataCounts(siteId);

                        CacheUtils.InsertHours(cacheKey, retval, 1);
                    }
                }

                return retval;
            }

            public static void Clear(int siteId)
            {
                var cacheKey = GetCacheKey(siteId);
                CacheUtils.Remove(cacheKey);
            }
        }

        public static int GetCount(int siteId, List<DataState> stateList)
        {
            if (stateList.Count == 0)
            {
                return GetTotalCount(siteId);
            }
            var count = 0;
            foreach (var dataState in stateList)
            {
                count += GetCount(siteId, dataState);
            }
            return count;
        }

        public static int GetCount(int siteId, DataState state)
        {
            var entries = DataCountManagerCache.GetDataCountsCache(siteId);
            return entries[state.Value];
        }

        public static int GetTotalCount(int siteId)
        {
            var total = 0;
            var entries = DataCountManagerCache.GetDataCountsCache(siteId);
            foreach (var state in DataState.AllStates)
            {
                total += entries[state.Value];
            }
            return total;
        }

        public static void ClearCache(int siteId)
        {
            DataCountManagerCache.Clear(siteId);
        }
    }
}
