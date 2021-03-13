using System.Collections.Generic;

namespace SalinSDK
{
    public interface ILobbyManageCallback
    {
        void OnJoinedLobby();
        void OnLeftLobby();
        void OnRoomListUpdate();
    }
}