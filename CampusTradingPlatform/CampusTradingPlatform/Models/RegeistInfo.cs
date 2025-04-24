namespace CampusTradingPlatform.Models
{
    public class RegeistInfo
    {
        public string cellphone { get; set; }
        public string email { get; set; }
        public string vercode { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string nickname { get; set; }
        public int gender { get; set; }
        public bool agreement { get; set; } // 修改为布尔类型
    }
}