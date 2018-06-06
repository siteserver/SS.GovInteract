using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.GovInteract.Model;

namespace SS.GovInteract.Core
{
    public static class Utils
    {
        public const string Ellipsis = "...";

        public static string GetMessageHtml(string message, bool isSuccess)
        {
            return isSuccess
                ? $@"<div class=""alert alert-primary"" role=""alert"">{message}</div>"
                : $@"<div class=""alert alert-danger"" role=""alert"">{message}</div>";
        }

        public static void SelectListItems(ListControl listControl, params string[] values)
        {
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    item.Selected = false;
                }
                foreach (ListItem item in listControl.Items)
                {
                    foreach (var value in values)
                    {
                        if (string.Equals(item.Value, value))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        public static void SelectSingleItem(ListControl listControl, string value)
        {
            if (listControl == null) return;

            listControl.ClearSelection();

            foreach (ListItem item in listControl.Items)
            {
                if (string.Equals(item.Value, value))
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        public static void SelectSingleItemIgnoreCase(ListControl listControl, string value)
        {
            if (listControl == null) return;

            listControl.ClearSelection();
            foreach (ListItem item in listControl.Items)
            {
                if (EqualsIgnoreCase(item.Value, value))
                {
                    item.Selected = true;
                    break;
                }
            }
        } 

        /// <summary>
        /// 将数字集合转化为可供Sql语句查询的In()条件，如将集合{2,3,4}转化为字符串"2,3,4"。
        /// </summary>
        /// <param name="collection">非数字的集合</param>
        /// <returns>可供Sql语句查询的In()条件字符串，各元素不使用单引号包围</returns>
        public static string ToSqlInStringWithoutQuote(ICollection collection)
        {
            var builder = new StringBuilder();
            if (collection != null)
            {
                foreach (var obj in collection)
                {
                    builder.Append(obj).Append(",");
                }
                if (builder.Length != 0) builder.Remove(builder.Length - 1, 1);
            }
            return builder.Length == 0 ? "null" : builder.ToString();
        }

        public static List<string> StringCollectionToStringList(string collection)
        {
            return StringCollectionToStringList(collection, ',');
        }

        public static List<string> StringCollectionToStringList(string collection, char split)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(collection))
            {
                var array = collection.Split(split);
                foreach (var s in array)
                {
                    list.Add(s);
                }
            }
            return list;
        }

        public static string GetControlRenderHtml(Control control)
        {
            var builder = new StringBuilder();
            if (control != null)
            {
                var sw = new System.IO.StringWriter(builder);
                var htw = new HtmlTextWriter(sw);
                control.RenderControl(htw);
            }
            return builder.ToString();
        }

        public static string GetUrlWithoutQueryString(string rawUrl)
        {
            string urlWithoutQueryString;
            if (rawUrl != null && rawUrl.IndexOf("?", StringComparison.Ordinal) != -1)
            {
                var queryString = rawUrl.Substring(rawUrl.IndexOf("?", StringComparison.Ordinal));
                urlWithoutQueryString = rawUrl.Replace(queryString, "");
            }
            else
            {
                urlWithoutQueryString = rawUrl;
            }
            return urlWithoutQueryString;
        }

        public static string AddQueryString(string url, NameValueCollection queryString)
        {
            if (queryString == null || url == null || queryString.Count == 0)
                return url;

            var builder = new StringBuilder();
            foreach (string key in queryString.Keys)
            {
                builder.Append($"&{key}={HttpUtility.UrlEncode(queryString[key])}");
            }
            if (url.IndexOf("?", StringComparison.Ordinal) == -1)
            {
                if (builder.Length > 0) builder.Remove(0, 1);
                return string.Concat(url, "?", builder.ToString());
            }
            if (url.EndsWith("?"))
            {
                if (builder.Length > 0) builder.Remove(0, 1);
            }
            return string.Concat(url, builder.ToString());
        }

        public static bool EqualsIgnoreCase(string a, string b)
        {
            if (a == b) return true;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            return string.Equals(a.Trim().ToLower(), b.Trim().ToLower());
        }

        public static object Eval(object dataItem, string name)
        {
            object o = null;
            try
            {
                o = DataBinder.Eval(dataItem, name);
            }
            catch
            {
                // ignored
            }
            if (o == DBNull.Value)
            {
                o = null;
            }
            return o;
        }

        public static int EvalInt(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o == null ? 0 : Convert.ToInt32(o);
        }

        public static string EvalString(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o?.ToString() ?? string.Empty;
        }

        public static DateTime EvalDateTime(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            if (o == null)
            {
                return DateTime.MinValue;
            }
            return (DateTime)o;
        }

        public static string ReplaceNewline(string inputString, string replacement)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;
            var retVal = new StringBuilder();
            inputString = inputString.Trim();
            foreach (var t in inputString)
            {
                switch (t)
                {
                    case '\n':
                        retVal.Append(replacement);
                        break;
                    case '\r':
                        break;
                    default:
                        retVal.Append(t);
                        break;
                }
            }
            return retVal.ToString();
        }  

        public static string SwalSuccess(string title, string text, string button, string scripts)
        {
            if (!string.IsNullOrEmpty(scripts))
            {
                scripts = $@".then(function (value) {{
  {scripts}
}})";
            }
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'success',
  button: '{button}',
}}){scripts};";
            return script;
        }

        public static string GetOpenLayerString(string title, string pageUrl, int width, int height)
        {
            string areaWidth = $"'{width}px'";
            string areaHeight = $"'{height}px'";
            var offsetLeft = "''";
            var offsetRight = "''";
            if (width == 0)
            {
                areaWidth = "($(window).width() - 50) +'px'";
                offsetRight = "'25px'";
            }
            if (height == 0)
            {
                areaHeight = "($(window).height() - 50) +'px'";
                offsetLeft = "'25px'";
            }
            return
                $@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}'}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});return false;";
        }

        public static void CloseModalPage(Page page)
        {
            page.Response.Clear();
            page.Response.Write($"<script>window.parent.location.reload(false);{HidePopWin}</script>");
            //page.Response.End();
        }

        public const string HidePopWin = "window.parent.layer.closeAll();";

        /// <summary> 
        /// 过滤sql攻击脚本 
        /// </summary> 
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

        public static int ToInt(string str, int defaultVal = 0)
        {
            int i;
            return int.TryParse(str, out i) ? i : defaultVal;
        } 

        public static bool ToBool(string str)
        {
            bool i;
            return bool.TryParse(str, out i) && i;
        }

        public static DateTime ToDateTime(string dateTimeStr)
        {
            return ToDateTime(dateTimeStr, DateTime.Now);
        }

        public static DateTime ToDateTime(string dateTimeStr, DateTime defaultValue)
        {
            var datetime = defaultValue;
            if (!string.IsNullOrEmpty(dateTimeStr))
            {
                if (!DateTime.TryParse(dateTimeStr.Trim(), out datetime))
                {
                    datetime = defaultValue;
                }
                return datetime;
            }
            if (datetime <= DateTime.MinValue)
            {
                datetime = DateTime.Now;
            }
            return datetime;
        }

        public static string MaxLengthText(string inputString, int maxLength)
        {
            return MaxLengthText(inputString, maxLength, Ellipsis);
        }

        public static string MaxLengthText(string inputString, int maxLength, string endString)
        {
            var retval = inputString;
            try
            {
                if (maxLength > 0)
                {
                    var decodedInputString = HttpUtility.HtmlDecode(retval);
                    retval = decodedInputString;

                    var totalLength = maxLength * 2;
                    var length = 0;
                    var builder = new StringBuilder();

                    var isOneBytesChar = false;
                    var lastChar = ' ';

                    if (retval != null)
                    {
                        foreach (var singleChar in retval.ToCharArray())
                        {
                            builder.Append(singleChar);

                            if (IsTwoBytesChar(singleChar))
                            {
                                length += 2;
                                if (length >= totalLength)
                                {
                                    lastChar = singleChar;
                                    break;
                                }
                            }
                            else
                            {
                                length += 1;
                                if (length == totalLength)
                                {
                                    isOneBytesChar = true;//已经截取到需要的字数，再多截取一位
                                }
                                else if (length > totalLength)
                                {
                                    lastChar = singleChar;
                                    break;
                                }
                                else
                                {
                                    isOneBytesChar = !isOneBytesChar;
                                }
                            }
                        }
                        if (isOneBytesChar && length > totalLength)
                        {
                            builder.Length--;
                            var theStr = builder.ToString();
                            retval = builder.ToString();
                            if (char.IsLetter(lastChar))
                            {
                                for (var i = theStr.Length - 1; i > 0; i--)
                                {
                                    var theChar = theStr[i];
                                    if (!IsTwoBytesChar(theChar) && char.IsLetter(theChar))
                                    {
                                        retval = retval.Substring(0, i - 1);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                //int index = retval.LastIndexOfAny(new char[] { ' ', '\t', '\n', '\v', '\f', '\r', '\x0085' });
                                //if (index != -1)
                                //{
                                //    retval = retval.Substring(0, index);
                                //}
                            }
                        }
                        else
                        {
                            retval = builder.ToString();
                        }

                        var isCut = decodedInputString != retval;
                        retval = HttpUtility.HtmlEncode(retval);

                        if (isCut && endString != null)
                        {
                            retval += endString;
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            return retval;
        }

        public static bool IsTwoBytesChar(char chr)
        {
            // 使用中文支持编码
            return ECharsetUtils.GB2312.GetByteCount(new[] { chr }) == 2;
        }

        public static string GetIpAddress()
        {
            //取CDN用户真实IP的方法
            //当用户使用代理时，取到的是代理IP
            var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(result))
            {
                //可能有代理
                if (result.IndexOf(".", StringComparison.Ordinal) == -1)
                    result = null;
                else
                {
                    if (result.IndexOf(",", StringComparison.Ordinal) != -1)
                    {
                        result = result.Replace("  ", "").Replace("'", "");
                        var temparyip = result.Split(",;".ToCharArray());
                        foreach (var t in temparyip)
                        {
                            if (IsIp(t) && t.Substring(0, 3) != "10." && t.Substring(0, 7) != "192.168" && t.Substring(0, 7) != "172.16.")
                            {
                                result = t;
                            }
                        }
                        var str = result.Split(',');
                        if (str.Length > 0)
                            result = str[0].Trim();
                    }
                    else if (IsIp(result))
                        return result;
                }
            }

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result))
                result = "localhost";

            if (result == "::1" || result == "127.0.0.1")
            {
                result = "localhost";
            }

            return result;
        }

        public static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static List<int> StringCollectionToIntList(string collection)
        {
            var list = new List<int>();
            if (!string.IsNullOrEmpty(collection))
            {
                var array = collection.Split(',');
                foreach (var s in array)
                {
                    int i;
                    int.TryParse(s.Trim(), out i);
                    list.Add(i);
                }
            }
            return list;
        }

        public static string ObjectCollectionToString(ICollection collection)
        {
            return ObjectCollectionToString(collection, ",");
        }

        public static string ObjectCollectionToString(ICollection collection, string separatorStr)
        {
            var builder = new StringBuilder();
            if (collection != null)
            {
                foreach (var obj in collection)
                {
                    builder.Append(obj.ToString().Trim()).Append(separatorStr);
                }
                if (builder.Length != 0) builder.Remove(builder.Length - separatorStr.Length, separatorStr.Length);
            }
            return builder.ToString();
        }

        public static int GetStartCount(char startChar, string content)
        {
            if (content == null)
            {
                return 0;
            }
            var count = 0;

            foreach (var theChar in content)
            {
                if (theChar == startChar)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        public static bool Contains(string text, string inner)
        {
            return text?.IndexOf(inner, StringComparison.Ordinal) >= 0;
        }

        public static string GetRedirectStringWithCheckBoxValueAndAlert(string redirectUrl, string checkBoxServerId, string checkBoxClientId, string emptyAlertText, string confirmAlertText)
        {
            return
                $@"_confirmCheckBoxCollection(document.getElementsByName('{checkBoxClientId}'), '{emptyAlertText}', '{confirmAlertText}', '{redirectUrl}' + '&{checkBoxServerId}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxClientId}')));return false;";
        }

        public static string GetDateAndTimeString(DateTime datetime, EDateFormatType dateFormat, ETimeFormatType timeFormat)
        {
            return $"{GetDateString(datetime, dateFormat)} {GetTimeString(datetime, timeFormat)}";
        }

        public static string GetDateAndTimeString(DateTime datetime)
        {
            return GetDateAndTimeString(datetime, EDateFormatType.Day, ETimeFormatType.ShortTime);
        }

        public static string GetDateString(DateTime datetime, EDateFormatType dateFormat)
        {
            var format = string.Empty;
            if (dateFormat == EDateFormatType.Year)
            {
                format = "yyyy年MM月";
            }
            else if (dateFormat == EDateFormatType.Month)
            {
                format = "MM月dd日";
            }
            else if (dateFormat == EDateFormatType.Day)
            {
                format = "yyyy-MM-dd";
            }
            else if (dateFormat == EDateFormatType.Chinese)
            {
                format = "yyyy年M月d日";
            }
            return datetime.ToString(format);
        }

        public static string GetTimeString(DateTime datetime, ETimeFormatType timeFormat)
        {
            var retval = string.Empty;
            if (timeFormat == ETimeFormatType.LongTime)
            {
                retval = datetime.ToLongTimeString();
            }
            else if (timeFormat == ETimeFormatType.ShortTime)
            {
                retval = datetime.ToShortTimeString();
            }
            return retval;
        }

        public static void Redirect(string url)
        {
            var response = HttpContext.Current.Response;
            response.Clear();//这里是关键，清除在返回前已经设置好的标头信息，这样后面的跳转才不会报错
            response.BufferOutput = true;//设置输出缓冲
            if (!response.IsRequestBeingRedirected) //在跳转之前做判断,防止重复
            {
                response.Redirect(url, true);
            }
        }
    }
}
