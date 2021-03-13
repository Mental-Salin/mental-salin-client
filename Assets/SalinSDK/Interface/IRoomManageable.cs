namespace SalinSDK
{
    public interface IRoomManageable
    {
        Room GetCurrentRoom();
        void CreateRoom(string userToken, string roomName, RoomOption roomOption);
        void JoinRoom(string userToken, string roomName);
        void JoinRoomWithPassword(string userToken, string roomName, string password);
        void JoinOrCreateRoom(string userToken, string roomName, RoomOption roomOption);
        void LeaveRoom(string userToken);
    }
}