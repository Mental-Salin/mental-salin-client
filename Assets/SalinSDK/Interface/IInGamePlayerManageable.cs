namespace SalinSDK
{
    public interface IInGamePlayerManageable
    {
        Player GetUser(string userToken, string userId);
        void UserBlocking(string userToken, Player player);
        void UserBlocking(string userToken, string userId);
        void UserUnblock(string userToken, Player player);
        void UserUnblock(string userToken, string userId);
        void UserKick(string userToken, Player player);
        void UserKick(string userToken, string userId);
    }
}