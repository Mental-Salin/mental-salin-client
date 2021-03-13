using System.Collections.Generic;

namespace SalinSDK
{
    public class RoomOption
    {
        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible;}
            set { _isVisible = value; }
        }
        
        private bool _isOpen = true;
        public bool IsOpen
        {
            get { return _isOpen;}
            set { _isOpen = value; }
        }
        
        public Dictionary<object, object> RoomProperties { get; set; }
        public List<string> RoomPropertiesForLobby{ get; set; }
        public int MaxPlayerCount{ get; set; }
        public string Password{ get; set; }
        public Dictionary<string, string> BlockedPlayerIdList{ get; set; }
        public HashSet<string> ExpectPlayerIdList{ get; set; }
    }
}