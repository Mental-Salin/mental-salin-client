using UnityEngine;

namespace SalinSDK
{
    class FriendSearcher : IFriendSearchable
    {
        //친구 검색
        public void SearchFriend(  string searchAccount)
        {
            RequestData reqData = new RequestData(HTTPMethod.GET, RequestDataType.SEARCHFRIEND);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.searchFriend);
            reqData.AddField(SalinAPIKey.userID, UserManager.Instance.userID);
            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField(SalinAPIKey.sessionKey, UserManager.Instance.sessionKey);
            reqData.AddField(SalinAPIKey.account, searchAccount);
            reqData.SendRequest();
        }

        public void SearchFriendList(  string searchAccount)
        {
            RequestData reqData = new RequestData(HTTPMethod.GET, RequestDataType.SEARCHFRIENDLIST);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.searchFriend);
            reqData.AddField(SalinAPIKey.userID, UserManager.Instance.userID);
            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField(SalinAPIKey.sessionKey, UserManager.Instance.sessionKey);
            reqData.AddField(SalinAPIKey.account, searchAccount);
            reqData.SendRequest();
        }
    }
}
