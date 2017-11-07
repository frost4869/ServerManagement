using System.ComponentModel;

namespace ServerManagement
{
    public enum Enums
    {
        Admin = 1,
        Writer = 2,
        Reader = 3
    }
    public enum IpType
    {
        [Description("Ip Management")]
        Management = 1,
        [Description("Ip WAN")]
        Wan = 2,
        [Description("Ip LAN")]
        Lan = 3,
        [Description("Ip Database")]
        Database = 4
    }
    public enum ActivityType
    {
        Create = 0,
        Update = 1,
        Delete = 2,
        Login = 3,
        Logout = 4
    }
}
