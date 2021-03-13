namespace SalinSDK
{
    public interface IMessageServerConnectable
    {
        bool IsConnected();
        MessageServer Connect(string appToken, string userToken);
    }
}