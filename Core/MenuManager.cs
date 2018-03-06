using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SiteServer.Plugin;
using SS.GovInteract.Pages;

namespace SS.GovInteract.Core
{
    public class MenuManager
    {
        public static Func<int, Menu> SiteMenu => siteId => new Menu
        {
            Text = "互动交流",
            IconClass = "ion-chatbox-working",
            Menus = new List<Menu>
                {
                    new Menu
                    {
                        Text = "互动交流设置",
                        Href = $"{nameof(PageCommonConfiguration)}.aspx"
                    },
                    new Menu
                    {
                        Text = "数据统计分析",
                        Href = $"{nameof(PageAnalysis)}.aspx"
                    }
                }
        };
    }
}
