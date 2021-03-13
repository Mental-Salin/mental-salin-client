using UnityEngine;

namespace SalinSDK
{
    public class ServerManager
    {
        #region MainServer

        private MainServer mainServer;

        private IMainServerConnectable _mainServerConnector;
        IMainServerConnectable mainServerConnector
        {
            get
            {
                if (_mainServerConnector == null)
                {
                    _mainServerConnector = new TokenServerConnector();
                }
                return _mainServerConnector;
            }
        }

        public MainServer GetMainServer()
        {
            return mainServer;
        }

        public bool IsConnectedToMainServer()
        {
            return mainServerConnector.IsConnected();
        }

        public MainServer ConnectToMainServer()
        {
            mainServer = mainServerConnector.Connect(SalinTokens.AppId);
            return mainServer;
        }

        #endregion

        #region SocialServer

        private  SocialServer socialServer;
        
        private ISocialServerConnectable _socialServerConnector;
        ISocialServerConnectable socialServerConnector
        {
            get
            {
                if (_socialServerConnector == null)
                {
#if PHOTON_UNITY_NETWORKING
                    _socialServerConnector = new PhotonConnector();
#elif UseWebRTC
                    
#endif
                }
                return _socialServerConnector;
            }
        }

        public SocialServer GetSocialServer()
        {
            return socialServer;
        }
        
        public bool IsConnectedToSocialServer()
        {
            return socialServerConnector.IsConnected();
        }
        
        public SocialServer ConnectToSocialServer(bool autoJoinLobby = false)
        {
            if (UserManager.Instance.userInfo == null)
            {
                Debug.Log("To connect to message server you should sign in first.");
                return null;
            }
            
            socialServer = socialServerConnector.Connect(SalinTokens.AppToken, autoJoinLobby);
            return socialServer;
        }

        #endregion

        #region MessageServer

        private MessageServer messageServer;

        private IMessageServerConnectable _messageServerConnector;
        IMessageServerConnectable messageServerConnector
        {
            get
            {
                if (_messageServerConnector == null)
                {
                    _messageServerConnector = new SalinRelayServerConnector();
                }
                return _messageServerConnector;
            }
        }

        public MessageServer GetMessageServer()
        {
            return messageServer;
        }
        
        public bool IsConnectedToMessageServer()
        {
            return messageServerConnector.IsConnected();
        }
        
        public MessageServer ConnectToMessageServer()
        {
            if (UserManager.Instance.userInfo == null)
            {
                Debug.Log("To connect to message server you should sign in first.");
                return null;
            }

            string userId = UserManager.Instance.userID;
            
            messageServer = messageServerConnector.Connect(SalinTokens.AppToken, userId);
            return messageServer;
        }

        #endregion
    }
}