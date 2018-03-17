using System;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;

namespace SS.GovInteract.Pages
{
    public class ModalTypeList : PageBase
    {
        public Literal LtlMessage;
        public DataGrid DgContents;
        public Button BtnAdd;

        private int channelId;

        public static string GetRedirectUrl(int siteId, int channelId)
        {
            return $"{nameof(ModalTypeList)}.aspx?siteId={siteId}&channelId={channelId}";
        }

        public static string GetOpenWindowStringToList(int siteId,int channelId)
        {  
           return Utils.GetOpenLayerString("办件类型管理", Main.Instance.PluginApi.GetPluginUrl($"{nameof(ModalTypeList)}.aspx?siteId={siteId}&channelId={channelId}"), 0, 0);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            channelId = Utils.ToInt(Request.QueryString["channelId"]);
            var id = Convert.ToInt32(Request.QueryString["id"]);

            if (!IsPostBack && channelId > 0)
            {
                if (Request.QueryString["Delete"] != null && id > 0)
                {
                    try
                    {
                        Main.TypeDao.Delete(id);
                        LtlMessage.Text = Utils.GetMessageHtml("成功删除分类法", true);
                    }
                    catch (Exception ex)
                    {
                        LtlMessage.Text = Utils.GetMessageHtml($"删除分类法失败，{ex.Message}", false);
                    }
                }
                else if ((Request.QueryString["Up"] != null || Request.QueryString["Down"] != null) && id > 0)
                {
                    var isDown = Request.QueryString["Down"] != null;
                    if (isDown)
                    {
                        Main.TypeDao.UpdateTaxisToUp(id, channelId);
                        LtlMessage.Text = Utils.GetMessageHtml($"排序成功", true);
                    }
                    else
                    {
                        Main.TypeDao.UpdateTaxisToDown(id, channelId);
                        LtlMessage.Text = Utils.GetMessageHtml($"排序成功", true);
                    }
                }

                DgContents.DataSource = Main.TypeDao.GetDataSource(channelId);
                DgContents.ItemDataBound += DgContents_ItemDataBound;
                DgContents.DataBind();

                BtnAdd.Attributes.Add("onclick", ModalTypeAdd.GetOpenWindowStringToAdd(SiteId, channelId));
            }
        }

        private void DgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var id = Utils.EvalInt(e.Item.DataItem, "ID");
                var typeName = Utils.EvalString(e.Item.DataItem, "TypeName");

                var ltlTypeName = e.Item.FindControl("ltlTypeName") as Literal;
                var hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                var hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                var ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                var ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlTypeName.Text = typeName;

                hlUpLinkButton.NavigateUrl = $"{GetRedirectUrl(SiteId, channelId)}&id={id.ToString()}&Up={true.ToString()}";
                hlDownLinkButton.NavigateUrl = $"{GetRedirectUrl(SiteId,channelId)}&id={id.ToString()}&Down={true.ToString()}";
                ltlEditUrl.Text = $@"<a href='javascript:;' onclick=""{ModalTypeAdd.GetOpenWindowStringToEdit(SiteId, channelId, id)}"">编辑</a>";  
                var urlDelete = $"{GetRedirectUrl(SiteId, channelId)}&id={id.ToString()}&Delete={true.ToString()}";
                ltlDeleteUrl.Text = $@"<a href=""{urlDelete}"" onClick=""javascript:return confirm('此操作将删除办件类型“{typeName}”，确认吗？');"">删除</a>";
            }

        } 
            
    }
}
