using System.Collections;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class AdministratorDao
    {  
        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public AdministratorDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        } 

        public ArrayList GetUserNameArrayList(int departmentId, bool isAll)
        {
            var arraylist = new ArrayList();
            string sqlSelect = $"SELECT UserName FROM siteserver_Administrator WHERE Id = {departmentId}";
            if (isAll)
            {
                var departmentIdList = Main.DepartmentDao.GetDepartmentIdListForDescendant(departmentId);
                departmentIdList.Add(departmentId);
                sqlSelect =
                    $"SELECT UserName FROM siteserver_Administrator WHERE Id IN ({Utils.ObjectCollectionToString(departmentIdList)})";
            }

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlSelect))
            {
                while (rdr.Read())
                {
                    arraylist.Add(_helper.GetString(rdr, 0));
                }
                rdr.Close();
            }
            return arraylist;
        }

        public AdministratorInfo GetByUserName(string userName)
        {
            AdministratorInfo info = null;

            string sqlString = "SELECT UserName, DisplayName, Id FROM siteserver_Administrator WHERE UserName = @UserName";
            var parameters = new[]
            {
                _helper.GetParameter("UserName", userName)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    info = GetAdministratorInfo(rdr);
                }
                rdr.Close();
            }  

            return info;
        }

        private AdministratorInfo GetAdministratorInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new AdministratorInfo
            {
                UserName = _helper.GetString(rdr, i++),
                DisplayName = _helper.GetString(rdr, i++),
                DepartmentId = _helper.GetInt(rdr, i) 
            };
        }
    }
}
