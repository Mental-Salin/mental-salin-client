using SalinSDK.Pattern;
using System.Collections;
using System.Collections.Generic;
namespace SalinSDK
{
    class UserManager : Singleton<UserManager>
    {
        public UserInfo userInfo { get; private set; }

        public string processID;

        public string userID { get { return userInfo.userID; } }
        public string sessionKey { get { return userInfo.sessionKey; } }
        

        List<Friend> friendSearchList = new List<Friend>();       
        public void MakeSearchList(List<Friend> _searchList)
        {
            friendSearchList = _searchList;
        }

        public List<Friend> GetFriendSearchList()
        {
            return friendSearchList;
        }

        List<Friend> friendList = new List<Friend>();
        public void SetFriendList(List<Friend> _friendList)
        {
            friendList.Clear();
            for (int i = 0; i < _friendList.Count; i++)
            {
                if (_friendList[i].status == FriendStatus.Completed ||
                    _friendList[i].status == FriendStatus.Pending)
                    friendList.Add(_friendList[i]);
            }
        }

        public List<Friend> GetFriendList()
        {
            return friendList;
        }

        public void UpdateUserInfo(UserInfo _userInfo)
        {
            userInfo = _userInfo;
        }
    }
}
