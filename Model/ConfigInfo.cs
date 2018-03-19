namespace SS.GovInteract.Model
{
    public class ConfigInfo
    {
        public ConfigInfo()
        {
            ApplyDateLimit = 15;
            ApplyAlertDate = -3;
            ApplyYellowAlertDate = 3;
            ApplyRedAlertDate = 10;
            ApplyIsDeleteAllowed = true;
            ApplyIsOpenWindow = false;
        }

        public int ApplyDateLimit { get; set; } // 办理时限
        public int ApplyAlertDate { get; set; } // 预警
        public int ApplyYellowAlertDate { get; set; } // 黄牌
        public int ApplyRedAlertDate { get; set; } // 红牌
        public bool ApplyIsDeleteAllowed { get; set; } // 办件是否可删除 
        public bool ApplyIsOpenWindow { get; set; } // 办件是否可删除 
    }
}