using System;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
	public class PageListReply : PageBaseList
    {
        public static string GetRedirectUrl(int siteId, int channelId)
        {
            return $"{nameof(PageListReply)}.aspx?siteId={siteId}&channelId={channelId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override string GetSelectString()
        {
            return Main.ContentDao.GetSelectStringByState(SiteId, ChannelId, EState.Accepted, EState.Redo);
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
