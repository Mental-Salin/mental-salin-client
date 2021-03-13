namespace SalinSDK
{
    public class TokenServerConnector : IMainServerConnectable
    {
        public bool IsConnected()
        {
            return string.IsNullOrEmpty(SalinTokens.AppToken) == false;
        }

        public MainServer Connect(string token)
        {
            RequestData reqData = new RequestData(HTTPMethod.GET, RequestDataType.APPTOKEN);
            reqData.SetReqStr(SalinServerURL.serverUrl + SalinServerAPI.appToken);
            reqData.AddField(SalinAPIKey.apikey, token);
            reqData.SendRequest();

            return null;
        }
    }    
}