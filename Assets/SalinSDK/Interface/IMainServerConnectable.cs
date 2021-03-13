namespace SalinSDK
{
    public interface IMainServerConnectable
    {
        bool IsConnected();
        MainServer Connect(string token);
    }    
}