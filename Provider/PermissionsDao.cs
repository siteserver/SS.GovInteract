using System.Collections;
using System.Collections.Generic;
using System.Data; 
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class PermissionsDao
    {
        public const string TableName = "ss_govinteract_permissions"; 

        public static List<TableColumn> Columns => new List<TableColumn>
        { 
            new TableColumn
            {
                AttributeName = nameof(PermissionsInfo.UserName),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(PermissionsInfo.ChannelId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(PermissionsInfo.Permissions),
                DataType = DataType.Text 
            }  
        };

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public PermissionsDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void Insert(int siteId, PermissionsInfo permissionsInfo)
        { 
            string sqlString = $@"INSERT INTO {TableName}
            (
                {nameof(PermissionsInfo.UserName)}, 
                {nameof(PermissionsInfo.ChannelId)}, 
                {nameof(PermissionsInfo.Permissions)} 
            ) VALUES (
                @{nameof(PermissionsInfo.UserName)}, 
                @{nameof(PermissionsInfo.ChannelId)}, 
                @{nameof(PermissionsInfo.Permissions)} 
            )";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(PermissionsInfo.UserName), permissionsInfo.UserName),
                _helper.GetParameter(nameof(PermissionsInfo.ChannelId), permissionsInfo.ChannelId),
                _helper.GetParameter(nameof(PermissionsInfo.Permissions), permissionsInfo.Permissions)
            };

            if (!Main.ChannelDao.IsExists(permissionsInfo.ChannelId))
            {
                var channelInfo = new ChannelInfo(permissionsInfo.ChannelId, siteId, 0, 0, string.Empty, string.Empty);
                Main.ChannelDao.Insert(channelInfo);
            }

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Delete(string userName, int channelId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(PermissionsInfo.UserName)} = @{nameof(PermissionsInfo.UserName)} AND {nameof(PermissionsInfo.ChannelId)} = @{nameof(PermissionsInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(PermissionsInfo.UserName), userName),
                _helper.GetParameter(nameof(PermissionsInfo.ChannelId), channelId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Update(PermissionsInfo permissionsInfo)
        {
            string sqlString = $"UPDATE {TableName} SET {nameof(PermissionsInfo.Permissions)} = @{nameof(PermissionsInfo.Permissions)} WHERE {nameof(PermissionsInfo.UserName)} = @{nameof(PermissionsInfo.UserName)} AND {nameof(PermissionsInfo.ChannelId)} = @{nameof(PermissionsInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(PermissionsInfo.UserName), permissionsInfo.UserName),
                _helper.GetParameter(nameof(PermissionsInfo.ChannelId), permissionsInfo.ChannelId),
                _helper.GetParameter(nameof(PermissionsInfo.Permissions), permissionsInfo.Permissions)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public PermissionsInfo GetPermissionsInfo(string userName, int channelId)
        {
            PermissionsInfo permissionsInfo = null;

            string sqlString = $"SELECT {nameof(PermissionsInfo.UserName)}, {nameof(PermissionsInfo.ChannelId)}, {nameof(PermissionsInfo.Permissions)} FROM {TableName} WHERE {nameof(PermissionsInfo.UserName)} = @{nameof(PermissionsInfo.UserName)} AND {nameof(PermissionsInfo.ChannelId)} = @{nameof(PermissionsInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(PermissionsInfo.UserName), userName),
                _helper.GetParameter(nameof(PermissionsInfo.ChannelId), channelId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    permissionsInfo = GetPermissionsInfo(rdr);
                }
                rdr.Close();
            } 

            return permissionsInfo;
        }

        public ArrayList GetPermissionsInfoArrayList(string userName)
        {
            var arraylist = new ArrayList();

            var parameters = new[]
           {
                _helper.GetParameter(nameof(PermissionsInfo.UserName), userName) 
           };

            string sqlString = $"SELECT {nameof(PermissionsInfo.UserName)}, {nameof(PermissionsInfo.ChannelId)}, {nameof(PermissionsInfo.Permissions)} FROM {TableName} WHERE {nameof(PermissionsInfo.UserName)} = @{nameof(PermissionsInfo.UserName)}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    PermissionsInfo permissionsInfo = GetPermissionsInfo(rdr);
                    arraylist.Add(permissionsInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public Dictionary<int, List<string>> GetPermissionSortedList(string userName)
        {
            var sortedlist = new Dictionary<int, List<string>>();

            var permissionsInfoArrayList = GetPermissionsInfoArrayList(userName);

            foreach (PermissionsInfo permissionsInfo in permissionsInfoArrayList)
            {
                var list = new List<string>();
                if (sortedlist[permissionsInfo.ChannelId] != null)
                {
                    list = sortedlist[permissionsInfo.ChannelId];
                }

                var permissionArrayList = Utils.StringCollectionToStringList(permissionsInfo.Permissions);
                foreach (string permission in permissionArrayList)
                {
                    if (!list.Contains(permission)) list.Add(permission);
                }
                sortedlist[permissionsInfo.ChannelId] = list;
            }

            return sortedlist;
        }

        private PermissionsInfo GetPermissionsInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new PermissionsInfo
            {
                UserName = _helper.GetString(rdr, i++), 
                ChannelId = _helper.GetInt(rdr, i++),
                Permissions = _helper.GetString(rdr, i++) 
            };
        }
    }
}
