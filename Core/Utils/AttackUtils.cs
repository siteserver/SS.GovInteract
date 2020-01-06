using System;

namespace SS.GovInteract.Core.Utils
{
    public static class AttackUtils
    {
        public static string FilterSql(string objStr)
        {
            if (string.IsNullOrEmpty(objStr)) return string.Empty;

            var isSqlExists = false;
            const string strSql = "',--,\\(,\\)";
            var strSqls = strSql.Split(',');
            foreach (var sql in strSqls)
            {
                if (objStr.IndexOf(sql, StringComparison.Ordinal) != -1)
                {
                    isSqlExists = true;
                    break;
                }
            }
            if (isSqlExists)
            {
                return objStr.Replace("'", "_sqlquote_").Replace("--", "_sqldoulbeline_").Replace("\\(", "_sqlleftparenthesis_").Replace("\\)", "_sqlrightparenthesis_");
            }
            return objStr;
        }

        public static string UnFilterSql(string objStr)
        {
            if (string.IsNullOrEmpty(objStr)) return string.Empty;

            return objStr.Replace("_sqlquote_", "'").Replace("_sqldoulbeline_", "--").Replace("_sqlleftparenthesis_", "\\(").Replace("_sqlrightparenthesis_", "\\)");
        }
    }
}
