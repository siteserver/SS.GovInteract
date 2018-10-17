using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public static class ReplyDao
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

        public static void Insert(ReplyInfo replyInfo)
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
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.SiteId), replyInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.ChannelId), replyInfo.ChannelId),
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.ContentId), replyInfo.ContentId),
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.Reply), replyInfo.Reply),
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.FileUrl), replyInfo.FileUrl),
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.DepartmentId), replyInfo.DepartmentId),
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.UserName), replyInfo.UserName),
                Context.DatabaseApi.GetParameter(nameof(ReplyInfo.AddDate), replyInfo.AddDate)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static void Delete(int replyId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(ReplyInfo.Id)} = {replyId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static void DeleteByContentId(int siteId, int contentId)
        {
            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(ReplyInfo.SiteId)} = {siteId} AND {nameof(ReplyInfo.ContentId)} = {contentId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static ReplyInfo GetReplyInfo(int replayId)
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
                Context.DatabaseApi.GetParameter(nameof(replyInfo.Id), replayId),
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    replyInfo = GetReplyInfo(rdr);
                }
                rdr.Close();
            }

            return replyInfo;
        }

        public static ReplyInfo GetReplyInfoByContentId(int siteId, int contentId)
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
                Context.DatabaseApi.GetParameter(nameof(replyInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(replyInfo.ContentId), contentId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    replyInfo = GetReplyInfo(rdr);
                }
                rdr.Close();
            }

            return replyInfo;
        }

        private static ReplyInfo GetReplyInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new ReplyInfo(Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetInt(rdr, i++),
                Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetString(rdr, i++), Context.DatabaseApi.GetString(rdr, i++),
                Context.DatabaseApi.GetInt(rdr, i++), Context.DatabaseApi.GetString(rdr, i++), Context.DatabaseApi.GetDateTime(rdr, i));
        }
    }
}
