using System.Collections.Generic;
using SiteServer.Plugin;
using SS.GovInteract.Core.Model;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Core
{
    public static class TemplateManager
    {
        public static string GetTemplatesDirectoryPath()
        {
            return Context.PluginApi.GetPluginPath(ApplicationUtils.PluginId, "templates");
        }

        public static List<TemplateInfo> GetTemplateInfoList()
        {
            var templateInfoList = new List<TemplateInfo>();

            var directoryPath = GetTemplatesDirectoryPath();
            var directoryNames = ApplicationUtils.GetDirectoryNames(directoryPath);
            foreach (var directoryName in directoryNames)
            {
                templateInfoList.Add(GetTemplateInfo(directoryPath, directoryName));
            }

            return templateInfoList;
        }

        public static TemplateInfo GetTemplateInfo(string name)
        {
            var directoryPath = GetTemplatesDirectoryPath();
            return GetTemplateInfo(directoryPath, name);
        }

        private static TemplateInfo GetTemplateInfo(string templatesDirectoryPath, string name)
        {
            TemplateInfo templateInfo;

            var configPath = ApplicationUtils.PathCombine(templatesDirectoryPath, name, "config.json");
            if (ApplicationUtils.IsFileExists(configPath))
            {
                templateInfo = ApplicationUtils.JsonDeserialize<TemplateInfo>(ApplicationUtils.ReadText(configPath));
                templateInfo.Name = name;
            }
            else
            {
                templateInfo = new TemplateInfo
                {
                    Name = name
                };
            }

            return templateInfo;
        }

        public static void Clone(string nameToClone, TemplateInfo templateInfo, List<TemplateInfo> templateInfoList)
        {
            var directoryPath = Context.PluginApi.GetPluginPath(ApplicationUtils.PluginId, "templates");

            ApplicationUtils.CopyDirectory(ApplicationUtils.PathCombine(directoryPath, nameToClone), ApplicationUtils.PathCombine(directoryPath, templateInfo.Name), true);
            
            var configJson = ApplicationUtils.JsonSerialize(templateInfo);
            var configPath = ApplicationUtils.PathCombine(directoryPath, templateInfo.Name, "config.json");
            ApplicationUtils.WriteText(configPath, configJson);
        }

        public static string GetTemplateHtml(TemplateInfo templateInfo)
        {
            var directoryPath = GetTemplatesDirectoryPath();
            var htmlPath = ApplicationUtils.PathCombine(directoryPath, templateInfo.Name, templateInfo.Main);
            var html = CacheUtils.Get<string>(htmlPath);
            if (html != null) return html;

            html = ApplicationUtils.ReadText(htmlPath);

            CacheUtils.InsertHours(htmlPath, html, 1);
            return html;
        }

        public static void SetTemplateHtml(TemplateInfo templateInfo, string html)
        {
            var directoryPath = GetTemplatesDirectoryPath();
            var htmlPath = ApplicationUtils.PathCombine(directoryPath, templateInfo.Name, templateInfo.Main);

            ApplicationUtils.WriteText(htmlPath, html);
            ClearCache(htmlPath);
        }

        public static void DeleteTemplate(string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            var directoryPath = GetTemplatesDirectoryPath();
            var templatePath = ApplicationUtils.PathCombine(directoryPath, name);
            ApplicationUtils.DeleteDirectoryIfExists(templatePath);
        }

        public static void ClearCache(string htmlPath)
        {
            CacheUtils.Remove(htmlPath);
        }
    }
}
