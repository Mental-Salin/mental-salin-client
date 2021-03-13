using UnityEngine;


namespace SalinSDK
{
    class MyFriendManager : IMyFriendManageable
    {
        //친구 목록(상태) 업데이트
        public void UpdateState()
        {
            RequestData reqData = new RequestData(HTTPMethod.GET, RequestDataType.UPDATESTATE);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.updateState);
            reqData.AddField(SalinAPIKey.userID, UserManager.Instance.userID);
            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField(SalinAPIKey.sessionKey, UserManager.Instance.sessionKey);
            reqData.AddField(SalinAPIKey.unixtime, "0");
            reqData.SendRequest();
        }

        //친구 신청 요청 상태 
        public void RequestFriend( string friendID)
        {
            RequestData reqData = new RequestData(HTTPMethod.POST, RequestDataType.REQUESTFRIEND);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.reqFriend);
            reqData.AddField(SalinAPIKey.userID, UserManager.Instance.userID);
            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField(SalinAPIKey.sessionKey, UserManager.Instance.sessionKey);
            reqData.AddField(SalinAPIKey.friendID, friendID);

            reqData.SendRequest();
        }

        //친구 신청 받은 상태 
        public void ResponseFriend( string processID, bool Approve)
        {
            RequestData reqData = new RequestData(HTTPMethod.PUT, RequestDataType.RESPONSEFRIEND);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.responseFriend);
            reqData.AddField(SalinAPIKey.userID, UserManager.Instance.userID);
            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField(SalinAPIKey.sessionKey, UserManager.Instance.sessionKey);
            reqData.AddField(SalinAPIKey.processID, processID);
            if (Approve)
                reqData.AddField(SalinAPIKey.type, "y");
            else
                reqData.AddField(SalinAPIKey.type, "n");

            reqData.SendRequest();
        }

        //친구 삭제 
        public void RemoveFriend(  string friendID)
        {
            RequestData reqData = new RequestData(HTTPMethod.DELETE, RequestDataType.REMOVEFRIEND);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.removeFriend);
            reqData.AddField(SalinAPIKey.userID, UserManager.Instance.userID);
            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField(SalinAPIKey.sessionKey, UserManager.Instance.sessionKey);
            reqData.AddField(SalinAPIKey.friendID, friendID);
            reqData.SendRequest();
        }

        #region 사용안함 나중에 수정 가능성 있음
        //일반적인 친구 추가(사용될지 잘 모르겠음)
        //public void AddFriend(  string friendAccount)
        //{

        //}

        //public void MyFriendList(string userToken)
        //{

        //}
        #endregion
    }
}
