using System;
using DefaultNamespace;

namespace SalinSDK
{
    [Serializable]
    public class RoomUserStateMessage : MessageData
    {

        public RoomUserStateMessage() : base("RoomUserStateMessage")
        {
        }

        public UserMode userMode;
        public bool isConnected;
        public bool isReady;
    }
}
