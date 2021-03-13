namespace SalinSDK
{
    public static class SalinTokens
    {
        private static string _appId;
        public static string AppId
        {
            get
            {
                return _appId;
            }
            set
            {
                _appId = value;
            }
        }
        
        private static string _appToken;
        public static string AppToken {
            get
            {
                return _appToken;
            }
            set
            {
                _appToken = value;
            }
        }
        
        private static string _userToken;
        public static string UserToken 
        { get
            {
                return _userToken;
            }
            set
            {
                _userToken = value;
            }
        }

        
        public static bool ValidateTokenAppId()
        {
            return true; 
        }
        
        public static bool ValidateTokenAppToken()
        {
            return true; 
        }
        
        public static bool ValidateTokenUserToken()
        {
            return true; 
        }

        static SalinTokens()
        {
            AppId = SalinSetting.Data.appKey;
        }
    }
}