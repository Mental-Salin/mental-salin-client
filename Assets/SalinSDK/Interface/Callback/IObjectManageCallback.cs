using System.Collections.Generic;

namespace SalinSDK
{
    public interface IObjectManageCallback
    {
        void OnSyncInstance(int netId);
        void OnSyncInstanceFail(ErrorCode errorCode);
        void OnRoomPropertiesChanged(KeyValuePair<string, object> roomProperties);
        void OnSendMessage();
        void OnSendMessageFail(ErrorCode errorCode);
        void OnSendBroadcastMessage();
        void OnSendBroadcastMessageFail(ErrorCode errorCode);
        void OnReceiveMessage(string message);
    }
}