namespace SalinSDK
{
    public interface IOutGamePlayerManageable
    {                
        void InvitePlayerToRoom(string userID, string roomName, string hostName);
        void RespondInviteRoom(string userID, string roomName, bool acceptInvite);
        void SendMessageToPlayer(string userID, string message);
    }
}