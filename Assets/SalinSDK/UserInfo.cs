using System.Collections;
using LitJson;
using SalinSDK.ExtensionMethod;

namespace SalinSDK
{
    public class UserInfo
    {
        public string userID { get; private set; }
        public string userAccount { get; private set; }
        public string userNickname { get; private set; }
        public string sessionKey { get; private set; }
        public string loginKey { get; private set; }


        public UserInfo()
        {
            userID      = string.Empty;
            userAccount     = string.Empty;
            userNickname    = string.Empty;
            sessionKey  = string.Empty;
            loginKey    = string.Empty;
        }

        public UserInfo(string _userID, string _account, string _nickname)
        {
            userID = _userID;
            userAccount = _account;
            userNickname = _nickname;
            sessionKey = string.Empty;
            loginKey = string.Empty;
        }

        public UserInfo(string _userID, string _account, string _nickname, string _sessionKey, string _loginKey)
        {
            userID      = _userID;
            userAccount = _account;
            userNickname= _nickname;
            sessionKey  = _sessionKey;
            loginKey    = _loginKey;
        }


        public static UserInfo Convert(JsonData jsonData)
        {
            //Json 데이터를 다른 데이터로 파싱하는 작업
            string userID = string.Empty;
            if (jsonData.JsonDataContainsKey(SalinAPIKey.userID))
            {
                userID = jsonData[SalinAPIKey.userID].ToString();
            }
            string userAccount = string.Empty;
            if (jsonData.JsonDataContainsKey(SalinAPIKey.account))
            {
                userAccount = jsonData[SalinAPIKey.account].ToString();
            }
            string userNickname = string.Empty;
            if (jsonData.JsonDataContainsKey(SalinAPIKey.nickname))
            {
                userNickname = jsonData[SalinAPIKey.nickname].ToString();
            }
            string sessionKey = string.Empty;
            if (jsonData.JsonDataContainsKey(SalinAPIKey.sessionKey))
            {
                sessionKey = jsonData[SalinAPIKey.sessionKey].ToString();
            }
            string loginKey = string.Empty;
            if (jsonData.JsonDataContainsKey(SalinAPIKey.loginKey))
            {
                loginKey = jsonData[SalinAPIKey.loginKey].ToString();
            }

            return new UserInfo(userID, userAccount, userNickname, sessionKey, loginKey);
        }
    }
}