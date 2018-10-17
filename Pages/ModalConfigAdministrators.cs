using System;
using System.Collections;
using System.Web.UI.WebControls;
using SS.GovInteract.Core;
using SS.GovInteract.Model;
using SS.GovInteract.Provider;

namespace SS.GovInteract.Pages
{
    public class ModalConfigAdministrators : PageBase
    {
        public Literal LtlMessage;
        public DataGrid DgContents; 

        private int channelId;

        public static string GetRedirectUrl(int siteId, int channelId)
        {
            return $"{nameof(ModalConfigAdministrators)}.aspx?siteId={siteId}&channelId={channelId}";
        }

        public static string GetOpenWindowString(int siteId,int channelId)
        {  
           return LayerUtils.GetOpenScript("负责人员设置", $"{nameof(ModalConfigAdministrators)}.aspx?siteId={siteId}&channelId={channelId}", 0, 0);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            channelId = Utils.ToInt(Request.QueryString["channelId"]);

            if (!IsPostBack && channelId > 0)
            {
                var channelInfo = ChannelDao.GetChannelInfo(SiteId, channelId);
                var departmentIdList = InteractManager.GetDepartmentIdList(channelInfo);
                var userNameArrayList = new ArrayList();
                foreach (var departmentId in departmentIdList)
                {
                    userNameArrayList.AddRange(AdministratorDao.GetUserNameArrayList(departmentId, true));
                }

                string userA, userB;
                for (int i = 0; i < userNameArrayList.Count-1; i++)
                {
                    userA = userNameArrayList[i].ToString();
                    for (int j = userNameArrayList.Count - 1; j > i; j--)
                    {
                        userB = userNameArrayList[j].ToString();
                        if (userA == userB)
                        {
                            userNameArrayList.Remove(userNameArrayList[j]);
                            j--;
                        }
                    }
                }

                DgContents.DataSource = userNameArrayList;
                DgContents.ItemDataBound += DgContents_ItemDataBound;
                DgContents.DataBind(); 
            }
        }

        private void DgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var userName = e.Item.DataItem as string;
                var administratorInfo = AdministratorDao.GetByUserName(userName);
                var permissionsInfo = PermissionsDao.GetPermissionsInfo(userName, channelId);

                var ltlDepartmentName = e.Item.FindControl("ltlDepartmentName") as Literal;
                var ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                var ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                var ltlPermissions = e.Item.FindControl("ltlPermissions") as Literal;
                var ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(administratorInfo.DepartmentId);
                ltlUserName.Text = userName;
                ltlDisplayName.Text = administratorInfo.DisplayName;

                if (permissionsInfo != null)
                {
                    var permissionNameArrayList = new ArrayList();
                    var permissionArrayList = Utils.StringCollectionToStringList(permissionsInfo.Permissions);
                    foreach (string permission in permissionArrayList)
                    {
                        permissionNameArrayList.Add(EPermissionTypeUtils.GetText(EPermissionTypeUtils.GetEnumType(permission)));
                    }
                    ltlPermissions.Text = Utils.ObjectCollectionToString(permissionNameArrayList);
                }

                //ltlEditUrl.Text =
                //    $@"<a href='javascript:;' onclick=""{ModalPermissions.GetOpenWindowString(
                //        SiteId, channelId, userName)}"">设置权限</a>";
            }

        } 
            
    }
}
