using System;

namespace SS.GovInteract.Model
{
    public static class ContentAttribute
    {
        public const string Id = "Id";
        public const string DepartmentName = "DepartmentName";
        public const string QueryCode = "QueryCode";
        public const string State = "State";
        public const string IpAddress = "IpAddress";

        public const string RealName = "RealName";
        public const string Organization = "Organization";
        public const string CardType = "CardType";
        public const string CardNo = "CardNo";
        public const string Phone = "Phone";
        public const string PostCode = "PostCode";
        public const string Address = "Address";
        public const string Email = "Email";
        public const string Fax = "Fax";

        public const string TypeId = "TypeId";
        public const string IsPublic = "IsPublic"; 
        public const string Content = "Content";
        public const string FileUrl = "FileUrl";
        public const string DepartmentId = "DepartmentId";

        public const string ReplyContent = nameof(ReplyContent);
        public const string ReplyFileUrl = nameof(ReplyFileUrl);
        public const string ReplyDepartmentName = nameof(ReplyDepartmentName);
        public const string ReplyUserName = nameof(ReplyUserName);
        public const string ReplyAddDate = nameof(ReplyAddDate);

        //extend
        public const string TranslateFromChannelId = "TranslateFromChannelId";
    }
}
