using UnityEngine;

namespace SalinSDK
{
    /// <summary>
    /// 성공 실패에 대한 콜백을 어떻게 해줄지에 대해 고민 해봐야 할듯
    /// </summary>
    class BaseAccountManager : IAccountManageable
    {
        //IAccountManageCallback accountManageCallback;

        public void LogIn(string account, string password)
        {
            RequestData reqData = new RequestData(HTTPMethod.PUT,RequestDataType.LOGIN);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.login);
            reqData.AddField(SalinAPIKey.workspaceId, SalinConstants.workspaceID);
   
            reqData.AddField(SalinAPIKey.account, account);
            reqData.AddField(SalinAPIKey.password, password);
            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField(SalinAPIKey.unixtime,"0");
            reqData.SendRequest();
        }

        public void LogOut()
        {
            RequestData reqData = new RequestData(HTTPMethod.PUT, RequestDataType.LOGOUT);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.logout);
            reqData.AddField(SalinAPIKey.workspaceId, SalinConstants.workspaceID);
            reqData.AddField(SalinAPIKey.loginKey, UserManager.Instance.userInfo.loginKey);        
            reqData.SendRequest();
        }

        public void SignUp(string account, string password, string nickname, Gender gender)
        {
            RequestData reqData = new RequestData(HTTPMethod.POST, RequestDataType.SIGNUP);
            reqData.AddField(SalinAPIKey.workspaceId, SalinConstants.workspaceID);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.signUp);

            reqData.AddField(SalinAPIKey.account, account);
            reqData.AddField(SalinAPIKey.password, password);
            reqData.AddField(SalinAPIKey.nickname, nickname);

            switch (gender)
            {
                case Gender.Female:
                    reqData.AddField(SalinAPIKey.sex, "w");
                    break;
                case Gender.Male:
                    reqData.AddField(SalinAPIKey.sex, "m");
                    break;
            }

            reqData.AddField(SalinAPIKey.pkgname, Application.productName);
            reqData.AddField("ava_index", "0");
            //reqData.AddField("sex", "M");

            reqData.SendRequest();

        }
    }
}
