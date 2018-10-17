using System.Collections;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public static class AdministratorDao
    {
        public static ArrayList GetUserNameArrayList(int departmentId, bool isAll)
        {
            var arraylist = new ArrayList();
            string sqlSelect = $"SELECT UserName FROM siteserver_Administrator WHERE Id = {departmentId}";
            if (isAll)
            {
                var departmentIdList = DepartmentDao.GetDepartmentIdListForDescendant(departmentId);
                departmentIdList.Add(departmentId);
                sqlSelect =
                    $"SELECT UserName FROM siteserver_Administrator WHERE Id IN ({Utils.ObjectCollectionToString(departmentIdList)})";
            }

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlSelect))
            {
                while (rdr.Read())
                {
                    arraylist.Add(Context.DatabaseApi.GetString(rdr, 0));
                }
                rdr.Close();
            }
            return arraylist;
        }

        public static AdministratorInfo GetByUserName(string userName)
        {
            AdministratorInfo info = null;

            string sqlString = "SELECT UserName, DisplayName, Id FROM siteserver_Administrator WHERE UserName = @UserName";
            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter("UserName", userName)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    info = GetAdministratorInfo(rdr);
                }
                rdr.Close();
            }  

            return info;
        }

        private static AdministratorInfo GetAdministratorInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new AdministratorInfo
            {
                UserName = Context.DatabaseApi.GetString(rdr, i++),
                DisplayName = Context.DatabaseApi.GetString(rdr, i++),
                DepartmentId = Context.DatabaseApi.GetInt(rdr, i) 
            };
        }
    }
}
