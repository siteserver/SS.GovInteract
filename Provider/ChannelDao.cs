using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public static class ChannelDao
    {
        public const string TableName = "ss_govinteract_channel";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(ChannelInfo.Id),
                DataType = DataType.Integer,
                IsPrimaryKey = true,
                IsIdentity = true
            },
            new TableColumn
            {
                AttributeName = nameof(ChannelInfo.ChannelId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ChannelInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ChannelInfo.ApplyStyleId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ChannelInfo.QueryStyleId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ChannelInfo.DepartmentIdCollection),
                DataType = DataType.VarChar,
                DataLength = 255
            },
            new TableColumn
            {
                AttributeName = nameof(ChannelInfo.Summary),
                DataType = DataType.VarChar,
                DataLength = 255
            }
        };

        public static void Insert(ChannelInfo channelInfo)
        {
            //channelInfo.ApplyStyleID = DataProvider.TagStyleDao.Insert(new TagStyleInfo(0, channelInfo.ChannelId.ToString(), StlGovInteractApply.ElementName, channelInfo.SiteId, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
            //channelInfo.QueryStyleID = DataProvider.TagStyleDao.Insert(new TagStyleInfo(0, channelInfo.ChannelId.ToString(), StlGovInteractQuery.ElementName, channelInfo.SiteId, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
            
            string sqlString = $@"INSERT INTO {TableName}
            (
                {nameof(ChannelInfo.ChannelId)}, 
                {nameof(ChannelInfo.SiteId)}, 
                {nameof(ChannelInfo.ApplyStyleId)} , 
                {nameof(ChannelInfo.QueryStyleId)}, 
                {nameof(ChannelInfo.DepartmentIdCollection)}, 
                {nameof(ChannelInfo.Summary)}
            ) VALUES (
                @{nameof(ChannelInfo.ChannelId)}, 
                @{nameof(ChannelInfo.SiteId)}, 
                @{nameof(ChannelInfo.ApplyStyleId)}, 
                @{nameof(ChannelInfo.QueryStyleId)}, 
                @{nameof(ChannelInfo.DepartmentIdCollection)}, 
                @{nameof(ChannelInfo.Summary)}
            )";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.ChannelId), channelInfo.ChannelId),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.SiteId), channelInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.ApplyStyleId), channelInfo.ApplyStyleId),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.QueryStyleId), channelInfo.QueryStyleId),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.DepartmentIdCollection), channelInfo.DepartmentIdCollection),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.Summary), channelInfo.Summary)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);

            InteractManager.AddDefaultTypeInfos(channelInfo.SiteId, channelInfo.ChannelId);
        }

        public static void Update(ChannelInfo channelInfo)
        {
            string sqlString = $"UPDATE {TableName} SET {nameof(ChannelInfo.DepartmentIdCollection)} = @{nameof(ChannelInfo.DepartmentIdCollection)}, {nameof(ChannelInfo.Summary)} = @{nameof(ChannelInfo.Summary)} WHERE {nameof(ChannelInfo.Id)} = @{nameof(ChannelInfo.Id)}";

            var parameters = new[]
            { 
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.DepartmentIdCollection), channelInfo.DepartmentIdCollection),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.Summary), channelInfo.Summary),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.Id), channelInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static ChannelInfo GetChannelInfo(int siteId, int channelId)
        {
            ChannelInfo channelInfo = null;

            var sqlString = $@"SELECT 
                {nameof(ChannelInfo.Id)}, 
                {nameof(ChannelInfo.ChannelId)}, 
                {nameof(ChannelInfo.SiteId)}, 
                {nameof(ChannelInfo.ApplyStyleId)} , 
                {nameof(ChannelInfo.QueryStyleId)}, 
                {nameof(ChannelInfo.DepartmentIdCollection)}, 
                {nameof(ChannelInfo.Summary)} FROM {TableName} WHERE {nameof(ChannelInfo.SiteId)} = @{nameof(ChannelInfo.SiteId)} AND {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                { 
                    channelInfo = GetChannelInfo(rdr);
                }
                rdr.Close();
            }

            if (channelInfo == null)
            {
                var theChannelInfo = new ChannelInfo(0, channelId, siteId, 0, 0, string.Empty, string.Empty);

                Insert(theChannelInfo);

                using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
                {
                    if (rdr.Read())
                    {
                        channelInfo = GetChannelInfo(rdr);
                    }
                    rdr.Close();
                }
            }

            return channelInfo;
        }

        public static bool IsExists(int channelId)
        {
            var exists = false;

            var sqlString = $"SELECT {nameof(ChannelInfo.Id)} FROM {TableName} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        exists = true;
                    }
                }
                rdr.Close();
            }
            return exists;
        }

        private static ChannelInfo GetChannelInfo(IDataReader rdr)
        {
            if (rdr == null)
                return null;
            var i = 0;

            return new ChannelInfo
            {
                Id = Context.DatabaseApi.GetInt(rdr, i++),
                ChannelId = Context.DatabaseApi.GetInt(rdr, i++),
                SiteId = Context.DatabaseApi.GetInt(rdr, i++),
                ApplyStyleId = Context.DatabaseApi.GetInt(rdr, i++),
                QueryStyleId = Context.DatabaseApi.GetInt(rdr, i++),
                DepartmentIdCollection = Context.DatabaseApi.GetString(rdr, i++),
                Summary = Context.DatabaseApi.GetString(rdr, i) 
            };
        }
    }
}
