using System.Collections.Generic;

namespace SalinSDK
{
    public interface ILobbyManageable
    {
        bool InLobby();
        void JoinLobby(string lobbyName);
        void LeaveLobby();
        Dictionary<string, RoomInfo> GetRoomList();
        RoomInfo GetRoomInfoFromLobby(string roomName);
        RoomInfo GetCachedRoomInfo(string roomName);
    }
}