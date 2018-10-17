using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public static class LogDao
    {
        public const string TableName = "ss_govinteract_log";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(LogInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.ChannelId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.ContentId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.DepartmentId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.UserName),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.LogType),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.IpAddress),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.AddDate),
                DataType = DataType.DateTime
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.Summary),
                DataType = DataType.VarChar,
                DataLength = 255
            }
        };

        public static void Insert(LogInfo logInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
            (
                {nameof(LogInfo.SiteId)}, 
                {nameof(LogInfo.ChannelId)}, 
                {nameof(LogInfo.ContentId)}, 
                {nameof(LogInfo.DepartmentId)},
                {nameof(LogInfo.UserName)},
                {nameof(LogInfo.LogType)},
                {nameof(LogInfo.IpAddress)},
                {nameof(LogInfo.AddDate)},
                {nameof(LogInfo.Summary)}
            ) VALUES (
                @{nameof(LogInfo.SiteId)}, 
                @{nameof(LogInfo.ChannelId)}, 
                @{nameof(LogInfo.ContentId)}, 
                @{nameof(LogInfo.DepartmentId)},
                @{nameof(LogInfo.UserName)},
                @{nameof(LogInfo.LogType)},
                @{nameof(LogInfo.IpAddress)},
                @{nameof(LogInfo.AddDate)},
                @{nameof(LogInfo.Summary)}
            )";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(LogInfo.SiteId), logInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.ChannelId), logInfo.ChannelId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.ContentId), logInfo.ContentId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.DepartmentId), logInfo.DepartmentId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.UserName), logInfo.UserName),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.LogType), logInfo.LogType),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.IpAddress), logInfo.IpAddress),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.AddDate), logInfo.AddDate),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.Summary), logInfo.Summary)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static void Delete(ArrayList idArrayList)
        {
            if (idArrayList == null || idArrayList.Count <= 0) return;
            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(LogInfo.Summary)} IN ({Utils.ToSqlInStringWithoutQuote(idArrayList)})";

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static void DeleteAll(int siteId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId}";

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static void Delete(int id)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(LogInfo.Id)} = {id}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static void DeleteByContentId(int siteId, int contentId)
        {
            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId} AND {nameof(LogInfo.ContentId)} = {contentId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static IEnumerable GetDataSourceByContentId(int siteId, int contentId)
        {
            string sqlString =
                $"SELECT {nameof(LogInfo.Id)}, {nameof(LogInfo.SiteId)}, {nameof(LogInfo.ChannelId)}, {nameof(LogInfo.ContentId)}, {nameof(LogInfo.DepartmentId)}, {nameof(LogInfo.UserName)}, {nameof(LogInfo.LogType)}, {nameof(LogInfo.IpAddress)}, {nameof(LogInfo.AddDate)}, {nameof(LogInfo.Summary)} FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId} AND {nameof(LogInfo.ContentId)} = {contentId} ORDER BY {nameof(LogInfo.Id)}";

            var enumerable = (IEnumerable)Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString);
            return enumerable;
        }

        public static List<LogInfo> GetLogInfoList(int siteId, int contentId)
        {
            var list = new List<LogInfo>();

            string sqlString =
                $"SELECT {nameof(LogInfo.Id)}, {nameof(LogInfo.SiteId)}, {nameof(LogInfo.ChannelId)}, {nameof(LogInfo.ContentId)}, {nameof(LogInfo.DepartmentId)}, {nameof(LogInfo.UserName)}, {nameof(LogInfo.LogType)}, {nameof(LogInfo.IpAddress)}, {nameof(LogInfo.AddDate)}, {nameof(LogInfo.Summary)} FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId} AND {nameof(LogInfo.ContentId)} = {contentId} ORDER BY {nameof(LogInfo.Id)}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                while (rdr.Read())
                { 
                    var logInfo = GetLogInfo(rdr);
                    list.Add(logInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public static string GetSelectCommend(int siteId)
        {
            return  $"SELECT {nameof(LogInfo.Id)}, {nameof(LogInfo.SiteId)}, {nameof(LogInfo.ChannelId)}, {nameof(LogInfo.ContentId)}, {nameof(LogInfo.DepartmentId)}, {nameof(LogInfo.UserName)}, {nameof(LogInfo.LogType)}, {nameof(LogInfo.IpAddress)}, {nameof(LogInfo.AddDate)}, {nameof(LogInfo.Summary)} FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId}";

        }

        public static string GetSelectCommend(int siteId, string keyword, string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return GetSelectCommend(siteId);
            }

            var whereString = new StringBuilder();
            whereString.AppendFormat("WHERE (SiteId = {0})", siteId);

            if (!string.IsNullOrEmpty(keyword))
            {
                whereString.AppendFormat(" AND (UserName LIKE '%{0}%' OR Summary LIKE '%{0}%')", Utils.FilterSql(keyword));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                whereString.AppendFormat(" AND (AddDate >= '{0}')", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                whereString.AppendFormat(" AND (AddDate <= '{0}')", dateTo);
            }

            return  $"SELECT {nameof(LogInfo.Id)}, {nameof(LogInfo.SiteId)}, {nameof(LogInfo.ChannelId)}, {nameof(LogInfo.ContentId)}, {nameof(LogInfo.DepartmentId)}, {nameof(LogInfo.UserName)}, {nameof(LogInfo.LogType)}, {nameof(LogInfo.IpAddress)}, {nameof(LogInfo.AddDate)}, {nameof(LogInfo.Summary)} FROM {TableName} {whereString}";
        }

        public static LogInfo GetLogInfo(int id)
        {
            LogInfo logInfo = null;

            var sqlString = $@"SELECT {nameof(LogInfo.Id)}, 
                    {nameof(LogInfo.SiteId)}, 
                    {nameof(LogInfo.ChannelId)}, 
                    {nameof(LogInfo.ContentId)}, 
                    {nameof(LogInfo.DepartmentId)},
                    {nameof(LogInfo.UserName)},
                    {nameof(LogInfo.LogType)},
                    {nameof(LogInfo.IpAddress)},
                    {nameof(LogInfo.AddDate)},
                    {nameof(LogInfo.Summary)}
                    FROM {TableName} WHERE {nameof(LogInfo.Id)} = @{nameof(LogInfo.Id)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(logInfo.Id), id),
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    logInfo = GetLogInfo(rdr);
                }
                rdr.Close();
            }

            return logInfo;
        }

        private static LogInfo GetLogInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new LogInfo(Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetInt(rdr, i++),
                Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetString(rdr, i++),
                Context.DatabaseApi.GetString(rdr, i++), Context.DatabaseApi.GetString(rdr, i++), Context.DatabaseApi.GetDateTime(rdr, i++),
                Context.DatabaseApi.GetString(rdr, i));
        }
    }
}
