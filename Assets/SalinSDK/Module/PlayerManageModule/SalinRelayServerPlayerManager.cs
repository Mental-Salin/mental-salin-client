using UnityEngine;

namespace SalinSDK
{
    public class SalinRelayServerPlayerManager : IOutGamePlayerManageable
    {
        private RelayServer _relayServer = null;
        private RelayServer relayServer
        {
            get
            {
                if (_relayServer == null)
                {
                    if (XRSocialSDK.MessageServer == null)
                    {
                        Debug.Log("messageServer is null");
                    }
                    else
                    {
                        if (XRSocialSDK.MessageServer is RelayServer)
                            _relayServer = XRSocialSDK.MessageServer as RelayServer;
                    }
                }

                return _relayServer;
            }
        }

        public void InvitePlayerToRoom(string userID, string roomName, string hostName = "")
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            relayServer?.InvitePlayerToRoom(userID, roomName, hostName);
        }

        public void RespondInviteRoom(string userID, string roomName, bool acceptInvite)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            relayServer?.RespondInviteRoom(userID, roomName, acceptInvite);
        }

        
        // Don't use this method
        public void SendMessageToPlayer(string userID, string message)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            relayServer?.SendMessageToTarget(userID, message);
        }
    }
}