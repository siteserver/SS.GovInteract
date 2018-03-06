using System;
using System.Web.UI.WebControls;

namespace SS.GovInteract.Model
{
    public enum EPermissionType
    {
        GovInteractView, // 浏览办件
        GovInteractAdd,  // 新增办件
        GovInteractEdit, // 编辑办件
        GovInteractDelete, // 删除办件
        GovInteractSwitchToTranslate,  // 转办转移
        GovInteractComment,  // 批示办件
        GovInteractAccept,  // 受理办件
        GovInteractReply,  // 办理办件
        GovInteractCheck  //审核办件 
    }

    public class EPermissionTypeUtils
    {
        public static string GetValue(EPermissionType type)
        {
            if (type == EPermissionType.GovInteractView)
            {
                return "GovInteractView";
            }
            else if (type == EPermissionType.GovInteractAdd)
            {
                return "GovInteractAdd";
            }
            else if(type == EPermissionType.GovInteractEdit)
            {
                return "GovInteractEdit";
            }
            else if(type == EPermissionType.GovInteractDelete)
            {
                return "GovInteractDelete";
            }
            else if(type == EPermissionType.GovInteractSwitchToTranslate)
            {
                return "GovInteractSwitchToTranslate";
            }
            else if (type == EPermissionType.GovInteractComment)
            {
                return "GovInteractComment";
            }
            else if (type == EPermissionType.GovInteractAccept)
            {
                return "GovInteractAccept";
            }
            else if (type == EPermissionType.GovInteractReply)
            {
                return "GovInteractReply";
            }
            else if (type == EPermissionType.GovInteractCheck)
            {
                return "GovInteractCheck";
            }
            throw new Exception();
        }

        public static string GetText(EPermissionType type)
        {
            if (type == EPermissionType.GovInteractView)
            {
                return "浏览办件";
            }
            else if (type == EPermissionType.GovInteractAdd)
            {
                return "新增办件";
            }
            else if (type == EPermissionType.GovInteractEdit)
            {
                return "编辑办件";
            }
            else if (type == EPermissionType.GovInteractDelete)
            {
                return "删除办件";
            }
            else if (type == EPermissionType.GovInteractSwitchToTranslate)
            {
                return "转办转移";
            }
            else if (type == EPermissionType.GovInteractComment)
            {
                return "批示办件";
            }
            else if (type == EPermissionType.GovInteractAccept)
            {
                return "受理办件";
            }
            else if (type == EPermissionType.GovInteractReply)
            {
                return "办理办件";
            }
            else if (type == EPermissionType.GovInteractCheck)
            {
                return "审核办件 ";
            }
            throw new Exception();
        }     

        public static EPermissionType GetEnumType(string typeStr)
        {
            var retval = EPermissionType.GovInteractView;

            if (Equals(EPermissionType.GovInteractAdd, typeStr))
            {
                retval = EPermissionType.GovInteractAdd;
            }
            else if (Equals(EPermissionType.GovInteractEdit, typeStr))
            {
                retval = EPermissionType.GovInteractEdit;
            }
            else if (Equals(EPermissionType.GovInteractDelete, typeStr))
            {
                retval = EPermissionType.GovInteractDelete;
            }
            else if (Equals(EPermissionType.GovInteractSwitchToTranslate, typeStr))
            {
                retval = EPermissionType.GovInteractSwitchToTranslate;
            }
            else if (Equals(EPermissionType.GovInteractComment, typeStr))
            {
                retval = EPermissionType.GovInteractComment;
            }
            else if (Equals(EPermissionType.GovInteractAccept, typeStr))
            {
                retval = EPermissionType.GovInteractAccept;
            }
            else if (Equals(EPermissionType.GovInteractReply, typeStr))
            {
                retval = EPermissionType.GovInteractReply;
            }
            else if (Equals(EPermissionType.GovInteractCheck, typeStr))
            {
                retval = EPermissionType.GovInteractCheck;
            }
            return retval;
        }

        public static bool Equals(EPermissionType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EPermissionType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EPermissionType type, bool selected)
        {
            var item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractView, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractAdd, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractEdit, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractDelete, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractSwitchToTranslate, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractComment, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractAccept, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractReply, false));
                listControl.Items.Add(GetListItem(EPermissionType.GovInteractCheck, false)); 
            }
        }
    }
}
