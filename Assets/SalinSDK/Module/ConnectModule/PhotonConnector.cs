using System.Collections.Generic;

namespace SalinSDK
{
#if PHOTON_UNITY_NETWORKING
    using Photon.Pun;
    using Photon.Realtime;
    using ExitGames.Client.Photon;

    public class PhotonConnector : ISocialServerConnectable, IConnectionCallbacks
    {
        private bool isConnected = false;
        private bool AutoJoinLobby = false;
        public PhotonConnector()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public bool IsConnected()
        {
            return PhotonNetwork.IsConnected;
        }

        public SocialServer Connect(string token, bool autoJoinLobby = false)
        {
            isConnected = false;
            AutoJoinLobby = autoJoinLobby;

            PhotonNetwork.ConnectUsingSettings();
            return null;
        }

        private void SetMyPlayer()
        {
            PhotonNetwork.LocalPlayer.NickName = XRSocialSDK.myPlayer.userNickname;
            
            Hashtable datas = new Hashtable();

            var eUserProp = XRSocialSDK.myPlayer.userProperties.GetEnumerator();
            while (eUserProp.MoveNext() == true)
                datas.Add(eUserProp.Current.Key, eUserProp.Current.Value);

            if(datas.ContainsKey(PlayerKey.UserId))
                datas.Add(PlayerKey.UserId, XRSocialSDK.myPlayer.userId);
            else
                datas[PlayerKey.UserId] = XRSocialSDK.myPlayer.userId;
            
            if(datas.ContainsKey(PlayerKey.AllowInvite))
                datas.Add(PlayerKey.AllowInvite, XRSocialSDK.myPlayer.allowInvite);
            else
                datas[PlayerKey.AllowInvite] = XRSocialSDK.myPlayer.allowInvite;

            PhotonNetwork.LocalPlayer.SetCustomProperties(datas);
        }

        public void OnConnected() { }

        public void OnConnectedToMaster()
        {
            isConnected = true;

            SetMyPlayer();
            
            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.Connect));
            
            if(AutoJoinLobby == true)
                XRSocialSDK.JoinLobby();
        }
        
        public void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            if(isConnected == true)
                 SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.Disconnect, (DisconnectCause)cause));
             else
                 SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.Connect, (DisconnectCause)cause));
            
            isConnected = false;
        }

        public void OnRegionListReceived(RegionHandler regionHandler) { }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

        public void OnCustomAuthenticationFailed(string debugMessage) { }
    }

#else
    #region Fake class
    public class PhotonConnector : ISocialServerConnectable
    {
        public void Connect(string token) {}
        public bool IsConnected() { return false; }
        public SocialServer Connect(string token, bool autoJoinLobby) { return null; }
    }
    #endregion
#endif
}