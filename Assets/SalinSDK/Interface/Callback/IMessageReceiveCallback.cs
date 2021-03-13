namespace SalinSDK
{
    public interface IMessageReceiveCallback
    {
        void OnReceiveMessage<T>(T reveiveData) where T : MessageData;
    }

}