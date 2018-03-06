namespace SS.GovInteract.Model
{
    public class ConfigInfo
    { 
        public int GovInteractChannelId { get; set; }
        public int ApplyDateLimit { get; set; } // 办理时限
        public int ApplyAlertDate { get; set; } // 预警
        public int ApplyYellowAlertDate { get; set; } // 黄牌
        public int ApplyRedAlertDate { get; set; } // 红牌
        public bool ApplyIsDeleteAllowed { get; set; } // 办件是否可删除 
        public bool ApplyIsOpenWindow { get; set; } // 办件是否可删除 
    }
}