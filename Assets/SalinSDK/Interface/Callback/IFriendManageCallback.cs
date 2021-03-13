namespace SalinSDK
{
    using System.Collections.Generic;
    
    public interface IFriendManageCallback
    {
        void OnSearchFriend(UserInfo info);
        void OnSearchFriendFail(ErrorCode errorCode);

        void OnSearchFriendList(List<Friend> friendList);
        void OnSearchFriendListFail(ErrorCode errorCode);

        void OnUpdateState(List<Friend> friendList);
        void OnUpdateStateFail(ErrorCode errorCode);

        void OnRequestFriend(UserInfo info);
        void OnRequestFriendFail(ErrorCode errorCode);

        void OnResponseFriend(UserInfo info);
        void OnResponseFriendFail(ErrorCode errorCode);

        void OnRemoveFriend(UserInfo info);
        void OnRemoveFriendFail(ErrorCode errorCode);
    }
}