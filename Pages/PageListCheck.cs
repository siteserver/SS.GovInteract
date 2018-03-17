using System;
using System.Collections.Specialized;
using SS.GovInteract.Core;

namespace SS.GovInteract.Pages
{
    public class PageListCheck : PageBaseList
    {
        public static string GetRedirectUrl(int siteId, int channelId)
        {
            return $"{nameof(PageListCheck)}.aspx?siteId={siteId}&channelId={channelId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override string GetSelectString()
        {
            return Main.ContentDao.GetSelectStringByState(SiteId, _channelId, EGovInteractState.Replied);
        }

        private string _pageUrl;
        protected override string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_pageUrl))
                {
                    _pageUrl = GetRedirectUrl(SiteId, _channelId);
                }
                return _pageUrl;
            }
        }
	}
}
