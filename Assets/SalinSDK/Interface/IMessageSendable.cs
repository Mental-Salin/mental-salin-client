namespace SalinSDK
{
    public interface IMessageSendable
    {
        void SendMessage<T>(string userToken, string targetUserId, T data) where T : MessageData;
        void SendMessage<T>(string userToken, Player player, T data) where T : MessageData;
        void SendBroadcastMessage<T>(string userToken, T data, SendTarget sendTarget) where T : MessageData;
    }
}