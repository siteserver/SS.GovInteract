using System.Collections.Generic;
using System.Data;
using Datory;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;

namespace SS.GovInteract.Core.Provider
{
    public class FileRepository
    {
        private readonly Repository<FileInfo> _repository;
        public FileRepository()
        {
            _repository = new Repository<FileInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private static class Attr
        {
            public const string Id = nameof(FileInfo.Id);
            public const string SiteId = nameof(FileInfo.SiteId);
            public const string DataId = nameof(FileInfo.DataId);
            public const string FileName = nameof(FileInfo.FileName);
            public const string FileUrl = nameof(FileInfo.FileUrl);
            public const string Length = nameof(FileInfo.Length);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public void Insert(FileInfo fileInfo)
        {
            _repository.Insert(fileInfo);
        }

        public void DeleteByDataId(int dataId)
        {
            _repository.Delete(Q.Where(Attr.DataId, dataId));
        }

        public void Delete(int fileId)
        {
            _repository.Delete(fileId);
        }

        public IList<FileInfo> GetFileInfoList(int siteId, int dataId)
        {
            return _repository.GetAll(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.DataId, dataId)
                .OrderBy(Attr.Id)
            );
        }
    }
}
