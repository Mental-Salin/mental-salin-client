namespace SalinSDK
{
    public interface ISocialServerConnectable
    {
        bool IsConnected();
        SocialServer Connect(string token, bool autoJoinLobby);
    }
}