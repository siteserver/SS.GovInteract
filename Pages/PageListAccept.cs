using SS.GovInteract.Core;
using System;
using SS.GovInteract.Model;
using SS.GovInteract.Provider;

namespace SS.GovInteract.Pages
{
	public class PageListAccept : PageBaseList
	{
        public static string GetRedirectUrl(int siteId, int channelId)
        {
            return $"{nameof(PageListAccept)}.aspx?siteId={siteId}&channelId={channelId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override string GetSelectString()
        {
            return ContentDao.GetSelectStringByState(SiteId, ChannelId, EState.New);
        }

        private string _pageUrl;
        protected override string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_pageUrl))
                {
                    _pageUrl = GetRedirectUrl(SiteId, ChannelId);
                }
                return _pageUrl;
            }
        }
	}
}
