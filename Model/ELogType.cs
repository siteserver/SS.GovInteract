using System;
using System.Web.UI.WebControls;

namespace SS.GovInteract.Model
{
    public enum ELogType
    {
        New,                //新申请
        Accept,             //受理
        Deny,               //拒绝
        SwitchTo,           //转办
        Translate,          //转移
        Comment,            //批示
        Redo,               //要求返工
        Reply,              //办理
        Check,              //审核
    }

    public class ELogTypeUtils
    {
        public static string GetValue(ELogType type)
        {
            if (type == ELogType.New)
            {
                return "New";
            }
            if (type == ELogType.Accept)
            {
                return "Accept";
            }
            if (type == ELogType.Deny)
            {
                return "Deny";
            }
            if (type == ELogType.SwitchTo)
            {
                return "SwitchTo";
            }
            if (type == ELogType.Translate)
            {
                return "Translate";
            }
            if (type == ELogType.Comment)
            {
                return "Comment";
            }
            if (type == ELogType.Redo)
            {
                return "Redo";
            }
            if (type == ELogType.Reply)
            {
                return "Reply";
            }
            if (type == ELogType.Check)
            {
                return "Check";
            }
            throw new Exception();
        }

        public static string GetText(ELogType type)
        {
            if (type == ELogType.New)
            {
                return "前台 网友 提交办件";
            }
            if (type == ELogType.Accept)
            {
                return "受理办件";
            }
            if (type == ELogType.Deny)
            {
                return "拒绝办件";
            }
            if (type == ELogType.SwitchTo)
            {
                return "转办办件";
            }
            if (type == ELogType.Translate)
            {
                return "转移办件";
            }
            if (type == ELogType.Comment)
            {
                return "批示办件";
            }
            if (type == ELogType.Redo)
            {
                return "要求返工";
            }
            if (type == ELogType.Reply)
            {
                return "回复办件";
            }
            if (type == ELogType.Check)
            {
                return "审核通过办件";
            }
            throw new Exception();
        }

        public static ELogType GetEnumType(string typeStr)
        {
            var retval = ELogType.New;

            if (Equals(ELogType.New, typeStr))
            {
                retval = ELogType.New;
            }
            else if (Equals(ELogType.Accept, typeStr))
            {
                retval = ELogType.Accept;
            }
            else if (Equals(ELogType.Deny, typeStr))
            {
                retval = ELogType.Deny;
            }
            else if (Equals(ELogType.SwitchTo, typeStr))
            {
                retval = ELogType.SwitchTo;
            }
            else if (Equals(ELogType.Translate, typeStr))
            {
                retval = ELogType.Translate;
            }
            else if (Equals(ELogType.Comment, typeStr))
            {
                retval = ELogType.Comment;
            }
            else if (Equals(ELogType.Redo, typeStr))
            {
                retval = ELogType.Redo;
            }
            else if (Equals(ELogType.Reply, typeStr))
            {
                retval = ELogType.Reply;
            }
            else if (Equals(ELogType.Check, typeStr))
            {
                retval = ELogType.Check;
            }
            return retval;
        }

        public static bool Equals(ELogType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ELogType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ELogType type, bool selected)
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
                listControl.Items.Add(GetListItem(ELogType.New, false));
                listControl.Items.Add(GetListItem(ELogType.Accept, false));
                listControl.Items.Add(GetListItem(ELogType.Deny, false));
                listControl.Items.Add(GetListItem(ELogType.SwitchTo, false));
                listControl.Items.Add(GetListItem(ELogType.Translate, false));
                listControl.Items.Add(GetListItem(ELogType.Comment, false));
                listControl.Items.Add(GetListItem(ELogType.Redo, false));
                listControl.Items.Add(GetListItem(ELogType.Reply, false));
                listControl.Items.Add(GetListItem(ELogType.Check, false));
            }
        }
    }
}
