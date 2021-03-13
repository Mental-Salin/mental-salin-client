namespace SalinSDK
{
    public interface IConnectionCallback
    {
        void OnConnectedMainServer(string appToken);
        void OnConnectedMainServerFail(ErrorCode errorCode);
        void OnConnectedSocialServer();
        void OnConnectedSocialServerFail(DisconnectCause disconnectCause);
        void OnDisconnectedSocialServer(DisconnectCause disconnectCause);
        void OnConnectedMessageServer();
        void OnConnectedMessageServerFail(ErrorCode errorCode);
        void OnDisconnectedMessageServer();
    }
}