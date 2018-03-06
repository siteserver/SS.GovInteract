using System;
using System.Web.UI.WebControls;

namespace SS.GovInteract.Model
{
    public enum ELimitType
	{
        Normal,                 //正常
        Alert,                  //预警
        Yellow,                 //黄牌
        Red,                    //红牌
	}

    public class ELimitTypeUtils
	{
		public static string GetValue(ELimitType type)
		{
            if (type == ELimitType.Normal)
			{
                return "Normal";
			}
            else if (type == ELimitType.Alert)
			{
                return "Alert";
            }
            else if (type == ELimitType.Yellow)
            {
                return "Yellow";
            }
            else if (type == ELimitType.Red)
            {
                return "Red";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ELimitType type)
		{
            if (type == ELimitType.Normal)
			{
                return "未超期";
			}
            else if (type == ELimitType.Alert)
			{
                return "预警";
            }
            else if (type == ELimitType.Yellow)
            {
                return "黄牌";
            }
            else if (type == ELimitType.Red)
            {
                return "红牌";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ELimitType GetEnumType(string typeStr)
		{
            var retval = ELimitType.Normal;

            if (Equals(ELimitType.Normal, typeStr))
			{
                retval = ELimitType.Normal;
			}
            else if (Equals(ELimitType.Alert, typeStr))
			{
                retval = ELimitType.Alert;
            }
            else if (Equals(ELimitType.Yellow, typeStr))
            {
                retval = ELimitType.Yellow;
            }
            else if (Equals(ELimitType.Red, typeStr))
            {
                retval = ELimitType.Red;
            }
			return retval;
		}

		public static bool Equals(ELimitType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ELimitType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ELimitType type, bool selected)
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
                listControl.Items.Add(GetListItem(ELimitType.Normal, false));
                listControl.Items.Add(GetListItem(ELimitType.Alert, false));
                listControl.Items.Add(GetListItem(ELimitType.Yellow, false));
                listControl.Items.Add(GetListItem(ELimitType.Red, false));
            }
        }
	}
}
