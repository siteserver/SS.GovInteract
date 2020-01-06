using System.Web;
using SiteServer.Plugin;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Core.Parser
{
    public static class StlGovInteract
    {
        public const string ElementName = "stl:govInteract";
        
        private const string AttributeType = "type";

        public static string Parse(IParseContext context)
        {
            var type = string.Empty;

            foreach (var name in context.StlAttributes.AllKeys)
            {
                var value = context.StlAttributes[name];

                if (ApplicationUtils.EqualsIgnoreCase(name, AttributeType))
                {
                    type = Context.ParseApi.ParseAttributeValue(value, context);
                }
            }

            if (string.IsNullOrEmpty(context.StlInnerHtml))
            {
                var elementId = $"iframe_{StringUtils.GetShortGuid(false)}";
                var libUrl = Context.PluginApi.GetPluginUrl(ApplicationUtils.PluginId, "assets/lib/iframe-resizer-3.6.3/iframeResizer.min.js");
                var pageUrl = Context.PluginApi.GetPluginUrl(ApplicationUtils.PluginId, $"templates/{type}/index.html?siteId={context.SiteId}&apiUrl={HttpUtility.UrlEncode(Context.Environment.ApiUrl)}");

                return $@"
<iframe id=""{elementId}"" frameborder=""0"" scrolling=""no"" src=""{pageUrl}"" style=""width: 1px;min-width: 100%;""></iframe>
<script type=""text/javascript"" src=""{libUrl}""></script>
<script type=""text/javascript"">iFrameResize({{log: false}}, '#{elementId}')</script>
";
            }

            return $@"
<script>
var $apiUrl = '{Context.Environment.ApiUrl}';
var $siteId = {context.SiteId};
</script>
{context.StlInnerHtml}
";
        }
    }
}
