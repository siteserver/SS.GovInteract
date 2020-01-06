using System;
using System.Collections.Generic;
using System.Linq;
using Datory;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Core.Provider
{
    public class DataRepository
    {
        private readonly Repository<DataInfo> _repository;
        public DataRepository()
        {
            _repository = new Repository<DataInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private static class Attr
        {
            public const string Id = nameof(DataInfo.Id);
            public const string SiteId = nameof(DataInfo.SiteId);
            public const string QueryCode = nameof(DataInfo.QueryCode);
            public const string DepartmentId = nameof(DataInfo.DepartmentId);
            public const string IsCompleted = nameof(DataInfo.IsCompleted);
            public const string State = nameof(DataInfo.State);
            public const string DenyReason = nameof(DataInfo.DenyReason);
            public const string RedoComment = nameof(DataInfo.RedoComment);
            public const string ReplyContent = nameof(DataInfo.ReplyContent);
            public const string IsReplyFiles = nameof(DataInfo.IsReplyFiles);
            public const string ReplyDate = nameof(DataInfo.ReplyDate);
            public const string Name = nameof(DataInfo.Name);
            public const string Phone = nameof(DataInfo.Phone);
            public const string Email = nameof(DataInfo.Email);
            public const string Address = nameof(DataInfo.Address);
            //public const string IsOrganization = nameof(DataInfo.IsOrganization);
            //public const string CivicName = nameof(DataInfo.CivicName);
            //public const string CivicPhone = nameof(DataInfo.CivicPhone);
            //public const string OrgName = nameof(DataInfo.OrgName);
            //public const string OrgLinkName = nameof(DataInfo.OrgLinkName);
            //public const string OrgPhone = nameof(DataInfo.OrgPhone);
            public const string Title = nameof(DataInfo.Title);
            public const string Content = nameof(DataInfo.Content);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public int Insert(DataInfo dataInfo)
        {
            if (dataInfo == null || string.IsNullOrEmpty(dataInfo.Title) ||
                string.IsNullOrEmpty(dataInfo.QueryCode)) return 0;

            dataInfo.Id = _repository.Insert(dataInfo);

            LogManager.NewApplication(dataInfo);

            DataCountManager.ClearCache(dataInfo.SiteId);

            return dataInfo.Id;
        }

        public void Redo(int siteId, int dataId, string redoComment)
        {
            _repository.Update(Q
                .Set(Attr.State, DataState.Redo.Value)
                .Set(Attr.RedoComment, redoComment)
                .Where(Attr.Id, dataId)
            );

            DataCountManager.ClearCache(siteId);
        }

        public void Deny(int siteId, int dataId, string denyReason)
        {
            _repository.Update(Q
                .Set(Attr.IsCompleted, true)
                .Set(Attr.State, DataState.Denied.Value)
                .Set(Attr.DenyReason, denyReason)
                .Where(Attr.Id, dataId)
            );

            DataCountManager.ClearCache(siteId);
        }

        public void UpdateState(int siteId, int dataId, DataState state)
        {
            var isCompleted = state == DataState.Checked || state == DataState.Denied;

            _repository.Update(Q
                .Set(Attr.IsCompleted, isCompleted)
                .Set(Attr.State, state.Value)
                .Where(Attr.Id, dataId)
            );

            DataCountManager.ClearCache(siteId);
        }

        public void UpdateStateAndDepartmentId(int siteId, int dataId, DataState state, int departmentId)
        {
            var isCompleted = state == DataState.Checked || state == DataState.Denied;

            var query = Q.Set(Attr.IsCompleted, isCompleted).Set(Attr.State, state.Value);
            if (departmentId > 0)
            {
                query.Set(Attr.DepartmentId, departmentId);
            }

            query.Where(Attr.Id, dataId);

            _repository.Update(query);

            DataCountManager.ClearCache(siteId);
        }

        public void UpdateStateAndReply(int siteId, int dataId, string replyContent, bool isReplyFiles)
        {
            _repository.Update(Q
                .Set(Attr.State, DataState.Replied.Value)
                .Set(Attr.ReplyContent, replyContent)
                .Set(Attr.IsReplyFiles, isReplyFiles)
                .Set(Attr.ReplyDate, DateTime.Now)
                .Where(Attr.Id, dataId)
            );

            DataCountManager.ClearCache(siteId);
        }

        public void UpdateDepartmentId(int siteId, int dataId, int departmentId)
        {
            _repository.Update(Q
                .Set(Attr.DepartmentId, departmentId)
                .Where(Attr.Id, dataId)
            );
        }

        public void Delete(int siteId, int dataId)
        {
            _repository.Delete(dataId);

            Main.FileRepository.DeleteByDataId(dataId);
            Main.LogRepository.DeleteByDataId(dataId);

            DataCountManager.ClearCache(siteId);
        }

        public DataInfo GetDataInfo(int dataId)
        {
            return _repository.Get(dataId);
        }

        private int GetCount(int siteId, string state)
        {
            return _repository.Count(Q.Where(Attr.SiteId, siteId).Where(Attr.State, state));
        }

        public int GetCount(int siteId, List<DataState> stateList, string keyword, int departmentId)
        {
            var query = Q.Where(Attr.SiteId, siteId);

            if (stateList.Count == 1)
            {
                query.Where(Attr.State, stateList[0].Value);
            }
            else if (stateList.Count > 1)
            {
                query.WhereIn(Attr.State, stateList.Select(state => state.Value));
            }

            if (departmentId > 0)
            {
                query.Where(Attr.DepartmentId, departmentId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(q => q
                    .WhereLike(Attr.Title, $"%{keyword}%")
                    .OrWhereLike(Attr.Name, $"%{keyword}%")
                    .OrWhereLike(Attr.Phone, $"%{keyword}%")
                    .OrWhereLike(Attr.Email, $"%{keyword}%")
                    .OrWhereLike(Attr.Address, $"%{keyword}%")
                    .OrWhereLike(Attr.Content, $"%{keyword}%")
                    .OrWhereLike(Attr.QueryCode, $"%{keyword}%")
                );
            }

            return _repository.Count(query);
        }

        public int GetUserCount(int siteId, List<DataState> stateList, string keyword, int departmentId, List<int> userDepartmentIdList)
        {
            if (userDepartmentIdList.Count == 0) return 0;

            var query = Q.Where(Attr.SiteId, siteId).WhereIn(Attr.DepartmentId, userDepartmentIdList);

            if (stateList.Count == 1)
            {
                query.Where(Attr.State, stateList[0].Value);
            }
            else if (stateList.Count > 1)
            {
                query.WhereIn(Attr.State, stateList.Select(state => state.Value));
            }

            if (departmentId > 0)
            {
                query.Where(Attr.DepartmentId, departmentId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(q => q
                    .WhereLike(Attr.Title, $"%{keyword}%")
                    .OrWhereLike(Attr.Name, $"%{keyword}%")
                    .OrWhereLike(Attr.Phone, $"%{keyword}%")
                    .OrWhereLike(Attr.Email, $"%{keyword}%")
                    .OrWhereLike(Attr.Address, $"%{keyword}%")
                    .OrWhereLike(Attr.Content, $"%{keyword}%")
                    .OrWhereLike(Attr.QueryCode, $"%{keyword}%")
                );
            }

            return _repository.Count(query);
        }

        public IList<DataInfo> GetDataInfoList(int siteId, List<DataState> stateList, string keyword, int departmentId, int offset, int limit)
        {
            var query = Q.Where(Attr.SiteId, siteId);

            if (stateList.Count == 1)
            {
                query.Where(Attr.State, stateList[0].Value);
            }
            else if (stateList.Count > 1)
            {
                query.WhereIn(Attr.State, stateList.Select(state => state.Value));
            }

            if (departmentId > 0)
            {
                query.Where(Attr.DepartmentId, departmentId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(q => q
                    .WhereLike(Attr.Title, $"%{keyword}%")
                    .OrWhereLike(Attr.Name, $"%{keyword}%")
                    .OrWhereLike(Attr.Phone, $"%{keyword}%")
                    .OrWhereLike(Attr.Email, $"%{keyword}%")
                    .OrWhereLike(Attr.Address, $"%{keyword}%")
                    .OrWhereLike(Attr.Content, $"%{keyword}%")
                    .OrWhereLike(Attr.QueryCode, $"%{keyword}%")
                );
            }

            query.OrderBy(Attr.IsCompleted, Attr.Id);
            query.Offset(offset).Limit(limit);

            return _repository.GetAll(query);
        }

        public IList<DataInfo> GetUserDataInfoList(int siteId, List<DataState> stateList, string keyword, int departmentId, List<int> userDepartmentIdList, int offset, int limit)
        {
            if (userDepartmentIdList.Count == 0) return new List<DataInfo>();

            var query = Q.Where(Attr.SiteId, siteId).WhereIn(Attr.DepartmentId, userDepartmentIdList);

            if (stateList.Count == 1)
            {
                query.Where(Attr.State, stateList[0].Value);
            }
            else if (stateList.Count > 1)
            {
                query.WhereIn(Attr.State, stateList.Select(state => state.Value));
            }

            if (departmentId > 0)
            {
                query.Where(Attr.DepartmentId, departmentId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(q => q
                    .WhereLike(Attr.Title, $"%{keyword}%")
                    .OrWhereLike(Attr.Name, $"%{keyword}%")
                    .OrWhereLike(Attr.Phone, $"%{keyword}%")
                    .OrWhereLike(Attr.Email, $"%{keyword}%")
                    .OrWhereLike(Attr.Address, $"%{keyword}%")
                    .OrWhereLike(Attr.Content, $"%{keyword}%")
                    .OrWhereLike(Attr.QueryCode, $"%{keyword}%")
                );
            }

            query.OrderBy(Attr.IsCompleted, Attr.Id);
            query.Offset(offset).Limit(limit);

            return _repository.GetAll(query);
        }

        public DataInfo Query(int siteId, string name, string queryCode)
        {
            var query = Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.Name, name)
                .Where(Attr.QueryCode, queryCode);

            return _repository.Get(query);
        }

        public Dictionary<string, int> GetDataCounts(int siteId)
        {
            var dict = new Dictionary<string, int>();
            foreach (var state in DataState.AllStates)
            {
                dict[state.Value] = GetCount(siteId, state.Value);
            }

            return dict;
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Data;
//using SiteServer.Plugin;
//using SS.GovInteract.Core.Model;
//using SS.GovInteract.Core.Utils;

//namespace SS.GovInteract.Core.Provider
//{
//    public static class DataDao
//    {
//        public const string TableName = "ss_application_data";

//        public static List<TableColumn> Columns => new List<TableColumn>
//        {
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.Id),
//                DataType = DataType.Integer,
//                IsIdentity = true,
//                IsPrimaryKey = true
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.SiteId),
//                DataType = DataType.Integer
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.AddDate),
//                DataType = DataType.DateTime
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.QueryCode),
//                DataType = DataType.VarChar,
//                DataLength = 50
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.DepartmentId),
//                DataType = DataType.Integer
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.IsCompleted),
//                DataType = DataType.Boolean
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.State),
//                DataType = DataType.VarChar,
//                DataLength = 50
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.DenyReason),
//                DataType = DataType.Text
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.RedoComment),
//                DataType = DataType.Text
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.ReplyContent),
//                DataType = DataType.Text
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.IsReplyFiles),
//                DataType = DataType.Boolean
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.ReplyDate),
//                DataType = DataType.DateTime
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.IsOrganization),
//                DataType = DataType.Boolean
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicName),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicOrganization),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicCardType),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicCardNo),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicPhone),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicPostCode),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicAddress),
//                DataType = DataType.VarChar,
//                DataLength = 500
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicEmail),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.CivicFax),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgName),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgUnitCode),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgLegalPerson),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgLinkName),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgPhone),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgPostCode),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgAddress),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgEmail),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.OrgFax),
//                DataType = DataType.VarChar,
//                DataLength = 200
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.Title),
//                DataType = DataType.VarChar,
//                DataLength = 500
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.Content),
//                DataType = DataType.Text
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.Purpose),
//                DataType = DataType.VarChar,
//                DataLength = 500
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.IsApplyFree),
//                DataType = DataType.Boolean
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.ProvideType),
//                DataType = DataType.VarChar,
//                DataLength = 50
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.ObtainType),
//                DataType = DataType.VarChar,
//                DataLength = 50
//            },
//            new TableColumn
//            {
//                AttributeName = nameof(DataInfo.DepartmentName),
//                DataType = DataType.VarChar,
//                DataLength = 500
//            }
//        };

//        public static int Insert(DataInfo dataInfo)
//        {
//            if (dataInfo == null || string.IsNullOrEmpty(dataInfo.Title) ||
//                string.IsNullOrEmpty(dataInfo.QueryCode)) return 0;

//            var sqlString = $@"INSERT INTO {TableName}
//(
//    {nameof(DataInfo.SiteId)},
//    {nameof(DataInfo.AddDate)},
//    {nameof(DataInfo.QueryCode)},
//    {nameof(DataInfo.DepartmentId)},
//    {nameof(DataInfo.IsCompleted)},
//    {nameof(DataInfo.State)},
//    {nameof(DataInfo.DenyReason)},
//    {nameof(DataInfo.RedoComment)},
//    {nameof(DataInfo.ReplyContent)},
//    {nameof(DataInfo.IsReplyFiles)},
//    {nameof(DataInfo.ReplyDate)},
//    {nameof(DataInfo.IsOrganization)},
//    {nameof(DataInfo.CivicName)},
//    {nameof(DataInfo.CivicOrganization)},
//    {nameof(DataInfo.CivicCardType)},
//    {nameof(DataInfo.CivicCardNo)},
//    {nameof(DataInfo.CivicPhone)},
//    {nameof(DataInfo.CivicPostCode)},
//    {nameof(DataInfo.CivicAddress)},
//    {nameof(DataInfo.CivicEmail)},
//    {nameof(DataInfo.CivicFax)},
//    {nameof(DataInfo.OrgName)},
//    {nameof(DataInfo.OrgUnitCode)},
//    {nameof(DataInfo.OrgLegalPerson)},
//    {nameof(DataInfo.OrgLinkName)},
//    {nameof(DataInfo.OrgPhone)},
//    {nameof(DataInfo.OrgPostCode)},
//    {nameof(DataInfo.OrgAddress)},
//    {nameof(DataInfo.OrgEmail)},
//    {nameof(DataInfo.OrgFax)},
//    {nameof(DataInfo.Title)},
//    {nameof(DataInfo.Content)},
//    {nameof(DataInfo.Purpose)},
//    {nameof(DataInfo.IsApplyFree)},
//    {nameof(DataInfo.ProvideType)},
//    {nameof(DataInfo.ObtainType)},
//    {nameof(DataInfo.DepartmentName)}
//) VALUES (
//    @{nameof(DataInfo.SiteId)},
//    @{nameof(DataInfo.AddDate)},
//    @{nameof(DataInfo.QueryCode)},
//    @{nameof(DataInfo.DepartmentId)},
//    @{nameof(DataInfo.IsCompleted)},
//    @{nameof(DataInfo.State)},
//    @{nameof(DataInfo.DenyReason)},
//    @{nameof(DataInfo.RedoComment)},
//    @{nameof(DataInfo.ReplyContent)},
//    @{nameof(DataInfo.IsReplyFiles)},
//    @{nameof(DataInfo.ReplyDate)},
//    @{nameof(DataInfo.IsOrganization)},
//    @{nameof(DataInfo.CivicName)},
//    @{nameof(DataInfo.CivicOrganization)},
//    @{nameof(DataInfo.CivicCardType)},
//    @{nameof(DataInfo.CivicCardNo)},
//    @{nameof(DataInfo.CivicPhone)},
//    @{nameof(DataInfo.CivicPostCode)},
//    @{nameof(DataInfo.CivicAddress)},
//    @{nameof(DataInfo.CivicEmail)},
//    @{nameof(DataInfo.CivicFax)},
//    @{nameof(DataInfo.OrgName)},
//    @{nameof(DataInfo.OrgUnitCode)},
//    @{nameof(DataInfo.OrgLegalPerson)},
//    @{nameof(DataInfo.OrgLinkName)},
//    @{nameof(DataInfo.OrgPhone)},
//    @{nameof(DataInfo.OrgPostCode)},
//    @{nameof(DataInfo.OrgAddress)},
//    @{nameof(DataInfo.OrgEmail)},
//    @{nameof(DataInfo.OrgFax)},
//    @{nameof(DataInfo.Title)},
//    @{nameof(DataInfo.Content)},
//    @{nameof(DataInfo.Purpose)},
//    @{nameof(DataInfo.IsApplyFree)},
//    @{nameof(DataInfo.ProvideType)},
//    @{nameof(DataInfo.ObtainType)},
//    @{nameof(DataInfo.DepartmentName)}
//)";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), dataInfo.SiteId),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.AddDate), dataInfo.AddDate),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.QueryCode), dataInfo.QueryCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), dataInfo.DepartmentId),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsCompleted), dataInfo.IsCompleted),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), dataInfo.State),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DenyReason), dataInfo.DenyReason),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.RedoComment), dataInfo.RedoComment),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ReplyContent), dataInfo.ReplyContent),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsReplyFiles), dataInfo.IsReplyFiles),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ReplyDate), dataInfo.ReplyDate),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsOrganization), dataInfo.IsOrganization),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicName), dataInfo.CivicName),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicOrganization), dataInfo.CivicOrganization),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicCardType), dataInfo.CivicCardType),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicCardNo), dataInfo.CivicCardNo),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicPhone), dataInfo.CivicPhone),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicPostCode), dataInfo.CivicPostCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicAddress), dataInfo.CivicAddress),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicEmail), dataInfo.CivicEmail),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicFax), dataInfo.CivicFax),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgName), dataInfo.OrgName),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgUnitCode), dataInfo.OrgUnitCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgLegalPerson), dataInfo.OrgLegalPerson),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgLinkName), dataInfo.OrgLinkName),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgPhone), dataInfo.OrgPhone),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgPostCode), dataInfo.OrgPostCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgAddress), dataInfo.OrgAddress),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgEmail), dataInfo.OrgEmail),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgFax), dataInfo.OrgFax),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Title), dataInfo.Title),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Content), dataInfo.Content),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Purpose), dataInfo.Purpose),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsApplyFree), dataInfo.IsApplyFree),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ProvideType), dataInfo.ProvideType),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ObtainType), dataInfo.ObtainType),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentName), dataInfo.DepartmentName)
//            };

//            dataInfo.Id = Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(DataInfo.Id),
//                Context.ConnectionString, sqlString, parameters.ToArray());

//            LogManager.NewApplication(dataInfo);

//            DataCountManager.ClearCache(dataInfo.SiteId);

//            return dataInfo.Id;
//        }

//        public static void Update(DataInfo dataInfo)
//        {
//            var sqlString = $@"UPDATE {TableName} SET
//    {nameof(DataInfo.SiteId)} = @{nameof(DataInfo.SiteId)},
//    {nameof(DataInfo.AddDate)} = @{nameof(DataInfo.AddDate)},
//    {nameof(DataInfo.QueryCode)} = @{nameof(DataInfo.QueryCode)},
//    {nameof(DataInfo.DepartmentId)} = @{nameof(DataInfo.DepartmentId)},
//    {nameof(DataInfo.IsCompleted)} = @{nameof(DataInfo.IsCompleted)},
//    {nameof(DataInfo.State)} = @{nameof(DataInfo.State)},
//    {nameof(DataInfo.DenyReason)} = @{nameof(DataInfo.DenyReason)},
//    {nameof(DataInfo.RedoComment)} = @{nameof(DataInfo.RedoComment)},
//    {nameof(DataInfo.ReplyContent)} = @{nameof(DataInfo.ReplyContent)},
//    {nameof(DataInfo.IsReplyFiles)} = @{nameof(DataInfo.IsReplyFiles)},
//    {nameof(DataInfo.ReplyDate)} = @{nameof(DataInfo.ReplyDate)},
//    {nameof(DataInfo.IsOrganization)} = @{nameof(DataInfo.IsOrganization)},
//    {nameof(DataInfo.CivicName)} = @{nameof(DataInfo.CivicName)},
//    {nameof(DataInfo.CivicOrganization)} = @{nameof(DataInfo.CivicOrganization)},
//    {nameof(DataInfo.CivicCardType)} = @{nameof(DataInfo.CivicCardType)},
//    {nameof(DataInfo.CivicCardNo)} = @{nameof(DataInfo.CivicCardNo)},
//    {nameof(DataInfo.CivicPhone)} = @{nameof(DataInfo.CivicPhone)},
//    {nameof(DataInfo.CivicPostCode)} = @{nameof(DataInfo.CivicPostCode)},
//    {nameof(DataInfo.CivicAddress)} = @{nameof(DataInfo.CivicAddress)},
//    {nameof(DataInfo.CivicEmail)} = @{nameof(DataInfo.CivicEmail)},
//    {nameof(DataInfo.CivicFax)} = @{nameof(DataInfo.CivicFax)},
//    {nameof(DataInfo.OrgName)} = @{nameof(DataInfo.OrgName)},
//    {nameof(DataInfo.OrgUnitCode)} = @{nameof(DataInfo.OrgUnitCode)},
//    {nameof(DataInfo.OrgLegalPerson)} = @{nameof(DataInfo.OrgLegalPerson)},
//    {nameof(DataInfo.OrgLinkName)} = @{nameof(DataInfo.OrgLinkName)},
//    {nameof(DataInfo.OrgPhone)} = @{nameof(DataInfo.OrgPhone)},
//    {nameof(DataInfo.OrgPostCode)} = @{nameof(DataInfo.OrgPostCode)},
//    {nameof(DataInfo.OrgAddress)} = @{nameof(DataInfo.OrgAddress)},
//    {nameof(DataInfo.OrgEmail)} = @{nameof(DataInfo.OrgEmail)},
//    {nameof(DataInfo.OrgFax)} = @{nameof(DataInfo.OrgFax)},
//    {nameof(DataInfo.Title)} = @{nameof(DataInfo.Title)},
//    {nameof(DataInfo.Content)} = @{nameof(DataInfo.Content)},
//    {nameof(DataInfo.Purpose)} = @{nameof(DataInfo.Purpose)},
//    {nameof(DataInfo.IsApplyFree)} = @{nameof(DataInfo.IsApplyFree)},
//    {nameof(DataInfo.ProvideType)} = @{nameof(DataInfo.ProvideType)},
//    {nameof(DataInfo.ObtainType)} = @{nameof(DataInfo.ObtainType)},
//    {nameof(DataInfo.DepartmentName)} = @{nameof(DataInfo.DepartmentName)}
//WHERE Id = @Id";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), dataInfo.SiteId),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.AddDate), dataInfo.AddDate),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.QueryCode), dataInfo.QueryCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), dataInfo.DepartmentId),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsCompleted), dataInfo.IsCompleted),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), dataInfo.State),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DenyReason), dataInfo.DenyReason),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.RedoComment), dataInfo.RedoComment),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ReplyContent), dataInfo.ReplyContent),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsReplyFiles), dataInfo.IsReplyFiles),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ReplyDate), dataInfo.ReplyDate),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsOrganization), dataInfo.IsOrganization),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicName), dataInfo.CivicName),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicOrganization), dataInfo.CivicOrganization),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicCardType), dataInfo.CivicCardType),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicCardNo), dataInfo.CivicCardNo),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicPhone), dataInfo.CivicPhone),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicPostCode), dataInfo.CivicPostCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicAddress), dataInfo.CivicAddress),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicEmail), dataInfo.CivicEmail),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicFax), dataInfo.CivicFax),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgName), dataInfo.OrgName),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgUnitCode), dataInfo.OrgUnitCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgLegalPerson), dataInfo.OrgLegalPerson),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgLinkName), dataInfo.OrgLinkName),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgPhone), dataInfo.OrgPhone),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgPostCode), dataInfo.OrgPostCode),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgAddress), dataInfo.OrgAddress),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgEmail), dataInfo.OrgEmail),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgFax), dataInfo.OrgFax),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Title), dataInfo.Title),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Content), dataInfo.Content),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Purpose), dataInfo.Purpose),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsApplyFree), dataInfo.IsApplyFree),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ProvideType), dataInfo.ProvideType),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ObtainType), dataInfo.ObtainType),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentName), dataInfo.DepartmentName),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Id), dataInfo.Id)
//            };

//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
//        }

//        public static void Redo(int siteId, int dataId, string redoComment)
//        {
//            var sqlString = $@"UPDATE {TableName} SET
//    {nameof(DataInfo.State)} = @{nameof(DataInfo.State)},
//    {nameof(DataInfo.RedoComment)} = @{nameof(DataInfo.RedoComment)}
//WHERE Id = @Id";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), DataState.Redo.Value),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.RedoComment), redoComment),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Id), dataId)
//            };

//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
//        }

//        public static void Deny(int siteId, int dataId, string denyReason)
//        {
//            var sqlString = $@"UPDATE {TableName} SET
//    {nameof(DataInfo.IsCompleted)} = @{nameof(DataInfo.IsCompleted)},
//    {nameof(DataInfo.State)} = @{nameof(DataInfo.State)},
//    {nameof(DataInfo.DenyReason)} = @{nameof(DataInfo.DenyReason)}
//WHERE Id = @Id";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsCompleted), true),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), DataState.Denied.Value),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DenyReason), denyReason),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Id), dataId)
//            };

//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
//        }

//        public static void UpdateState(int siteId, int dataId, DataState state)
//        {
//            var sqlString = $@"UPDATE {TableName} SET
//    {nameof(DataInfo.IsCompleted)} = @{nameof(DataInfo.IsCompleted)},
//    {nameof(DataInfo.State)} = @{nameof(DataInfo.State)}
//WHERE Id = @Id";

//            var isCompleted = state == DataState.Checked || state == DataState.Denied;

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsCompleted), isCompleted),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), state.Value),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Id), dataId)
//            };

//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());

//            DataCountManager.ClearCache(siteId);
//        }

//        public static void UpdateStateAndDepartmentId(int siteId, int dataId, DataState state, int departmentId)
//        {
//            var sqlString = $@"UPDATE {TableName} SET 
//    {nameof(DataInfo.IsCompleted)} = @{nameof(DataInfo.IsCompleted)},
//    {nameof(DataInfo.State)} = @{nameof(DataInfo.State)}
//";

//            if (departmentId > 0)
//            {
//                sqlString += $", {nameof(DataInfo.DepartmentId)} = @{nameof(DataInfo.DepartmentId)}";
//            }

//            sqlString += " WHERE Id = @Id";

//            var isCompleted = state == DataState.Checked || state == DataState.Denied;

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsCompleted), isCompleted),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), state.Value)
//            };
//            if (departmentId > 0)
//            {
//                parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), departmentId));
//            }
//            parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.Id), dataId));

//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());

//            DataCountManager.ClearCache(siteId);
//        }

//        public static void UpdateStateAndReply(int siteId, int dataId, string replyContent, bool isReplyFiles)
//        {
//            var sqlString = $@"UPDATE {TableName} SET {nameof(DataInfo.State)} = @{nameof(DataInfo.State)}, {nameof(DataInfo.ReplyContent)} = @{nameof(DataInfo.ReplyContent)}, {nameof(DataInfo.IsReplyFiles)} = @{nameof(DataInfo.IsReplyFiles)}, {nameof(DataInfo.ReplyDate)} = @{nameof(DataInfo.ReplyDate)} WHERE Id = @Id";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), DataState.Replied.Value),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ReplyContent), replyContent),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsReplyFiles), isReplyFiles),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.ReplyDate), DateTime.Now),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Id), dataId)
//            };

//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());

//            DataCountManager.ClearCache(siteId);
//        }

//        public static void UpdateDepartmentId(int siteId, int dataId, int departmentId)
//        {
//            var sqlString = $@"UPDATE {TableName} SET {nameof(DataInfo.DepartmentId)} = @{nameof(DataInfo.DepartmentId)} WHERE Id = @Id";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), departmentId),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.Id), dataId)
//            };

//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
//        }

//        public static void Delete(int siteId, int dataId)
//        {
//            var sqlString =
//                $"DELETE FROM {TableName} WHERE {nameof(DataInfo.Id)} = {dataId}";
//            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);

//            FileDao.DeleteByDataId(dataId);
//            LogDao.DeleteByDataId(dataId);

//            DataCountManager.ClearCache(siteId);
//        }

//        private int GetIntResult(string sqlString, List<IDataParameter> parameters)
//        {
//            var count = 0;

//            using (var conn = Context.DatabaseApi.GetConnection(Context.ConnectionString))
//            {
//                conn.Open();
//                using (var rdr = Context.DatabaseApi.ExecuteReader(conn, sqlString, parameters.ToArray()))
//                {
//                    if (rdr.Read() && !rdr.IsDBNull(0))
//                    {
//                        count = rdr.GetInt32(0);
//                    }

//                    rdr.Close();
//                }
//            }

//            return count;
//        }

//        private int GetCount(int siteId, string state)
//        {
//            var sqlString =
//                $"SELECT COUNT(*) FROM {TableName} WHERE {nameof(DataInfo.SiteId)} = @{nameof(DataInfo.SiteId)} AND {nameof(DataInfo.State)} = @{nameof(DataInfo.State)}";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), siteId),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.State), state)
//            };

//            return GetIntResult(sqlString, parameters);
//        }

//        public static int GetCount(int siteId, List<DataState> stateList, string keyword, int departmentId)
//        {
//            var sqlString =
//                $"SELECT COUNT(*) FROM {TableName} WHERE {nameof(DataInfo.SiteId)} = @{nameof(DataInfo.SiteId)}";

//            if (stateList.Count == 1)
//            {
//                sqlString += $" AND {nameof(DataInfo.State)} = '{stateList[0].Value}'";
//            }
//            else if (stateList.Count > 1)
//            {
//                sqlString += " AND (";
//                for (var i = 0; i < stateList.Count; i++)
//                {
//                    var state = stateList[i];
//                    if (i == 0)
//                    {
//                        sqlString += $"{nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                    else
//                    {
//                        sqlString += $" OR {nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                }
//                sqlString += ")";
//            }

//            if (!string.IsNullOrEmpty(keyword))
//            {
//                keyword = AttackUtils.FilterSql(keyword);
//                sqlString +=
//                    $" AND ({nameof(DataInfo.Title)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgLinkName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgName)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.QueryCode)} LIKE '%{keyword}%')";
//            }

//            if (departmentId > 0)
//            {
//                sqlString += $" AND {nameof(DataInfo.DepartmentId)} = @{nameof(DataInfo.DepartmentId)}";
//            }

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), siteId)
//            };
//            if (departmentId > 0)
//            {
//                parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), departmentId));
//            }

//            return GetIntResult(sqlString, parameters);
//        }

//        public static int GetUserCount(int siteId, List<DataState> stateList, string keyword, int departmentId, List<int> userDepartmentIdList)
//        {
//            if (userDepartmentIdList.Count == 0) return 0;

//            var sqlString =
//                $"SELECT COUNT(*) FROM {TableName} WHERE {nameof(DataInfo.SiteId)} = @{nameof(DataInfo.SiteId)} AND {nameof(DataInfo.DepartmentId)} IN ({string.Join(",", userDepartmentIdList)})";

//            if (stateList.Count == 1)
//            {
//                sqlString += $" AND {nameof(DataInfo.State)} = '{stateList[0].Value}'";
//            }
//            else if (stateList.Count > 1)
//            {
//                sqlString += " AND (";
//                for (var i = 0; i < stateList.Count; i++)
//                {
//                    var state = stateList[i];
//                    if (i == 0)
//                    {
//                        sqlString += $"{nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                    else
//                    {
//                        sqlString += $" OR {nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                }
//                sqlString += ")";
//            }

//            if (!string.IsNullOrEmpty(keyword))
//            {
//                keyword = AttackUtils.FilterSql(keyword);
//                sqlString +=
//                    $" AND ({nameof(DataInfo.Title)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgLinkName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgName)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.QueryCode)} LIKE '%{keyword}%')";
//            }

//            if (departmentId > 0)
//            {
//                sqlString += $" AND {nameof(DataInfo.DepartmentId)} = @{nameof(DataInfo.DepartmentId)}";
//            }

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), siteId)
//            };
//            if (departmentId > 0)
//            {
//                parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), departmentId));
//            }

//            return GetIntResult(sqlString, parameters);
//        }

//        //public static List<DataInfo> GetDataInfoList(int siteId, string state, int page)
//        //{
//        //    List<DataInfo> dataInfoList;
//        //    var totalCount = DataCountManager.GetCount(siteId, state);
//        //    if (totalCount == 0)
//        //    {
//        //        dataInfoList = new List<DataInfo>();
//        //    }
//        //    else if (totalCount <= ApplicationUtils.PageSize)
//        //    {
//        //        dataInfoList = GetDataInfoList(siteId, state, 0, totalCount);
//        //    }
//        //    else
//        //    {
//        //        if (page == 0) page = 1;
//        //        var offset = (page - 1) * ApplicationUtils.PageSize;
//        //        var limit = totalCount - offset > ApplicationUtils.PageSize
//        //            ? ApplicationUtils.PageSize
//        //            : totalCount - offset;
//        //        dataInfoList = GetDataInfoList(siteId, state, offset, limit);
//        //    }

//        //    return dataInfoList;
//        //}

//        public static List<DataInfo> GetDataInfoList(int siteId, List<DataState> stateList, string keyword, int departmentId, int offset, int limit)
//        {
//            var dataInfoList = new List<DataInfo>();

//            var whereString = $"WHERE {nameof(DataInfo.SiteId)} = @{nameof(DataInfo.SiteId)}";

//            if (stateList.Count == 1)
//            {
//                whereString += $" AND {nameof(DataInfo.State)} = '{stateList[0].Value}'";
//            }
//            else if (stateList.Count > 1)
//            {
//                whereString += " AND (";
//                for (var i = 0; i < stateList.Count; i++)
//                {
//                    var state = stateList[i];
//                    if (i == 0)
//                    {
//                        whereString += $"{nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                    else
//                    {
//                        whereString += $" OR {nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                }
//                whereString += ")";
//            }

//            if (!string.IsNullOrEmpty(keyword))
//            {
//                keyword = AttackUtils.FilterSql(keyword);
//                whereString +=
//                    $" AND ({nameof(DataInfo.Title)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgLinkName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgName)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.QueryCode)} LIKE '%{keyword}%')";
//            }
//            if (departmentId > 0)
//            {
//                whereString += $" AND {nameof(DataInfo.DepartmentId)} = @{nameof(DataInfo.DepartmentId)}";
//            }

//            var sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $@"{nameof(DataInfo.Id)},
//{nameof(DataInfo.SiteId)},
//{nameof(DataInfo.AddDate)},
//{nameof(DataInfo.QueryCode)},
//{nameof(DataInfo.DepartmentId)},
//{nameof(DataInfo.IsCompleted)},
//{nameof(DataInfo.State)},
//{nameof(DataInfo.DenyReason)},
//{nameof(DataInfo.RedoComment)},
//{nameof(DataInfo.ReplyContent)},
//{nameof(DataInfo.IsReplyFiles)},
//{nameof(DataInfo.ReplyDate)},
//{nameof(DataInfo.IsOrganization)},
//{nameof(DataInfo.CivicName)},
//{nameof(DataInfo.CivicOrganization)},
//{nameof(DataInfo.CivicCardType)},
//{nameof(DataInfo.CivicCardNo)},
//{nameof(DataInfo.CivicPhone)},
//{nameof(DataInfo.CivicPostCode)},
//{nameof(DataInfo.CivicAddress)},
//{nameof(DataInfo.CivicEmail)},
//{nameof(DataInfo.CivicFax)},
//{nameof(DataInfo.OrgName)},
//{nameof(DataInfo.OrgUnitCode)},
//{nameof(DataInfo.OrgLegalPerson)},
//{nameof(DataInfo.OrgLinkName)},
//{nameof(DataInfo.OrgPhone)},
//{nameof(DataInfo.OrgPostCode)},
//{nameof(DataInfo.OrgAddress)},
//{nameof(DataInfo.OrgEmail)},
//{nameof(DataInfo.OrgFax)},
//{nameof(DataInfo.Title)},
//{nameof(DataInfo.Content)},
//{nameof(DataInfo.Purpose)},
//{nameof(DataInfo.IsApplyFree)},
//{nameof(DataInfo.ProvideType)},
//{nameof(DataInfo.ObtainType)},
//{nameof(DataInfo.DepartmentName)}", whereString,
//                $"ORDER BY {nameof(DataInfo.IsCompleted)}, {nameof(DataInfo.Id)}", offset, limit);

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), siteId)
//            };
//            if (departmentId > 0)
//            {
//                parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), departmentId));
//            }

//            using (var rdr =
//                Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
//            {
//                while (rdr.Read())
//                {
//                    var formLogInfo = GetDataInfo(rdr);
//                    dataInfoList.Add(formLogInfo);
//                }

//                rdr.Close();
//            }

//            return dataInfoList;
//        }

//        public static List<DataInfo> GetUserDataInfoList(int siteId, List<DataState> stateList, string keyword, int departmentId, List<int> userDepartmentIdList, int offset, int limit)
//        {
//            var dataInfoList = new List<DataInfo>();
//            if (userDepartmentIdList.Count == 0) return dataInfoList;

//            var whereString = $"WHERE {nameof(DataInfo.SiteId)} = @{nameof(DataInfo.SiteId)} AND {nameof(DataInfo.DepartmentId)} IN ({string.Join(",", userDepartmentIdList)})";

//            if (stateList.Count == 1)
//            {
//                whereString += $" AND {nameof(DataInfo.State)} = '{stateList[0].Value}'";
//            }
//            else if (stateList.Count > 1)
//            {
//                whereString += " AND (";
//                for (var i = 0; i < stateList.Count; i++)
//                {
//                    var state = stateList[i];
//                    if (i == 0)
//                    {
//                        whereString += $"{nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                    else
//                    {
//                        whereString += $" OR {nameof(DataInfo.State)} = '{state.Value}'";
//                    }
//                }
//                whereString += ")";
//            }

//            if (!string.IsNullOrEmpty(keyword))
//            {
//                keyword = AttackUtils.FilterSql(keyword);
//                whereString +=
//                    $" AND ({nameof(DataInfo.Title)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgLinkName)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgName)} LIKE '%{keyword}%' OR {nameof(DataInfo.CivicPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.OrgPhone)} LIKE '%{keyword}%' OR {nameof(DataInfo.QueryCode)} LIKE '%{keyword}%')";
//            }
//            if (departmentId > 0)
//            {
//                whereString += $" AND {nameof(DataInfo.DepartmentId)} = @{nameof(DataInfo.DepartmentId)}";
//            }

//            var sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $@"{nameof(DataInfo.Id)},
//{nameof(DataInfo.SiteId)},
//{nameof(DataInfo.AddDate)},
//{nameof(DataInfo.QueryCode)},
//{nameof(DataInfo.DepartmentId)},
//{nameof(DataInfo.IsCompleted)},
//{nameof(DataInfo.State)},
//{nameof(DataInfo.DenyReason)},
//{nameof(DataInfo.RedoComment)},
//{nameof(DataInfo.ReplyContent)},
//{nameof(DataInfo.IsReplyFiles)},
//{nameof(DataInfo.ReplyDate)},
//{nameof(DataInfo.IsOrganization)},
//{nameof(DataInfo.CivicName)},
//{nameof(DataInfo.CivicOrganization)},
//{nameof(DataInfo.CivicCardType)},
//{nameof(DataInfo.CivicCardNo)},
//{nameof(DataInfo.CivicPhone)},
//{nameof(DataInfo.CivicPostCode)},
//{nameof(DataInfo.CivicAddress)},
//{nameof(DataInfo.CivicEmail)},
//{nameof(DataInfo.CivicFax)},
//{nameof(DataInfo.OrgName)},
//{nameof(DataInfo.OrgUnitCode)},
//{nameof(DataInfo.OrgLegalPerson)},
//{nameof(DataInfo.OrgLinkName)},
//{nameof(DataInfo.OrgPhone)},
//{nameof(DataInfo.OrgPostCode)},
//{nameof(DataInfo.OrgAddress)},
//{nameof(DataInfo.OrgEmail)},
//{nameof(DataInfo.OrgFax)},
//{nameof(DataInfo.Title)},
//{nameof(DataInfo.Content)},
//{nameof(DataInfo.Purpose)},
//{nameof(DataInfo.IsApplyFree)},
//{nameof(DataInfo.ProvideType)},
//{nameof(DataInfo.ObtainType)},
//{nameof(DataInfo.DepartmentName)}", whereString,
//                $"ORDER BY {nameof(DataInfo.IsCompleted)}, {nameof(DataInfo.Id)}", offset, limit);

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), siteId)
//            };
//            if (departmentId > 0)
//            {
//                parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.DepartmentId), departmentId));
//            }

//            using (var rdr =
//                Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
//            {
//                while (rdr.Read())
//                {
//                    var formLogInfo = GetDataInfo(rdr);
//                    dataInfoList.Add(formLogInfo);
//                }

//                rdr.Close();
//            }

//            return dataInfoList;
//        }

//        public static DataInfo GetDataInfo(int dataId)
//        {
//            DataInfo dataInfo = null;

//            var sqlString =
//                $@"SELECT {nameof(DataInfo.Id)},
//    {nameof(DataInfo.SiteId)},
//    {nameof(DataInfo.AddDate)},
//    {nameof(DataInfo.QueryCode)},
//    {nameof(DataInfo.DepartmentId)},
//    {nameof(DataInfo.IsCompleted)},
//    {nameof(DataInfo.State)},
//    {nameof(DataInfo.DenyReason)},
//    {nameof(DataInfo.RedoComment)},
//    {nameof(DataInfo.ReplyContent)},
//    {nameof(DataInfo.IsReplyFiles)},
//    {nameof(DataInfo.ReplyDate)},
//    {nameof(DataInfo.IsOrganization)},
//    {nameof(DataInfo.CivicName)},
//    {nameof(DataInfo.CivicOrganization)},
//    {nameof(DataInfo.CivicCardType)},
//    {nameof(DataInfo.CivicCardNo)},
//    {nameof(DataInfo.CivicPhone)},
//    {nameof(DataInfo.CivicPostCode)},
//    {nameof(DataInfo.CivicAddress)},
//    {nameof(DataInfo.CivicEmail)},
//    {nameof(DataInfo.CivicFax)},
//    {nameof(DataInfo.OrgName)},
//    {nameof(DataInfo.OrgUnitCode)},
//    {nameof(DataInfo.OrgLegalPerson)},
//    {nameof(DataInfo.OrgLinkName)},
//    {nameof(DataInfo.OrgPhone)},
//    {nameof(DataInfo.OrgPostCode)},
//    {nameof(DataInfo.OrgAddress)},
//    {nameof(DataInfo.OrgEmail)},
//    {nameof(DataInfo.OrgFax)},
//    {nameof(DataInfo.Title)},
//    {nameof(DataInfo.Content)},
//    {nameof(DataInfo.Purpose)},
//    {nameof(DataInfo.IsApplyFree)},
//    {nameof(DataInfo.ProvideType)},
//    {nameof(DataInfo.ObtainType)},
//    {nameof(DataInfo.DepartmentName)} FROM {TableName} WHERE {nameof(DataInfo.Id)} = {dataId}";

//            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
//            {
//                if (rdr.Read())
//                {
//                    dataInfo = GetDataInfo(rdr);
//                }

//                rdr.Close();
//            }

//            return dataInfo;
//        }

//        public static DataInfo Query(int siteId, bool isOrganization, string civicName, string orgName, string queryCode)
//        {
//            DataInfo dataInfo = null;

//            var sqlString = $@"SELECT {nameof(DataInfo.Id)},
//{nameof(DataInfo.SiteId)},
//{nameof(DataInfo.AddDate)},
//{nameof(DataInfo.QueryCode)},
//{nameof(DataInfo.DepartmentId)},
//{nameof(DataInfo.IsCompleted)},
//{nameof(DataInfo.State)},
//{nameof(DataInfo.DenyReason)},
//{nameof(DataInfo.RedoComment)},
//{nameof(DataInfo.ReplyContent)},
//{nameof(DataInfo.IsReplyFiles)},
//{nameof(DataInfo.ReplyDate)},
//{nameof(DataInfo.IsOrganization)},
//{nameof(DataInfo.CivicName)},
//{nameof(DataInfo.CivicOrganization)},
//{nameof(DataInfo.CivicCardType)},
//{nameof(DataInfo.CivicCardNo)},
//{nameof(DataInfo.CivicPhone)},
//{nameof(DataInfo.CivicPostCode)},
//{nameof(DataInfo.CivicAddress)},
//{nameof(DataInfo.CivicEmail)},
//{nameof(DataInfo.CivicFax)},
//{nameof(DataInfo.OrgName)},
//{nameof(DataInfo.OrgUnitCode)},
//{nameof(DataInfo.OrgLegalPerson)},
//{nameof(DataInfo.OrgLinkName)},
//{nameof(DataInfo.OrgPhone)},
//{nameof(DataInfo.OrgPostCode)},
//{nameof(DataInfo.OrgAddress)},
//{nameof(DataInfo.OrgEmail)},
//{nameof(DataInfo.OrgFax)},
//{nameof(DataInfo.Title)},
//{nameof(DataInfo.Content)},
//{nameof(DataInfo.Purpose)},
//{nameof(DataInfo.IsApplyFree)},
//{nameof(DataInfo.ProvideType)},
//{nameof(DataInfo.ObtainType)},
//{nameof(DataInfo.DepartmentName)}
//FROM {TableName}
//WHERE {nameof(DataInfo.SiteId)} = @{nameof(DataInfo.SiteId)} AND {nameof(DataInfo.IsOrganization)} = @{nameof(DataInfo.IsOrganization)} AND {nameof(DataInfo.QueryCode)} = @{nameof(DataInfo.QueryCode)}";

//            var parameters = new List<IDataParameter>
//            {
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.SiteId), siteId),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.IsOrganization), isOrganization),
//                Context.DatabaseApi.GetParameter(nameof(DataInfo.QueryCode), queryCode)
//            };
//            if (isOrganization)
//            {
//                sqlString += $" AND {nameof(DataInfo.OrgName)} = @{nameof(DataInfo.OrgName)}";
//                parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.OrgName), orgName));
//            }
//            else
//            {
//                sqlString += $" AND {nameof(DataInfo.CivicName)} = @{nameof(DataInfo.CivicName)}";
//                parameters.Add(Context.DatabaseApi.GetParameter(nameof(DataInfo.CivicName), civicName));
//            }

//            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
//            {
//                if (rdr.Read())
//                {
//                    dataInfo = GetDataInfo(rdr);
//                }

//                rdr.Close();
//            }

//            return dataInfo;
//        }

//        public static Dictionary<string, int> GetDataCounts(int siteId)
//        {
//            var dict = new Dictionary<string, int>();
//            foreach (var state in DataState.AllStates)
//            {
//                dict[state.Value] = GetCount(siteId, state.Value);
//            }

//            return dict;
//        }

//        private DataInfo GetDataInfo(IDataRecord rdr)
//        {
//            if (rdr == null) return null;

//            var dataInfo = new DataInfo();

//            var i = 0;
//            dataInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
//            i++;
//            dataInfo.SiteId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
//            i++;
//            dataInfo.AddDate = rdr.IsDBNull(i) ? DateTime.Now : rdr.GetDateTime(i);
//            i++;
//            dataInfo.QueryCode = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.DepartmentId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
//            i++;
//            dataInfo.IsCompleted = !rdr.IsDBNull(i) && rdr.GetBoolean(i);
//            i++;
//            dataInfo.State = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.DenyReason = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.RedoComment = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.ReplyContent = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.IsReplyFiles = !rdr.IsDBNull(i) && rdr.GetBoolean(i);
//            i++;
//            dataInfo.ReplyDate = rdr.IsDBNull(i) ? DateTime.Now : rdr.GetDateTime(i);
//            i++;
//            dataInfo.IsOrganization = !rdr.IsDBNull(i) && rdr.GetBoolean(i);
//            i++;
//            dataInfo.CivicName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicOrganization = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicCardType = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicCardNo = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicPhone = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicPostCode = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicAddress = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicEmail = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.CivicFax = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgUnitCode = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgLegalPerson = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgLinkName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgPhone = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgPostCode = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgAddress = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgEmail = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.OrgFax = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.Title = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.Content = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.Purpose = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.IsApplyFree = !rdr.IsDBNull(i) && rdr.GetBoolean(i);
//            i++;
//            dataInfo.ProvideType = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.ObtainType = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
//            i++;
//            dataInfo.DepartmentName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);

//            return dataInfo;
//        }
//    }
//}
