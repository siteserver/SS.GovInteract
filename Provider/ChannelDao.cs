using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class ChannelDao
    {
        public const string TableName = "ss_govinteract_channel";

        public static List<TableColumn> Columns => new List<TableColumn>
        { 
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

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public ChannelDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void Insert(ChannelInfo channelInfo)
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
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelInfo.ChannelId),
                _helper.GetParameter(nameof(ChannelInfo.SiteId), channelInfo.SiteId),
                _helper.GetParameter(nameof(ChannelInfo.ApplyStyleId), channelInfo.ApplyStyleId),
                _helper.GetParameter(nameof(ChannelInfo.QueryStyleId), channelInfo.QueryStyleId),
                _helper.GetParameter(nameof(ChannelInfo.DepartmentIdCollection), channelInfo.DepartmentIdCollection),
                _helper.GetParameter(nameof(ChannelInfo.Summary), channelInfo.Summary)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);

            InteractManager.AddDefaultTypeInfos(channelInfo.SiteId, channelInfo.ChannelId);
        }

        public void Update(ChannelInfo channelInfo)
        {
            string sqlString = $"UPDATE {TableName} SET {nameof(ChannelInfo.DepartmentIdCollection)} = @{nameof(ChannelInfo.DepartmentIdCollection)}, {nameof(ChannelInfo.Summary)} = @{nameof(ChannelInfo.Summary)} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            { 
                _helper.GetParameter(nameof(ChannelInfo.DepartmentIdCollection), channelInfo.DepartmentIdCollection),
                _helper.GetParameter(nameof(ChannelInfo.Summary), channelInfo.Summary),
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelInfo.ChannelId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Delete(int channelId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public ChannelInfo GetChannelInfo(int siteId, int channelId)
        {
            ChannelInfo channelInfo = null;

            string sqlString = $@"SELECT {nameof(ChannelInfo.ChannelId)}, 
                {nameof(ChannelInfo.SiteId)}, 
                {nameof(ChannelInfo.ApplyStyleId)} , 
                {nameof(ChannelInfo.QueryStyleId)}, 
                {nameof(ChannelInfo.DepartmentIdCollection)}, 
                {nameof(ChannelInfo.Summary)} FROM {TableName} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                { 
                    channelInfo = GetChannelInfo(rdr);
                }
                rdr.Close();
            }

            if (channelInfo == null)
            {
                ChannelInfo theChannelInfo = new ChannelInfo(channelId, siteId, 0, 0, string.Empty, string.Empty);

                Insert(theChannelInfo);

                using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
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

        public string GetSummary(int channelId)
        {
            string summary = string.Empty; 
            string sqlString = $"SELECT {nameof(ChannelInfo.Summary)} FROM {TableName} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    summary = _helper.GetString(rdr, 0);
                }
                rdr.Close();
            }
            return summary;
        }

        public int GetApplyStyleId(int siteId, int channelId)
        {
            string sqlString = $"SELECT {nameof(ChannelInfo.ApplyStyleId)} FROM {TableName} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            int styleId = _helper.ExecuteInt(_connectionString, sqlString, parameters);

            if (styleId == 0)
            {
                var theChannelInfo = new ChannelInfo(channelId, siteId, 0, 0, string.Empty, string.Empty);

                Insert(theChannelInfo);

                styleId = _helper.ExecuteInt(_connectionString, sqlString, parameters);
            }

            return styleId;
        }

        public int GetQueryStyleId(int siteId, int channelId)
        { 
            string sqlString = $"SELECT {nameof(ChannelInfo.QueryStyleId)} FROM {TableName} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            int styleId = _helper.ExecuteInt(_connectionString, sqlString, parameters);

            if (styleId == 0)
            {
                var theChannelInfo = new ChannelInfo(channelId, siteId, 0, 0, string.Empty, string.Empty);

                Insert(theChannelInfo);

                styleId = _helper.ExecuteInt(_connectionString, sqlString, parameters);
            }

            return styleId; 
        }

        public int GetChannelIdByApplyStyleId(int applyStyleId)
        {
            string sqlString = $"SELECT {nameof(ChannelInfo.ChannelId)} FROM {TableName} WHERE {nameof(ChannelInfo.ApplyStyleId)} = @{nameof(ChannelInfo.ApplyStyleId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.ApplyStyleId), applyStyleId)
            };

            return _helper.ExecuteInt(_connectionString, sqlString, parameters); 
        }

        public int GetChannelIdByQueryStyleId(int queryStyleId)
        {
            string sqlString = $"SELECT {nameof(ChannelInfo.ChannelId)} FROM {TableName} WHERE {nameof(ChannelInfo.QueryStyleId)} = @{nameof(ChannelInfo.QueryStyleId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.QueryStyleId), queryStyleId)
            };

            return _helper.ExecuteInt(_connectionString, sqlString, parameters); 
        }

        public bool IsExists(int channelId)
        {
            var exists = false;

            string sqlString = $"SELECT {nameof(ChannelInfo.ChannelId)} FROM {TableName} WHERE {nameof(ChannelInfo.ChannelId)} = @{nameof(ChannelInfo.ChannelId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(ChannelInfo.ChannelId), channelId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
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

        private ChannelInfo GetChannelInfo(IDataReader rdr)
        {
            if (rdr == null)
                return null;
            var i = 0;
            return new ChannelInfo
            {
                ChannelId = _helper.GetInt(rdr, i++),
                SiteId = _helper.GetInt(rdr, i++),
                ApplyStyleId = _helper.GetInt(rdr, i++),
                QueryStyleId = _helper.GetInt(rdr, i++),
                DepartmentIdCollection = _helper.GetString(rdr, i++),
                Summary = _helper.GetString(rdr, i) 
            };
        }
    }
}
