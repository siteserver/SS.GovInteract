using System.Collections.Generic;
using Datory;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;

namespace SS.GovInteract.Core.Provider
{
    public class LogRepository
    {
        private readonly Repository<LogInfo> _repository;
        public LogRepository()
        {
            _repository = new Repository<LogInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private static class Attr
        {
            public const string Id = nameof(LogInfo.Id);
            public const string SiteId = nameof(LogInfo.SiteId);
            public const string DataId = nameof(LogInfo.DataId);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public void Insert(LogInfo logInfo)
        {
            _repository.Insert(logInfo);
        }

        public void Update(LogInfo logInfo)
        {
            _repository.Update(logInfo);
        }

        public void DeleteByDataId(int dataId)
        {
            _repository.Delete(Q.Where(Attr.DataId, dataId));
        }

        public IList<LogInfo> GetLogInfoList(int siteId, int dataId)
        {
            return _repository.GetAll(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.DataId, dataId)
                .OrderBy(Attr.Id)
            );
        }
    }
}
