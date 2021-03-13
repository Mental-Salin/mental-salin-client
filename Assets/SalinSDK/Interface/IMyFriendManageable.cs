namespace SalinSDK
{
    public interface IMyFriendManageable
    {
        void UpdateState();
        void RequestFriend(string friendAccount);
        void ResponseFriend(string processID, bool approve);

        void RemoveFriend(string friendID);
    }
}