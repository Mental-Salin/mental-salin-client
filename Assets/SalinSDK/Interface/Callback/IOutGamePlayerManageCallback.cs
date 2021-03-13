namespace SalinSDK
{
    public interface IOutGamePlayerManageCallback
    {
        void OnReceiveInvitePlayerToRoom(string senderId, string roomName, string hostName);
        void OnReceiveRespondInviteRoom(string senderId, string roomName, bool acceptInvite);
        void OnUserNotFound(string action, string error);
    }

}