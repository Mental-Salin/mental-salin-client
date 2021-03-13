namespace SalinSDK
{
#if PHOTON_UNITY_NETWORKING
    using Photon.Pun;
    using Photon.Realtime;

    public class PhotonPlayerManager : IInGamePlayerManageable
    {
        public Player GetUser(string userToken, string userId)
        {
            if(XRSocialSDK.currentRoom.PlayerList.ContainsKey(userId))
                return XRSocialSDK.currentRoom.PlayerList[userId];

            return null;
        }


        public void UserBlocking(string userToken, string userId)
        {
            XRSocialSDK.currentRoom.AddBlockedPlayer(userId);
            
            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.BlockPlayer));
        }
        
        public void UserBlocking(string userToken, Player player)
        {
            UserBlocking(SalinTokens.UserToken, player.userId);
        }

        public void UserUnblock(string userToken, string userId)
        {
            XRSocialSDK.currentRoom.RemoveBlockedPlayer(userId);
        }
        
        public void UserUnblock(string userToken, Player player)
        {
            UserUnblock(SalinTokens.UserToken, player.userId);
        }

        public void UserKick(string userToken, Player player)
        {
            Photon.Realtime.Player photonPlayer = PhotonNetwork.CurrentRoom.GetPlayer(player.dynamicCodeInRoom);
            
            if (PhotonNetwork.CloseConnection(photonPlayer) == true)
            {
                UserBlocking(SalinTokens.UserToken, player);
                
                if(XRSocialSDK.currentRoom.PlayerList.ContainsKey(player.userId) == true)
                    XRSocialSDK.currentRoom.PlayerList.Remove(player.userId);
                    
                SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.KickPlayer, photonPlayer));
                SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.PlayerLeftRoom, photonPlayer));
            }
        }

        public void UserKick(string userToken, string userId)
        {
            Player player = GetUser(SalinTokens.UserToken, userId);

            UserKick(SalinTokens.UserToken, player);
        }
    }
#else
    #region Fake class
    public class PhotonPlayerManager : IInGamePlayerManageable
    {
        public Player GetUserWithAccount(string userToken, string userId) { return null; }
        public Player GetUserWithCode(string userToken, int userCode) { return null; }
        public Player GetUserWithNickName(string userToken, string userNickName) { return null; }
        public Player GetUser(string userToken, string userId) { return null; }
        public void UserBlocking(string userToken, Player player) { }
        public void UserBlocking(string userToken, string userId) { }
        public void UserUnblock(string userToken, Player player) { }
        public void UserUnblock(string userToken, string userId) { }
        public void UserBlocking(string userToken, int userCode) { }        
        public void UserKick(string userToken, Player player) { }
        public void UserKick(string userToken, string userId) { }
        public void UserKick(string userToken, int userCode) { }
    }
    #endregion
#endif
}