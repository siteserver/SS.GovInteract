using System.Collections;
using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public static class RemarkDao
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

        public static void Insert(RemarkInfo remarkInfo)
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
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.SiteId), remarkInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.ChannelId), remarkInfo.ChannelId),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.ContentId), remarkInfo.ContentId),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.RemarkType), remarkInfo.RemarkType),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.Remark), remarkInfo.Remark),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.DepartmentId), remarkInfo.DepartmentId),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.UserName), remarkInfo.UserName),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.AddDate), remarkInfo.AddDate)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static void Delete(int remarkId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(RemarkInfo.Id)} = {remarkId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        } 

        public static RemarkInfo GetRemarkInfo(int remarkId)
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
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.Id), remarkId),
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    remarkInfo = GetRemarkInfo(rdr);
                }
                rdr.Close();
            }

            return remarkInfo;
        }

        public static RemarkInfo GetRemarkInfoByContentId(int siteId, int contentId)
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
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.ContentId), contentId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    remarkInfo = GetRemarkInfo(rdr);
                }
                rdr.Close();
            }

            return remarkInfo;
        }

        public static ArrayList GetRemarkInfoArrayList(int siteId, int contentId)
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
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(RemarkInfo.ContentId), contentId) 
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
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

        public static IEnumerable GetDataSourceByContentId(int siteId, int contentId)
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
             
            var enumerable = (IEnumerable)Context.DatabaseApi.ExecuteReader(Context.ConnectionString,sqlString);
            return enumerable;
        }

        private static RemarkInfo GetRemarkInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new RemarkInfo
            {
                Id = Context.DatabaseApi.GetInt(rdr, i++),
                SiteId = Context.DatabaseApi.GetInt(rdr, i++),
                ChannelId = Context.DatabaseApi.GetInt(rdr, i++),
                ContentId = Context.DatabaseApi.GetInt(rdr, i++),
                RemarkType = Context.DatabaseApi.GetString(rdr, i++),
                Remark = Context.DatabaseApi.GetString(rdr, i++),
                DepartmentId = Context.DatabaseApi.GetInt(rdr, i++),
                UserName = Context.DatabaseApi.GetString(rdr, i++),
                AddDate = Context.DatabaseApi.GetDateTime(rdr, i)
            };
        }
    }
}
