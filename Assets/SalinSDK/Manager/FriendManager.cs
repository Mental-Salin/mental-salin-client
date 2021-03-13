using UnityEngine;

namespace SalinSDK
{
    public static class FriendManager
    {

        static private IFriendSearchable _friendSearcher;
        static private IFriendSearchable friendSearcher
        {
            get
            {
                if(_friendSearcher == null)
                {
                    _friendSearcher = new FriendSearcher();
                }
                return _friendSearcher;
            }
        }

        static private IMyFriendManageable _myFriendManager;
        static private IMyFriendManageable myFriendManager
        {
            get
            {
                if(_myFriendManager == null)
                {
                    _myFriendManager = new MyFriendManager();
                }
                return _myFriendManager;
            }
        }

        /// <summary>
        /// 친구 요청 함수
        /// </summary>
        /// <param name="friendID">요청할 친구의 고정ID값 </param>
        static public void RequestFriend(string friendID)
        {
            myFriendManager.RequestFriend(friendID);
        }
        /// <summary>
        ///  친구 요청 응답 함수
        /// </summary>
        /// <param name="processID">요청할 친구의 ProcessID값</param>
        /// <param name="approve">true: 수락, false: 거절</param>
        static public void ResponeFriend( string processID, bool approve)
        {
            myFriendManager.ResponseFriend(processID, approve);
        }
        /// <summary>
        /// 친구 상태 업데이트 함수
        /// </summary>
        static public void UpdateState()
        {
            myFriendManager.UpdateState();
        }
        /// <summary>
        /// 친구 삭제 함수
        /// </summary>
        /// <param name="friendID">삭제할 친구의 고정ID값</param>
        static public void RemoveFriend(string friendID)
        {
            myFriendManager.RemoveFriend(friendID);
        }

        /// <summary>
        /// 친구 검색 함수
        /// </summary>
        /// <param name="searchAccount">검색할 계정Account</param>
        static public void SearchFriendList(string searchAccount)
        {
            friendSearcher.SearchFriendList(searchAccount);
        }
    }
}
