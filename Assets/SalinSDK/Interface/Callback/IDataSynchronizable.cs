namespace SalinSDK
{
    public interface IDataSynchronizable
    {
        void SyncInstance(string userToken, NetworkObject netObj);
        void SyncRoomProperties(string userToken);
    }
}