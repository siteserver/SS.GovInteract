using System.Collections.Generic;
using System.Linq;
using Datory;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;

namespace SS.GovInteract.Core.Provider
{
    public class CategoryRepository
    {
        private readonly Repository<CategoryInfo> _repository;
        public CategoryRepository()
        {
            _repository = new Repository<CategoryInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private static class Attr
        {
            public const string SiteId = nameof(CategoryInfo.SiteId);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public int Insert(CategoryInfo departmentInfo)
        {
            var departmentId = _repository.Insert(departmentInfo);

            CategoryManager.ClearCache(departmentInfo.SiteId);

            return departmentId;
        }

        public void Update(CategoryInfo departmentInfo)
        {
            _repository.Update(departmentInfo);

            CategoryManager.ClearCache(departmentInfo.SiteId);
        }

        public void Delete(int siteId, int departmentId)
        {
            _repository.Delete(departmentId);

            CategoryManager.ClearCache(siteId);
        }

        public List<CategoryInfo> GetCategoryInfoList(int siteId)
        {
            var departmentInfoList = _repository.GetAll(Q.Where(Attr.SiteId, siteId));
            

            return departmentInfoList.OrderBy(departmentInfo => departmentInfo.Taxis == 0 ? int.MaxValue : departmentInfo.Taxis).ToList();
        }
    }
}
