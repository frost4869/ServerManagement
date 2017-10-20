using System.ComponentModel;

namespace ServerManagement
{
    public enum Enums
    {
        Admin, Writer, Reader
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
}
