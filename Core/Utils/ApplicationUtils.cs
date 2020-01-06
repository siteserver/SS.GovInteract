using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;

namespace SS.GovInteract.Core.Utils
{
    public static class ApplicationUtils
    {
        public const string PluginId = "SS.GovInteract";

        public const string PageTypeAccept = "Accept";
        public const string PageTypeReply = "Reply";
        public const string PageTypeCheck = "Check";

        public const int PageSize = 30;

        public static Settings GetSettings(int siteId)
        {
            return Context.ConfigApi.GetConfig<Settings>(PluginId, siteId) ?? new Settings
            {
                IsClosed = false,
                DaysWarning = 20,
                DaysDeadline = 30,
                IsDeleteAllowed = true,
                IsSelectCategory = true,
                IsSelectDepartment = true
            };
        }

        public static string GetFieldTypeText(string fieldType)
        {
            if (fieldType == InputType.Text.Value)
            {
                return "文本框(单行)";
            }
            if (fieldType == InputType.TextArea.Value)
            {
                return "文本框(多行)";
            }
            if (fieldType == InputType.CheckBox.Value)
            {
                return "复选框";
            }
            if (fieldType == InputType.Radio.Value)
            {
                return "单选框";
            }
            if (fieldType == InputType.SelectOne.Value)
            {
                return "下拉列表(单选)";
            }
            if (fieldType == InputType.SelectMultiple.Value)
            {
                return "下拉列表(多选)";
            }
            if (fieldType == InputType.Date.Value)
            {
                return "日期选择框";
            }
            if (fieldType == InputType.DateTime.Value)
            {
                return "日期时间选择框";
            }
            if (fieldType == InputType.Hidden.Value)
            {
                return "隐藏";
            }

            throw new Exception();
        }

        public static bool IsSelectFieldType(string fieldType)
        {
            return EqualsIgnoreCase(fieldType, InputType.CheckBox.Value) ||
                   EqualsIgnoreCase(fieldType, InputType.Radio.Value) ||
                   EqualsIgnoreCase(fieldType, InputType.SelectMultiple.Value) ||
                   EqualsIgnoreCase(fieldType, InputType.SelectOne.Value);
        }

        public static bool EqualsIgnoreCase(string a, string b)
        {
            if (a == b) return true;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            return string.Equals(a.Trim().ToLower(), b.Trim().ToLower());
        }
        
        public static string ReadText(string filePath)
        {
            var sr = new StreamReader(filePath, Encoding.UTF8);
            var text = sr.ReadToEnd();
            sr.Close();
            return text;
        }

        public static void WriteText(string filePath, string content)
        {
            var file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            using (var writer = new StreamWriter(file, Encoding.UTF8))
            {
                writer.Write(content);
                writer.Flush();
                writer.Close();

                file.Close();
            }
        }

        public static bool IsFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static string GetDirectoryPath(string path)
        {
            var ext = Path.GetExtension(path);
            var directoryPath = !string.IsNullOrEmpty(ext) ? Path.GetDirectoryName(path) : path;
            return directoryPath;
        }

        public static bool IsDirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static void CreateDirectoryIfNotExists(string path)
        {
            var directoryPath = GetDirectoryPath(path);

            if (!IsDirectoryExists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch
                {
                    //Scripting.FileSystemObject fso = new Scripting.FileSystemObjectClass();
                    //string[] directoryNames = directoryPath.Split('\\');
                    //string thePath = directoryNames[0];
                    //for (int i = 1; i < directoryNames.Length; i++)
                    //{
                    //    thePath = thePath + "\\" + directoryNames[i];
                    //    if (StringUtils.Contains(thePath.ToLower(), ConfigUtils.Instance.PhysicalApplicationPath.ToLower()) && !IsDirectoryExists(thePath))
                    //    {
                    //        fso.CreateFolder(thePath);
                    //    }
                    //}                    
                }
            }
        }

        public static void CopyDirectory(string sourcePath, string targetPath, bool isOverride)
        {
            if (!Directory.Exists(sourcePath)) return;

            CreateDirectoryIfNotExists(targetPath);
            var directoryInfo = new DirectoryInfo(sourcePath);
            foreach (var fileSystemInfo in directoryInfo.GetFileSystemInfos())
            {
                var destPath = Path.Combine(targetPath, fileSystemInfo.Name);
                if (fileSystemInfo is System.IO.FileInfo)
                {
                    CopyFile(fileSystemInfo.FullName, destPath, isOverride);
                }
                else if (fileSystemInfo is DirectoryInfo)
                {
                    CopyDirectory(fileSystemInfo.FullName, destPath, isOverride);
                }
            }
        }

        public static bool CopyFile(string sourceFilePath, string destFilePath, bool isOverride)
        {
            var returnValue = true;
            try
            {
                CreateDirectoryIfNotExists(destFilePath);

                File.Copy(sourceFilePath, destFilePath, isOverride);
            }
            catch
            {
                returnValue = false;
            }
            return returnValue;
        }

        public const char UrlSeparatorChar = '/';
        public const char PathSeparatorChar = '\\';

        public static string PathCombine(params string[] paths)
        {
            var retval = string.Empty;
            if (paths != null && paths.Length > 0)
            {
                retval = paths[0]?.Replace(UrlSeparatorChar, PathSeparatorChar).TrimEnd(PathSeparatorChar) ?? string.Empty;
                for (var i = 1; i < paths.Length; i++)
                {
                    var path = paths[i] != null ? paths[i].Replace(UrlSeparatorChar, PathSeparatorChar).Trim(PathSeparatorChar) : string.Empty;
                    retval = Path.Combine(retval, path);
                }
            }
            return retval;
        }

        public static string[] GetDirectoryNames(string directoryPath)
        {
            var directorys = Directory.GetDirectories(directoryPath);
            var retval = new string[directorys.Length];
            var i = 0;
            foreach (var directory in directorys)
            {
                var directoryInfo = new DirectoryInfo(directory);
                retval[i++] = directoryInfo.Name;
            }
            return retval;
        }

        public static bool DeleteDirectoryIfExists(string directoryPath)
        {
            var retval = true;
            try
            {
                if (IsDirectoryExists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
            }
            catch
            {
                retval = false;
            }
            return retval;
        }

        public static string JsonSerialize(object obj)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var timeFormat = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                settings.Converters.Add(timeFormat);

                return JsonConvert.SerializeObject(obj, settings);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T JsonDeserialize<T>(string json)
        {
            try
            {
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var timeFormat = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                settings.Converters.Add(timeFormat);

                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
