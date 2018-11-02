using System.Collections;
using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovInteract.Model;

namespace SS.GovInteract.Provider
{
    public static class TypeDao
    {
        public const string TableName = "ss_govinteract_type"; 

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(TypeInfo.Id),
                DataType = DataType.Integer,
                IsPrimaryKey = true,
                IsIdentity = true
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

        public static void Insert(TypeInfo typeInfo)
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
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.TypeName), typeInfo.TypeName),
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.ChannelId), typeInfo.ChannelId),
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.SiteId), typeInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.Taxis), typeInfo.Taxis) 
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters); 
        }

        public static void Update(TypeInfo typeInfo)
        {
            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.TypeName), typeInfo.TypeName),
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.Id), typeInfo.Id) 
            };

            string sqlString = $@"UPDATE {TableName} SET {nameof(TypeInfo.TypeName)} = @{nameof(TypeInfo.TypeName)} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)} ";

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters); 
        }

        public static void Delete(int typeId)
        {
            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.Id), typeId)
            };

            string sqlString = $@"DELETE FROM {TableName} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static TypeInfo GetTypeInfo(int typeId)
        {
            TypeInfo typeInfo = null;

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.Id), typeId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.Id)}, 
                        {nameof(TypeInfo.TypeName)}, 
                        {nameof(TypeInfo.ChannelId)}, 
                        {nameof(TypeInfo.SiteId)}, 
                        {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                { 
                    typeInfo = GetTypeInfo(rdr);
                }
                rdr.Close();
            }

            return typeInfo;
        }

        public static string GetTypeName(int typeId)
        {
            var typeName = string.Empty;

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.Id), typeId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.TypeName)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = @{nameof(TypeInfo.Id)}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    typeName = Context.DatabaseApi.GetString(rdr, 0);
                }
                rdr.Close();
            }

            return typeName;
        }

        public static IEnumerable GetDataSource(int channelId)
        {
            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.ChannelId), channelId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.Id)}, 
                        {nameof(TypeInfo.TypeName)}, 
                        {nameof(TypeInfo.ChannelId)}, 
                        {nameof(TypeInfo.SiteId)}, 
                        {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = @{nameof(TypeInfo.ChannelId)} ORDER BY {nameof(TypeInfo.Taxis)}";

            var enumerable = (IEnumerable)Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters);

            return enumerable;
        }

        public static List<TypeInfo> GetTypeInfoList(int channelId)
        {
            var list = new List<TypeInfo>();

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.ChannelId), channelId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.Id)}, 
                        {nameof(TypeInfo.TypeName)}, 
                        {nameof(TypeInfo.ChannelId)}, 
                        {nameof(TypeInfo.SiteId)}, 
                        {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = @{nameof(TypeInfo.ChannelId)} ORDER BY {nameof(TypeInfo.Taxis)}";


            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    list.Add(GetTypeInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public static List<string> GetTypeNameList(int channelId)
        {
            var arraylist = new List<string>();

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(TypeInfo.ChannelId), channelId)
            };

            string sqlString = $@"SELECT {nameof(TypeInfo.TypeName)} FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = @{nameof(TypeInfo.ChannelId)} ORDER BY {nameof(TypeInfo.Taxis)}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    arraylist.Add(Context.DatabaseApi.GetString(rdr, 0));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public static bool UpdateTaxisToUp(int typeId, int channelId)
        { 
            string sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $"{nameof(TypeInfo.Id)}, {nameof(TypeInfo.Taxis)}", $"WHERE (({nameof(TypeInfo.Taxis)} > (SELECT {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = {typeId})) AND {nameof(TypeInfo.ChannelId)} ={channelId})", $"ORDER BY {nameof(TypeInfo.Taxis)}", 0, 1);

            var higherId = 0;
            var higherTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString,sqlString))
            {
                if (rdr.Read())
                {
                    higherId = Context.DatabaseApi.GetInt(rdr, 0);
                    higherTaxis = Context.DatabaseApi.GetInt(rdr, 1);
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

        public static bool UpdateTaxisToDown(int typeId, int channelId)
        {
            //var sqlString = SqlUtils.GetTopSqlString("wcm_GovInteractType", "TypeID, Taxis", $"WHERE ((Taxis < (SELECT Taxis FROM wcm_GovInteractType WHERE TypeID = {typeId})) AND ChannelId = {channelId}) ORDER BY Taxis DESC", 1);
            string sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $"{nameof(TypeInfo.Id)}, {nameof(TypeInfo.Taxis)}", $"WHERE (({nameof(TypeInfo.Taxis)} < (SELECT {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = {typeId})) AND {nameof(TypeInfo.ChannelId)} ={channelId})", $"ORDER BY {nameof(TypeInfo.Taxis)} DESC", 0, 1);

            var lowerId = 0;
            var lowerTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString,sqlString))
            {
                if (rdr.Read())
                {
                    lowerId = Context.DatabaseApi.GetInt(rdr, 0);
                    lowerTaxis = Context.DatabaseApi.GetInt(rdr, 1);
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

        private static int GetMaxTaxis(int channelId)
        { 
            var sqlString = $"SELECT MAX({nameof(TypeInfo.Taxis)}) FROM {TableName} WHERE {nameof(TypeInfo.ChannelId)} = {channelId}";
             
            return Dao.GetIntResult(sqlString);  
        }

        private static int GetTaxis(int typeId)
        {
            var sqlString = $"SELECT {nameof(TypeInfo.Taxis)} FROM {TableName} WHERE {nameof(TypeInfo.Id)} = {typeId}";

            return Dao.GetIntResult(sqlString);
        }

        private static void SetTaxis(int typeId, int channelId, int taxis)
        {
            var sqlString = $"UPDATE {TableName} SET {nameof(TypeInfo.Taxis)} = {taxis} WHERE {nameof(TypeInfo.Id)} = {typeId} AND {nameof(TypeInfo.ChannelId)} = {channelId}";

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString,sqlString);
        }

        private static TypeInfo GetTypeInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new TypeInfo
            { 
                Id = Context.DatabaseApi.GetInt(rdr, i++),
                TypeName = Context.DatabaseApi.GetString(rdr, i++),
                ChannelId = Context.DatabaseApi.GetInt(rdr, i++),
                SiteId = Context.DatabaseApi.GetInt(rdr, i++),
                Taxis = Context.DatabaseApi.GetInt(rdr, i) 
            }; 
        }
    }
}
