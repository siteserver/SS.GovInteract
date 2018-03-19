using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class LogDao
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

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public LogDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void Insert(LogInfo logInfo)
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
                _helper.GetParameter(nameof(LogInfo.SiteId), logInfo.SiteId),
                _helper.GetParameter(nameof(LogInfo.ChannelId), logInfo.ChannelId),
                _helper.GetParameter(nameof(LogInfo.ContentId), logInfo.ContentId),
                _helper.GetParameter(nameof(LogInfo.DepartmentId), logInfo.DepartmentId),
                _helper.GetParameter(nameof(LogInfo.UserName), logInfo.UserName),
                _helper.GetParameter(nameof(LogInfo.LogType), logInfo.LogType),
                _helper.GetParameter(nameof(LogInfo.IpAddress), logInfo.IpAddress),
                _helper.GetParameter(nameof(LogInfo.AddDate), logInfo.AddDate),
                _helper.GetParameter(nameof(LogInfo.Summary), logInfo.Summary)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList == null || idArrayList.Count <= 0) return;
            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(LogInfo.Summary)} IN ({Utils.ToSqlInStringWithoutQuote(idArrayList)})";

            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        public void DeleteAll(int siteId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId}";

            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        public void Delete(int id)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(LogInfo.Id)} = {id}";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        public void DeleteByContentId(int siteId, int contentId)
        {
            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId} AND {nameof(LogInfo.ContentId)} = {contentId}";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        public IEnumerable GetDataSourceByContentId(int siteId, int contentId)
        {
            string sqlString =
                $"SELECT {nameof(LogInfo.Id)}, {nameof(LogInfo.SiteId)}, {nameof(LogInfo.ChannelId)}, {nameof(LogInfo.ContentId)}, {nameof(LogInfo.DepartmentId)}, {nameof(LogInfo.UserName)}, {nameof(LogInfo.LogType)}, {nameof(LogInfo.IpAddress)}, {nameof(LogInfo.AddDate)}, {nameof(LogInfo.Summary)} FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId} AND {nameof(LogInfo.ContentId)} = {contentId} ORDER BY {nameof(LogInfo.Id)}";

            var enumerable = (IEnumerable)_helper.ExecuteReader(_connectionString, sqlString);
            return enumerable;
        }

        public List<LogInfo> GetLogInfoList(int siteId, int contentId)
        {
            var list = new List<LogInfo>();

            string sqlString =
                $"SELECT {nameof(LogInfo.Id)}, {nameof(LogInfo.SiteId)}, {nameof(LogInfo.ChannelId)}, {nameof(LogInfo.ContentId)}, {nameof(LogInfo.DepartmentId)}, {nameof(LogInfo.UserName)}, {nameof(LogInfo.LogType)}, {nameof(LogInfo.IpAddress)}, {nameof(LogInfo.AddDate)}, {nameof(LogInfo.Summary)} FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId} AND {nameof(LogInfo.ContentId)} = {contentId} ORDER BY {nameof(LogInfo.Id)}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
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

        public string GetSelectCommend(int siteId)
        {
            return  $"SELECT {nameof(LogInfo.Id)}, {nameof(LogInfo.SiteId)}, {nameof(LogInfo.ChannelId)}, {nameof(LogInfo.ContentId)}, {nameof(LogInfo.DepartmentId)}, {nameof(LogInfo.UserName)}, {nameof(LogInfo.LogType)}, {nameof(LogInfo.IpAddress)}, {nameof(LogInfo.AddDate)}, {nameof(LogInfo.Summary)} FROM {TableName} WHERE {nameof(LogInfo.SiteId)} = {siteId}";

        }

        public string GetSelectCommend(int siteId, string keyword, string dateFrom, string dateTo)
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

        public LogInfo GetLogInfo(int id)
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
                _helper.GetParameter(nameof(logInfo.Id), id),
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    logInfo = GetLogInfo(rdr);
                }
                rdr.Close();
            }

            return logInfo;
        } 

        private LogInfo GetLogInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new LogInfo(_helper.GetInt(rdr, i++), _helper.GetInt(rdr, i++), _helper.GetInt(rdr, i++),
                _helper.GetInt(rdr, i++), _helper.GetInt(rdr, i++), _helper.GetString(rdr, i++),
                _helper.GetString(rdr, i++), _helper.GetString(rdr, i++), _helper.GetDateTime(rdr, i++),
                _helper.GetString(rdr, i));
        }
    }
}
