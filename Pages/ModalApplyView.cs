using System;
using SS.GovInteract.Core;

namespace SS.GovInteract.Pages
{
    public class ModalApplyView : PageBaseContent
	{
        public static string GetOpenWindowString(int siteId, int channelId, int contentId)
        {
            return LayerUtils.GetOpenScript("快速查看", $"{nameof(ModalApplyView)}.aspx?siteId={siteId}&channelId={channelId}&contentId={contentId}", 750, 600);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            
        }
	}
}
