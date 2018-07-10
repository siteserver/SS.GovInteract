using System.Collections;
using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public class TypeDao
    {
        public const string TableName = "ss_govinteract_type"; 

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(TypeInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(TypeInfo.TypeName),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(TypeInfo.ChannelId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(TypeInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(TypeInfo.Taxis),
                DataType = DataType.Integer
            } 
        };

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public TypeDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void Insert(TypeInfo typeInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
            (
                {nameof(TypeInfo.TypeName)}, 
                {nameof(TypeInfo.ChannelId)}, 
                {nameof(TypeInfo.SiteId)}, 
                {nameof(TypeInfo.Taxis)} 
            ) VALUES (
                @{nameof(TypeInfo.TypeName)}, 
                @{nameof(TypeInfo.ChannelId)}, 
                @{nameof(TypeInfo.SiteId)}, 
                @{nameof(TypeInfo.Taxis)} 
            )";

            if(typeInfo.Taxis == 0)
            {
                typeInfo.Taxis = GetMaxTaxis(typeInfo.ChannelId) + 1;
            }
            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.TypeName), typeInfo.TypeName),
                _helper.GetParameter(nameof(TypeInfo.ChannelId), typeInfo.ChannelId),
                _helper.GetParameter(nameof(TypeInfo.SiteId), typeInfo.SiteId),
                _helper.GetParameter(nameof(TypeInfo.Taxis), typeInfo.Taxis) 
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters); 
        }

        public void Update(TypeInfo typeInfo)
        {
            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.TypeName), typeInfo.TypeName),
                _helper.GetParameter(nameof(TypeInfo.Id), typeInfo.Id) 
            };

            string sqlString = $@"UPDATE {TableName} SET {nameof(TypeInfo.TypeName)} = @{nameof(TypeInfo.TypeName)} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)} ";

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters); 
        }

        public void Delete(int typeId)
        {
            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.Id), typeId)
            };

            string sqlString = $@"DELETE FROM {TableName} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)}";
            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public TypeInfo GetTypeInfo(int typeId)
        {
            TypeInfo typeInfo = null;

            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.Id), typeId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.Id)}, 
                        {nameof(TypeInfo.TypeName)}, 
                        {nameof(TypeInfo.ChannelId)}, 
                        {nameof(TypeInfo.SiteId)}, 
                        {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                { 
                    typeInfo = GetTypeInfo(rdr);
                }
                rdr.Close();
            }

            return typeInfo;
        }

        public string GetTypeName(int typeId)
        {
            var typeName = string.Empty;

            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.Id), typeId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.TypeName)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    typeName = _helper.GetString(rdr, 0);
                }
                rdr.Close();
            }

            return typeName;
        }

        public IEnumerable GetDataSource(int channelId)
        {
            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.ChannelId), channelId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.Id)}, 
                        {nameof(TypeInfo.TypeName)}, 
                        {nameof(TypeInfo.ChannelId)}, 
                        {nameof(TypeInfo.SiteId)}, 
                        {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = @{nameof(TypeInfo.ChannelId)} ORDER BY {nameof(TypeInfo.Taxis)}";

            var enumerable = (IEnumerable)_helper.ExecuteReader(_connectionString, sqlString, parameters);

            return enumerable;
        }

        public List<TypeInfo> GetTypeInfoList(int channelId)
        {
            var list = new List<TypeInfo>();

            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.ChannelId), channelId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.Id)}, 
                        {nameof(TypeInfo.TypeName)}, 
                        {nameof(TypeInfo.ChannelId)}, 
                        {nameof(TypeInfo.SiteId)}, 
                        {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = @{nameof(TypeInfo.ChannelId)} ORDER BY {nameof(TypeInfo.Taxis)}";


            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    list.Add(GetTypeInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public List<string> GetTypeNameList(int channelId)
        {
            var arraylist = new List<string>();

            var parameters = new[]
            {
                _helper.GetParameter(nameof(TypeInfo.ChannelId), channelId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.TypeName)} FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = @{nameof(TypeInfo.ChannelId)} ORDER BY {nameof(TypeInfo.Taxis)}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    arraylist.Add(_helper.GetString(rdr, 0));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public bool UpdateTaxisToUp(int typeId, int channelId)
        { 
            string sqlString = _helper.GetPageSqlString(TableName, $"{nameof(TypeInfo.Id)}, {nameof(TypeInfo.Taxis)}", $"WHERE (({nameof(TypeInfo.Taxis)} > (SELECT {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = {typeId})) AND {nameof(TypeInfo.ChannelId)} ={channelId})", $"ORDER BY {nameof(TypeInfo.Taxis)}", 0, 1);

            var higherId = 0;
            var higherTaxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString,sqlString))
            {
                if (rdr.Read())
                {
                    higherId = _helper.GetInt(rdr, 0);
                    higherTaxis = _helper.GetInt(rdr, 1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(typeId);

            if (higherId > 0)
            {
                SetTaxis(typeId, channelId, higherTaxis);
                SetTaxis(higherId, channelId, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int typeId, int channelId)
        {
            //var sqlString = SqlUtils.GetTopSqlString("wcm_GovInteractType", "TypeID, Taxis", $"WHERE ((Taxis < (SELECT Taxis FROM wcm_GovInteractType WHERE TypeID = {typeId})) AND ChannelId = {channelId}) ORDER BY Taxis DESC", 1);
            string sqlString = _helper.GetPageSqlString(TableName, $"{nameof(TypeInfo.Id)}, {nameof(TypeInfo.Taxis)}", $"WHERE (({nameof(TypeInfo.Taxis)} < (SELECT {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = {typeId})) AND {nameof(TypeInfo.ChannelId)} ={channelId})", $"ORDER BY {nameof(TypeInfo.Taxis)} DESC", 0, 1);

            var lowerId = 0;
            var lowerTaxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString,sqlString))
            {
                if (rdr.Read())
                {
                    lowerId = _helper.GetInt(rdr, 0);
                    lowerTaxis = _helper.GetInt(rdr, 1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(typeId);

            if (lowerId > 0)
            {
                SetTaxis(typeId, channelId, lowerTaxis);
                SetTaxis(lowerId, channelId, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int channelId)
        { 
            string sqlString = $"SELECT MAX({nameof(TypeInfo.Taxis)}) FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = {channelId}";
             
            return _helper.ExecuteInt(_connectionString, sqlString);  
        }

        private int GetTaxis(int typeId)
        {
            string sqlString = $"SELECT {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = {typeId}";

            return _helper.ExecuteInt(_connectionString, sqlString);
        }

        private void SetTaxis(int typeId, int channelId, int taxis)
        {
            string sqlString = $"UPDATE {TableName} SET {nameof(TypeInfo.Taxis)} = {taxis} WHERE {nameof(TypeInfo.Id)} = {typeId} AND {nameof(TypeInfo.ChannelId)} = {channelId}";

            _helper.ExecuteNonQuery(_connectionString,sqlString);
        }

        private TypeInfo GetTypeInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new TypeInfo
            { 
                Id = _helper.GetInt(rdr, i++),
                TypeName = _helper.GetString(rdr, i++),
                ChannelId = _helper.GetInt(rdr, i++),
                SiteId = _helper.GetInt(rdr, i++),
                Taxis = _helper.GetInt(rdr, i) 
            }; 
        }
    }
}
