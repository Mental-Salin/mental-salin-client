using System.Collections.Generic;

namespace SalinSDK
{
    public interface IRoomManageCallback
    {
        void OnCreateRoom();
        void OnCreateRoomFail(ErrorCode errorCode);
        void OnJoinRoom();
        void OnJoinRoomFail(ErrorCode errorCode);
        void OnLeaveRoom();
        void OnChangePassword();
        void OnUpdateRoomProperties(Dictionary<object, object> changeProp);
        void OnPlayerEnteredRoom(Player enterPlayer);
        void OnPlayerLeftRoom(Player leftPlayer);
    }
}

