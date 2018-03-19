using System;
using System.Web.UI.WebControls;

namespace SS.GovInteract.Model
{
    public enum EState
    {
        New,                //新办件
        Denied,             //拒绝受理
        Accepted,           //已受理
        Redo,               //要求返工
        Replied,            //已办理
        Checked,            //已审核
    }

    public class EStateUtils
    {
        public static string GetValue(EState type)
        {
            if (type == EState.New)
            {
                return "New";
            }
            if (type == EState.Denied)
            {
                return "Denied";
            }
            if (type == EState.Accepted)
            {
                return "Accepted";
            }
            if (type == EState.Redo)
            {
                return "Redo";
            }
            if (type == EState.Replied)
            {
                return "Replied";
            }
            if (type == EState.Checked)
            {
                return "Checked";
            }
            throw new Exception();
        }

        public static string GetText(EState type)
        {
            if (type == EState.New)
            {
                return "新办件";
            }
            if (type == EState.Denied)
            {
                return "拒绝受理";
            }
            if (type == EState.Accepted)
            {
                return "已受理";
            }
            if (type == EState.Redo)
            {
                return "要求返工";
            }
            if (type == EState.Replied)
            {
                return "已办理";
            }
            if (type == EState.Checked)
            {
                return "处理完毕";
            }
            throw new Exception();
        }

        public static string GetFrontText(EState type)
        {
            if (type == EState.Denied)
            {
                return "拒绝受理";
            }
            if (type == EState.Checked)
            {
                return "办理完毕";
            }
            return "办理中";
        }

        public static EState GetEnumType(string typeStr)
        {
            var retval = EState.New;

            if (Equals(EState.New, typeStr))
            {
                retval = EState.New;
            }
            else if (Equals(EState.Denied, typeStr))
            {
                retval = EState.Denied;
            }
            else if (Equals(EState.Accepted, typeStr))
            {
                retval = EState.Accepted;
            }
            else if (Equals(EState.Redo, typeStr))
            {
                retval = EState.Redo;
            }
            else if (Equals(EState.Replied, typeStr))
            {
                retval = EState.Replied;
            }
            else if (Equals(EState.Checked, typeStr))
            {
                retval = EState.Checked;
            }
            return retval;
        }

        public static bool Equals(EState type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EState type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EState type, bool selected)
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
            if (listControl == null) return;

            listControl.Items.Add(GetListItem(EState.New, false));
            listControl.Items.Add(GetListItem(EState.Denied, false));
            listControl.Items.Add(GetListItem(EState.Accepted, false));
            listControl.Items.Add(GetListItem(EState.Redo, false));
            listControl.Items.Add(GetListItem(EState.Replied, false));
            listControl.Items.Add(GetListItem(EState.Checked, false));
        }
    }
}
