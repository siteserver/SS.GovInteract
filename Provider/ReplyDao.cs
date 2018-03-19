using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class ReplyDao
    {
        public const string TableName = "ss_govinteract_reply";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.ChannelId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.ContentId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.Reply),
                DataType = DataType.Text 
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.FileUrl),
                DataType = DataType.VarChar,
                DataLength = 255
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.DepartmentId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.UserName),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(ReplyInfo.AddDate),
                DataType = DataType.DateTime
            }
        };

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public ReplyDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void Insert(ReplyInfo replyInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
            (
                {nameof(ReplyInfo.SiteId)}, 
                {nameof(ReplyInfo.ChannelId)}, 
                {nameof(ReplyInfo.ContentId)}, 
                {nameof(ReplyInfo.Reply)},
                {nameof(ReplyInfo.FileUrl)},
                {nameof(ReplyInfo.DepartmentId)},
                {nameof(ReplyInfo.UserName)},
                {nameof(ReplyInfo.AddDate)}
            ) VALUES (
                @{nameof(ReplyInfo.SiteId)}, 
                @{nameof(ReplyInfo.ChannelId)}, 
                @{nameof(ReplyInfo.ContentId)}, 
                @{nameof(ReplyInfo.Reply)},
                @{nameof(ReplyInfo.FileUrl)},
                @{nameof(ReplyInfo.DepartmentId)},
                @{nameof(ReplyInfo.UserName)},
                @{nameof(ReplyInfo.AddDate)}
            )";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ReplyInfo.SiteId), replyInfo.SiteId),
                _helper.GetParameter(nameof(ReplyInfo.ChannelId), replyInfo.ChannelId),
                _helper.GetParameter(nameof(ReplyInfo.ContentId), replyInfo.ContentId),
                _helper.GetParameter(nameof(ReplyInfo.Reply), replyInfo.Reply),
                _helper.GetParameter(nameof(ReplyInfo.FileUrl), replyInfo.FileUrl),
                _helper.GetParameter(nameof(ReplyInfo.DepartmentId), replyInfo.DepartmentId),
                _helper.GetParameter(nameof(ReplyInfo.UserName), replyInfo.UserName),
                _helper.GetParameter(nameof(ReplyInfo.AddDate), replyInfo.AddDate)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Delete(int replyId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(ReplyInfo.Id)} = {replyId}";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        public void DeleteByContentId(int siteId, int contentId)
        {
            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(ReplyInfo.SiteId)} = {siteId} AND {nameof(ReplyInfo.ContentId)} = {contentId}";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        public ReplyInfo GetReplyInfo(int replayId)
        {
            ReplyInfo replyInfo = null;

            var sqlString = $@"SELECT {nameof(ReplyInfo.Id)}, 
                    {nameof(ReplyInfo.SiteId)}, 
                    {nameof(ReplyInfo.ChannelId)}, 
                    {nameof(ReplyInfo.ContentId)}, 
                    {nameof(ReplyInfo.Reply)},
                    {nameof(ReplyInfo.FileUrl)},
                    {nameof(ReplyInfo.DepartmentId)},
                    {nameof(ReplyInfo.UserName)},
                    {nameof(ReplyInfo.AddDate)}
                    FROM {TableName} WHERE {nameof(ReplyInfo.Id)} = @{nameof(ReplyInfo.Id)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(replyInfo.Id), replayId),
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    replyInfo = GetReplyInfo(rdr);
                }
                rdr.Close();
            }

            return replyInfo;
        }

        public ReplyInfo GetReplyInfoByContentId(int siteId, int contentId)
        {
            ReplyInfo replyInfo = null;

            var sqlString = $@"SELECT {nameof(ReplyInfo.Id)}, 
                    {nameof(ReplyInfo.SiteId)}, 
                    {nameof(ReplyInfo.ChannelId)}, 
                    {nameof(ReplyInfo.ContentId)}, 
                    {nameof(ReplyInfo.Reply)},
                    {nameof(ReplyInfo.FileUrl)},
                    {nameof(ReplyInfo.DepartmentId)},
                    {nameof(ReplyInfo.UserName)},
                    {nameof(ReplyInfo.AddDate)}
                    FROM {TableName} WHERE {nameof(ReplyInfo.SiteId)} = @{nameof(ReplyInfo.SiteId)} AND {nameof(ReplyInfo.ContentId)} = @{nameof(ReplyInfo.ContentId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(replyInfo.SiteId), siteId),
                _helper.GetParameter(nameof(replyInfo.ContentId), contentId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    replyInfo = GetReplyInfo(rdr);
                }
                rdr.Close();
            }

            return replyInfo;
        }

        private ReplyInfo GetReplyInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new ReplyInfo(_helper.GetInt(rdr, i++), _helper.GetInt(rdr, i++), _helper.GetInt(rdr, i++),
                _helper.GetInt(rdr, i++), _helper.GetString(rdr, i++), _helper.GetString(rdr, i++),
                _helper.GetInt(rdr, i++), _helper.GetString(rdr, i++), _helper.GetDateTime(rdr, i));
        }
    }
}
