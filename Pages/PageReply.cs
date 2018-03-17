using System.Web;

namespace SS.GovInteract.Pages
{
	public class PageReply : PageBase
    {
        public static string GetRedirectUrl(int siteId, int channelId, int contentId, string listPageUrl)
        {
            return $"{nameof(PageReply)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}&returnUrl={HttpUtility.UrlEncode(listPageUrl)}";
        }
	}
}
