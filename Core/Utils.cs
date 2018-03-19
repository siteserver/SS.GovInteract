using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SS.GovInteract.Model;

namespace SS.GovInteract.Core
{
    public class Utils
    {
        public const string ReturnAndNewline = "\r\n";//回车换行
        public const string Html5Empty = @"<html><head><meta charset=""utf-8""></head><body></body></html>";

        public const string Ellipsis = "...";

        public static string GetMessageHtml(string message, bool isSuccess)
        {
            return isSuccess
                ? $@"<div class=""alert alert-primary"" role=""alert"">{message}</div>"
                : $@"<div class=""alert alert-danger"" role=""alert"">{message}</div>";
        }

        public static string GetSelectedListControlValueCollection(ListControl listControl)
        {
            var list = new List<string>();
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    if (item.Selected)
                    {
                        list.Add(item.Value);
                    }
                }
            }
            return string.Join(",", list);
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

        public static string GetSelectOptionText(string text, int parentsCount, bool isLastNode, bool[] isLastNodeArray)
        {
            var retval = string.Empty;
            if (isLastNode == false)
            {
                isLastNodeArray[parentsCount] = false;
            }
            else
            {
                isLastNodeArray[parentsCount] = true;
            }
            for (var i = 0; i < parentsCount; i++)
            {
                retval = string.Concat(retval, isLastNodeArray[i] ? "　" : "│");
            }
            retval = string.Concat(retval, isLastNode ? "└" : "├");
            retval = string.Concat(retval, text);

            return retval;
        }

        public static void AddListItems(ListControl listControl, string trueText, string falseText)
        {
            if (listControl != null)
            {
                var item = new ListItem(trueText, true.ToString());
                listControl.Items.Add(item);
                item = new ListItem(falseText, false.ToString());
                listControl.Items.Add(item);
            }
        }

        public static void AddListItems(ListControl listControl)
        {
            AddListItems(listControl, "是", "否");
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

        public static string ToStringWithQuote(List<string> collection)
        {
            var builder = new StringBuilder();
            if (collection != null)
            {
                foreach (var obj in collection)
                {
                    builder.Append("'").Append(obj).Append("'").Append(",");
                }
                if (builder.Length != 0) builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
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

        public static string ReplaceNewlineToBr(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;
            var retVal = new StringBuilder();
            inputString = inputString.Trim();
            foreach (var t in inputString)
            {
                switch (t)
                {
                    case '\n':
                        retVal.Append("<br />");
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

        public static string HtmlDecode(string inputString)
        {
            return HttpUtility.HtmlDecode(inputString);
        }

        public static string HtmlEncode(string inputString)
        {
            return HttpUtility.HtmlEncode(inputString);
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

        public static string GetTopSqlString(string databaseType, string tableName, string columns, string whereAndOrder, int topN)
        {
            if (topN > 0)
            {
                return EqualsIgnoreCase(databaseType, "MySql") ? $"SELECT {columns} FROM {tableName} {whereAndOrder} LIMIT {topN}" : $"SELECT TOP {topN} {columns} FROM {tableName} {whereAndOrder}";
            }
            return $"SELECT {columns} FROM {tableName} {whereAndOrder}";
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

        public static decimal EvalDecimal(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o == null ? 0 : Convert.ToDecimal(o);
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

        public static bool EvalBool(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o != null && Convert.ToBoolean(o.ToString());
        }

        public static List<string> GetHtmlFormElements(string content)
        {
            var list = new List<string>();

            const RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase;

            var regex = "<input\\s*[^>]*?/>|<input\\s*[^>]*?>[^>]*?</input>";
            var reg = new Regex(regex, options);
            var mc = reg.Matches(content);
            for (var i = 0; i < mc.Count; i++)
            {
                var element = mc[i].Value;
                list.Add(element);
            }

            regex = "<textarea\\s*[^>]*?/>|<textarea\\s*[^>]*?>[^>]*?</textarea>";
            reg = new Regex(regex, options);
            mc = reg.Matches(content);
            for (var i = 0; i < mc.Count; i++)
            {
                var element = mc[i].Value;
                list.Add(element);
            }

            regex = "<select\\b[\\s\\S]*?</select>";
            reg = new Regex(regex, options);
            mc = reg.Matches(content);
            for (var i = 0; i < mc.Count; i++)
            {
                var element = mc[i].Value;
                list.Add(element);
            }

            return list;
        }

        private const string XmlDeclaration = "<?xml version='1.0'?>";

        private const string XmlNamespaceStart = "<root>";

        private const string XmlNamespaceEnd = "</root>";

        public static XmlDocument GetXmlDocument(string element, bool isXml)
        {
            var xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true
            };
            try
            {
                if (isXml)
                {
                    xmlDocument.LoadXml(XmlDeclaration + XmlNamespaceStart + element + XmlNamespaceEnd);
                }
                else
                {
                    xmlDocument.LoadXml(XmlDeclaration + XmlNamespaceStart + Main.Instance.ParseApi.HtmlToXml(element) + XmlNamespaceEnd);
                }
            }
            catch
            {
                // ignored
            }
            //catch(Exception e)
            //{
            //    TraceUtils.Warn(e.ToString());
            //    throw e;
            //}
            return xmlDocument;
        }

        public static void ParseHtmlElement(string htmlElement, out string tagName, out string innerXml, out NameValueCollection attributes)
        {
            tagName = string.Empty;
            innerXml = string.Empty;
            attributes = new NameValueCollection();

            var document = GetXmlDocument(htmlElement, false);
            XmlNode elementNode = document.DocumentElement;
            if (elementNode == null) return;

            elementNode = elementNode.FirstChild;
            tagName = elementNode.Name;
            innerXml = elementNode.InnerXml;
            if (elementNode.Attributes == null) return;

            var elementIe = elementNode.Attributes.GetEnumerator();
            while (elementIe.MoveNext())
            {
                var attr = (XmlAttribute)elementIe.Current;
                if (attr != null)
                {
                    var attributeName = attr.Name;
                    attributes.Add(attributeName, attr.Value);
                }
            }
        }

        public static string GetHtmlElementById(string html, string id)
        {
            const RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase;

            var regex = $"<input\\s*[^>]*?id\\s*=\\s*(\"{id}\"|\'{id}\'|{id}).*?>";
            var reg = new Regex(regex, options);
            var match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<\\w+\\s*[^>]*?id\\s*=\\s*(\"{id}\"|\'{id}\'|{id})[^>]*/\\s*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<(\\w+?)\\s*[^>]*?id\\s*=\\s*(\"{id}\"|\'{id}\'|{id}).*?>[^>]*</\\1[^>]*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

        public static string GetHtmlElementByRole(string html, string role)
        {
            const RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase;

            var regex = $"<input\\s*[^>]*?role\\s*=\\s*(\"{role}\"|\'{role}\'|{role}).*?>";
            var reg = new Regex(regex, options);
            var match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<\\w+\\s*[^>]*?role\\s*=\\s*(\"{role}\"|\'{role}\'|{role})[^>]*/\\s*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<(\\w+?)\\s*[^>]*?role\\s*=\\s*(\"{role}\"|\'{role}\'|{role}).*?>[^>]*</\\1[^>]*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

        public static void RewriteSubmitButton(StringBuilder builder, string clickString)
        {
            var submitElement = GetHtmlElementByRole(builder.ToString(), "submit");
            if (string.IsNullOrEmpty(submitElement))
            {
                submitElement = GetHtmlElementById(builder.ToString(), "submit");
            }
            if (!string.IsNullOrEmpty(submitElement))
            {
                var document = GetXmlDocument(submitElement, false);
                XmlNode elementNode = document.DocumentElement;
                if (elementNode != null)
                {
                    elementNode = elementNode.FirstChild;
                    if (elementNode.Attributes != null)
                    {
                        var elementIe = elementNode.Attributes.GetEnumerator();
                        var attributes = new StringDictionary();
                        while (elementIe.MoveNext())
                        {
                            var attr = (XmlAttribute)elementIe.Current;
                            if (attr != null)
                            {
                                var attributeName = attr.Name.ToLower();
                                if (attributeName == "href")
                                {
                                    attributes.Add(attr.Name, "javascript:;");
                                }
                                else if (attributeName != "onclick")
                                {
                                    attributes.Add(attr.Name, attr.Value);
                                }
                            }
                        }
                        attributes.Add("onclick", clickString);
                        attributes.Remove("id");
                        attributes.Remove("name");

                        //attributes.Add("id", "submit_" + styleID);

                        if (EqualsIgnoreCase(elementNode.Name, "a"))
                        {
                            attributes.Remove("href");
                            attributes.Add("href", "javascript:;");
                        }

                        if (!string.IsNullOrEmpty(elementNode.InnerXml))
                        {
                            builder.Replace(submitElement,
                                $@"<{elementNode.Name} {ToAttributesString(attributes)}>{elementNode.InnerXml}</{elementNode
                                    .Name}>");
                        }
                        else
                        {
                            builder.Replace(submitElement,
                                $@"<{elementNode.Name} {ToAttributesString(attributes)}/>");
                        }
                    }
                }
            }
        }

        public static string ToAttributesString(NameValueCollection attributes)
        {
            var builder = new StringBuilder();
            if (attributes != null && attributes.Count > 0)
            {
                foreach (string key in attributes.Keys)
                {
                    var value = attributes[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Replace("\"", "'");
                    }
                    builder.Append($@"{key}=""{value}"" ");
                }
                builder.Length--;
            }
            return builder.ToString();
        }

        public static string ToAttributesString(StringDictionary attributes)
        {
            var builder = new StringBuilder();
            if (attributes != null && attributes.Count > 0)
            {
                foreach (string key in attributes.Keys)
                {
                    var value = attributes[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Replace("\"", "'");
                    }
                    builder.Append($@"{key}=""{value}"" ");
                }
                builder.Length--;
            }
            return builder.ToString();
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

        public static string SwalError(string title, string text)
        {
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'error',
  button: '关 闭',
}});";

            return script;
        }

        public static string SwalSuccess(string title, string text)
        {
            return SwalSuccess(title, text, "关 闭", null);
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

        public static string SwalWarning(string title, string text, string btnCancel, string btnSubmit, string scripts)
        {
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'warning',
  buttons: {{
    cancel: '{btnCancel}',
    catch: '{btnSubmit}'
  }}
}})
.then(function(willDelete){{
  if (willDelete) {{
    {scripts}
  }}
}});";
            return script;
        }

        public static string GetOpenLayerString(string title, string pageUrl)
        {
            return GetOpenLayerString(title, pageUrl, 0, 0);
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

        public static string GetOpenLayerStringWithTextBoxValue(string title, string pageUrl, string textBoxId)
        {
            return GetOpenLayerStringWithTextBoxValue(title, pageUrl, textBoxId, 0, 0);
        }

        public static string GetOpenLayerStringWithTextBoxValue(string title, string pageUrl, string textBoxId, int width, int height)
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
                $@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{textBoxId}=' + $('#{textBoxId}').val()}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});return false;";
        }

        public static string GetOpenLayerStringWithCheckBoxValue(string title, string pageUrl, string checkBoxId, string alertText)
        {
            return GetOpenLayerStringWithCheckBoxValue(title, pageUrl, checkBoxId, alertText, 0, 0);
        }

        public static string GetOpenLayerStringWithCheckBoxValue(string title, string pageUrl, string checkBoxId, string alertText, int width, int height)
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

            if (string.IsNullOrEmpty(alertText))
            {
                return
                    $@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{checkBoxId}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId}'))}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});return false;";
            }
            return
                $@"if (!_alertCheckBoxCollection(document.getElementsByName('{checkBoxId}'), '{alertText}')){{$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{checkBoxId}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId}'))}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});}};return false;";
        }

        public static string GetOpenLayerStringWithTwoCheckBoxValue(string title, string pageUrl, string checkBoxId1, string checkBoxId2, string alertText, int width, int height)
        {
            var offset = string.Empty;
            if (width == 0)
            {
                offset = "offset: ['0px','0px'],";
            }
            if (height == 0)
            {
                offset = "offset: ['0px','0px'],";
            }

            return
                $@"var collectionValue1 = _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId1}'));var collectionValue2 = _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId2}'));if (collectionValue1.length == 0 && collectionValue2.length == 0){{alert('{alertText}');}}else{{$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{checkBoxId1}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId1}')) + '&{checkBoxId2}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId2}'))}}, area: [{width}, {height}], {offset}}});}};return false;";
        }

        public static void SetCancelAttribute(IAttributeAccessor accessor)
        {
            accessor.SetAttribute("onclick", HidePopWin);
        }

        public static void CloseModalPage(Page page)
        {
            page.Response.Clear();
            page.Response.Write($"<script>window.parent.location.reload(false);{HidePopWin}</script>");
            //page.Response.End();
        }

        public static void CloseModalPage(Page page, string scripts)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{scripts}</script>");
            page.Response.Write($"<script>window.parent.location.reload(false);{HidePopWin}</script>");
            //page.Response.End();
        }

        public static void CloseModalPageAndRedirect(Page page, string redirectUrl)
        {
            page.Response.Clear();
            page.Response.Write($"<script>window.parent.location.href = '{redirectUrl}';{HidePopWin}</script>");
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void CloseModalPageAndRedirect(Page page, string redirectUrl, string scripts)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{scripts}</script>");
            page.Response.Write($"<script>window.parent.location.href = '{redirectUrl}';{HidePopWin}</script>");
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void CloseModalPageWithoutRefresh(Page page)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{HidePopWin}</script>");
            //page.Response.End();
        }

        public static void CloseModalPageWithoutRefresh(Page page, string scripts)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{scripts}</script>");
            page.Response.Write($"<script>{HidePopWin}</script>");
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

        public static string UnFilterSql(string objStr)
        {
            if (string.IsNullOrEmpty(objStr)) return string.Empty;

            return objStr.Replace("_sqlquote_", "'").Replace("_sqldoulbeline_", "--").Replace("_sqlleftparenthesis_", "\\(").Replace("_sqlrightparenthesis_", "\\)");
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

        public static int GetStartCount(string startString, string content)
        {
            if (content == null)
            {
                return 0;
            }
            var count = 0;

            while (true)
            {
                if (content.StartsWith(startString))
                {
                    count++;
                    content = content.Remove(0, startString.Length);
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

        public static string GetRedirectStringWithCheckBoxValue(string redirectUrl, string checkBoxServerId, string checkBoxClientId, string emptyAlertText)
        {
            return
                $@"if (!_alertCheckBoxCollection(document.getElementsByName('{checkBoxClientId}'), '{emptyAlertText}')){{_goto('{redirectUrl}' + '&{checkBoxServerId}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxClientId}')));}};return false;";
        }

        public static string GetRedirectStringWithCheckBoxValueAndAlert(string redirectUrl, string checkBoxServerId, string checkBoxClientId, string emptyAlertText, string confirmAlertText)
        {
            return
                $@"_confirmCheckBoxCollection(document.getElementsByName('{checkBoxClientId}'), '{emptyAlertText}', '{confirmAlertText}', '{redirectUrl}' + '&{checkBoxServerId}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxClientId}')));return false;";
        }

        public static string GetRedirectStringWithConfirm(string redirectUrl, string confirmString)
        {
            return $@"_confirm('{confirmString}', '{redirectUrl}');return false;";
        }

        public static string GetRedirectString(string redirectUrl)
        {
            return $@"window.location.href='{redirectUrl}';return false;";
        }

        public static string GetDateAndTimeString(DateTime datetime, EDateFormatType dateFormat, ETimeFormatType timeFormat)
        {
            return $"{GetDateString(datetime, dateFormat)} {GetTimeString(datetime, timeFormat)}";
        }

        public static string GetDateAndTimeString(DateTime datetime)
        {
            return GetDateAndTimeString(datetime, EDateFormatType.Day, ETimeFormatType.ShortTime);
        }

        public static string GetDateString(DateTime datetime)
        {
            return GetDateString(datetime, EDateFormatType.Day);
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

        public static string GetTimeString(DateTime datetime)
        {
            return GetTimeString(datetime, ETimeFormatType.ShortTime);
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
    }
}
