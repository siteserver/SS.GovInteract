using System.Collections;
using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class RemarkDao
    {
        public const string TableName = "ss_govinteract_remark";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.ChannelId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.ContentId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.RemarkType),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.Remark),
                DataType = DataType.VarChar,
                DataLength = 255
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.DepartmentId), 
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.UserName),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(RemarkInfo.AddDate),
                DataType = DataType.DateTime
            }
        };

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public RemarkDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void Insert(RemarkInfo remarkInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
            (
                {nameof(RemarkInfo.SiteId)}, 
                {nameof(RemarkInfo.ChannelId)}, 
                {nameof(RemarkInfo.ContentId)}, 
                {nameof(RemarkInfo.RemarkType)},
                {nameof(RemarkInfo.Remark)},
                {nameof(RemarkInfo.DepartmentId)},
                {nameof(RemarkInfo.UserName)},
                {nameof(RemarkInfo.AddDate)}
            ) VALUES (
                @{nameof(RemarkInfo.SiteId)}, 
                @{nameof(RemarkInfo.ChannelId)}, 
                @{nameof(RemarkInfo.ContentId)}, 
                @{nameof(RemarkInfo.RemarkType)},
                @{nameof(RemarkInfo.Remark)},
                @{nameof(RemarkInfo.DepartmentId)},
                @{nameof(RemarkInfo.UserName)},
                @{nameof(RemarkInfo.AddDate)}
            )";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(RemarkInfo.SiteId), remarkInfo.SiteId),
                _helper.GetParameter(nameof(RemarkInfo.ChannelId), remarkInfo.ChannelId),
                _helper.GetParameter(nameof(RemarkInfo.ContentId), remarkInfo.ContentId),
                _helper.GetParameter(nameof(RemarkInfo.RemarkType), remarkInfo.RemarkType),
                _helper.GetParameter(nameof(RemarkInfo.Remark), remarkInfo.Remark),
                _helper.GetParameter(nameof(RemarkInfo.DepartmentId), remarkInfo.DepartmentId),
                _helper.GetParameter(nameof(RemarkInfo.UserName), remarkInfo.UserName),
                _helper.GetParameter(nameof(RemarkInfo.AddDate), remarkInfo.AddDate)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Delete(int remarkId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(RemarkInfo.Id)} = {remarkId}";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        } 

        public RemarkInfo GetRemarkInfo(int remarkId)
        {
            RemarkInfo remarkInfo = null;

            var sqlString = $@"SELECT {nameof(RemarkInfo.Id)}, 
                    {nameof(RemarkInfo.SiteId)}, 
                    {nameof(RemarkInfo.ChannelId)}, 
                    {nameof(RemarkInfo.ContentId)}, 
                    {nameof(RemarkInfo.RemarkType)},
                    {nameof(RemarkInfo.Remark)},
                    {nameof(RemarkInfo.DepartmentId)},
                    {nameof(RemarkInfo.UserName)},
                    {nameof(RemarkInfo.AddDate)}
                    FROM {TableName} WHERE {nameof(RemarkInfo.Id)} = @{nameof(RemarkInfo.Id)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(RemarkInfo.Id), remarkId),
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    remarkInfo = GetRemarkInfo(rdr);
                }
                rdr.Close();
            }

            return remarkInfo;
        }

        public RemarkInfo GetRemarkInfoByContentId(int siteId, int contentId)
        {
            RemarkInfo remarkInfo = null;

            var sqlString = $@"SELECT {nameof(RemarkInfo.Id)}, 
                    {nameof(RemarkInfo.SiteId)}, 
                    {nameof(RemarkInfo.ChannelId)}, 
                    {nameof(RemarkInfo.ContentId)}, 
                    {nameof(RemarkInfo.RemarkType)},
                    {nameof(RemarkInfo.Remark)},
                    {nameof(RemarkInfo.DepartmentId)},
                    {nameof(RemarkInfo.UserName)},
                    {nameof(RemarkInfo.AddDate)}
                    FROM {TableName} WHERE {nameof(RemarkInfo.SiteId)} = @{nameof(RemarkInfo.SiteId)} AND {nameof(RemarkInfo.ContentId)} = @{nameof(RemarkInfo.ContentId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(RemarkInfo.SiteId), siteId),
                _helper.GetParameter(nameof(RemarkInfo.ContentId), contentId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    remarkInfo = GetRemarkInfo(rdr);
                }
                rdr.Close();
            }

            return remarkInfo;
        }

        public ArrayList GetRemarkInfoArrayList(int siteId, int contentId)
        {
            var arraylist = new ArrayList();

            var sqlString = $@"SELECT {nameof(RemarkInfo.Id)}, 
                    {nameof(RemarkInfo.SiteId)}, 
                    {nameof(RemarkInfo.ChannelId)}, 
                    {nameof(RemarkInfo.ContentId)}, 
                    {nameof(RemarkInfo.RemarkType)},
                    {nameof(RemarkInfo.Remark)},
                    {nameof(RemarkInfo.DepartmentId)},
                    {nameof(RemarkInfo.UserName)},
                    {nameof(RemarkInfo.AddDate)}
                    FROM {TableName} WHERE {nameof(RemarkInfo.SiteId)} = @{nameof(RemarkInfo.SiteId)} AND {nameof(RemarkInfo.ContentId)} = @{nameof(RemarkInfo.ContentId)}";


            var parameters = new[]
            {
                _helper.GetParameter(nameof(RemarkInfo.SiteId), siteId),
                _helper.GetParameter(nameof(RemarkInfo.ContentId), contentId) 
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                while (rdr.Read())
                { 
                    var remarkInfo = GetRemarkInfo(rdr);
                    arraylist.Add(remarkInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public IEnumerable GetDataSourceByContentId(int siteId, int contentId)
        {
            var sqlString = $@"SELECT {nameof(RemarkInfo.Id)}, 
                    {nameof(RemarkInfo.SiteId)}, 
                    {nameof(RemarkInfo.ChannelId)}, 
                    {nameof(RemarkInfo.ContentId)}, 
                    {nameof(RemarkInfo.RemarkType)},
                    {nameof(RemarkInfo.Remark)},
                    {nameof(RemarkInfo.DepartmentId)},
                    {nameof(RemarkInfo.UserName)},
                    {nameof(RemarkInfo.AddDate)}
                    FROM {TableName} WHERE {nameof(RemarkInfo.SiteId)} = {siteId} AND {nameof(RemarkInfo.ContentId)} = {contentId}";
             
            var enumerable = (IEnumerable)_helper.ExecuteReader(_connectionString,sqlString);
            return enumerable;
        }

        private RemarkInfo GetRemarkInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new RemarkInfo
            {
                Id = _helper.GetInt(rdr, i++),
                SiteId = _helper.GetInt(rdr, i++),
                ChannelId = _helper.GetInt(rdr, i++),
                ContentId = _helper.GetInt(rdr, i++),
                RemarkType = _helper.GetString(rdr, i++),
                Remark = _helper.GetString(rdr, i++),
                DepartmentId = _helper.GetInt(rdr, i++),
                UserName = _helper.GetString(rdr, i++),
                AddDate = _helper.GetDateTime(rdr, i)
            };
        }
    }
}
