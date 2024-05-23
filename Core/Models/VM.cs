namespace Manager.Core.Models
{
    public class VM
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Shift => Name.Split('_')[0]; // Extracts the shift from the name
        public string LAN => Name.Split('_').Last(); // Extracts the LAN from the name
        public string Disk { get; set; } // Disk extracted from config
        public int Cell { get; set; } // Cell extracted from config
        public string OnlineStatus { get; set; } // Will be set based on the current shift and VM status
    }
}
