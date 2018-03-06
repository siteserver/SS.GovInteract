using System;

namespace SS.GovInteract.Model
{
    public class AdministratorInfo
    {

        private string _displayName;

        public string UserName { get; set; }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                {
                    _displayName = UserName;
                }
                return _displayName;
            }
            set { _displayName = value; }
        } 

        public int DepartmentId { get; set; } 
         
        public AdministratorInfo()
        {
            UserName = string.Empty; 
            DepartmentId = 0;
            _displayName = string.Empty; 
        }

        public AdministratorInfo(string userName, int departmentId, string displayName)
        {
            UserName = userName;
            DepartmentId = departmentId;
            _displayName = displayName;
        }
    }
}
