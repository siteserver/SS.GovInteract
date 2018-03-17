using System;
using SS.GovInteract.Core;

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
            return Main.ContentDao.GetSelectStringByState(SiteId, _channelId, EGovInteractState.Accepted, EGovInteractState.Redo);
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
