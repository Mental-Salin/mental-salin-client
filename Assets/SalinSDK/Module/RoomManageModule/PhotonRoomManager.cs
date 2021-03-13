using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SalinSDK
{
#if PHOTON_UNITY_NETWORKING
    using Photon.Pun;
    using Photon.Realtime;
    using ExitGames.Client.Photon;

    public class PhotonRoomManager : IRoomManageable, IInRoomCallbacks, IMatchmakingCallbacks
    {
        private Room room = null;
        
        public PhotonRoomManager()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public Room GetCurrentRoom()
        {
            if (PhotonNetwork.CurrentRoom == null)
                return null;
            
            return room;
        }

        public void CreateRoom(string userToken, string roomName, RoomOption roomOption = null)
        {
            if (XRSocialSDK.InLobby() == false)
            {
                Debug.LogError("You can create the room only when you are in the lobby");
                SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.CreateRoom, ErrorCode.NotInTheLobby));
                return;
            }

            RoomOptions PhotonRoomOptions = PhotonUtility.ConvertPhotonRoomOption(roomOption);
            if (PhotonRoomOptions != null)
            {
                if (PhotonRoomOptions.CustomRoomProperties == null)
                {
                    PhotonRoomOptions.CustomRoomProperties = new Hashtable();
                    PhotonRoomOptions.CustomRoomProperties.Add(RoomOptionKey.IsVisible, true);    
                }
                
                PhotonRoomOptions.CustomRoomProperties.Add(RoomOptionKey.RoomName, roomName);
            }
            
            PhotonNetwork.CreateRoom(roomName, PhotonRoomOptions);
        }

        public void JoinOrCreateRoom(string userToken, string roomName, RoomOption roomOption)
        {
            if (XRSocialSDK.InLobby() == false)
            {
                Debug.LogError("You can create or join the room only when you are in the lobby");
                SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.CreateRoom, ErrorCode.NotInTheLobby));
                return;
            }
            
            RoomOptions PhotonRoomOptions = PhotonUtility.ConvertPhotonRoomOption(roomOption);
            if (PhotonRoomOptions != null)
            {
                if(PhotonRoomOptions.CustomRoomProperties == null)
                    PhotonRoomOptions.CustomRoomProperties = new Hashtable();
                PhotonRoomOptions.CustomRoomProperties.Add(RoomOptionKey.RoomName, roomName);
            }
            
            PhotonNetwork.JoinOrCreateRoom(roomName, PhotonRoomOptions, TypedLobby.Default);
        }

        public void JoinRoom(string userToken, string roomName)
        {
            if (XRSocialSDK.InLobby() == false)
            {
                Debug.LogError("You can join the room only when you are in the lobby");
                SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.JoinRoom, ErrorCode.NotInTheLobby));
                return;
            }
            
            RoomInfo roomInfo = XRSocialSDK.GetCachedRoomInfo(roomName);

            if (roomInfo != null)
            {
                if (string.IsNullOrEmpty(roomInfo.Password) == false)
                {
                    Debug.LogError("Target room has a password. \n" +
                                   "You must enter the password.");
                
                    SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.JoinRoom, ErrorCode.InvalidPassword));
                    return;
                }

                if (roomInfo.BlockedPlayerIdList.ContainsKey(UserManager.Instance.userID))
                {
                    Debug.LogError("You are blocked from target room. \n" +
                                   "You can't join the room");
                
                    SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.JoinRoom, ErrorCode.BlockedFromRoom));
                    return;
                }
            }

            PhotonNetwork.JoinRoom(roomName);
        }

        public void JoinRoomWithPassword(string userToken, string roomName, string password)
        {
            if (XRSocialSDK.InLobby() == false)
            {
                Debug.LogError("You can join the room only when you are in the lobby");
                SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.JoinRoom, ErrorCode.NotInTheLobby));
                return;
            }
            
            RoomInfo roomInfo = XRSocialSDK.GetCachedRoomInfo(roomName);
            
            if (roomInfo != null)
            {
                if (roomInfo.BlockedPlayerIdList.ContainsKey(UserManager.Instance.userID))
                {
                    Debug.LogError("You are blocked from target room. \n" +
                                   "You can't join the room");
                
                    SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.JoinRoom, ErrorCode.BlockedFromRoom));
                    return;
                }
            }
            
            Hashtable enterRoomInfo = new Hashtable() {
                { RoomOptionKey.RoomName, roomName },
                { RoomOptionKey.Password, password },
            };
            
            PhotonNetwork.JoinRandomRoom(enterRoomInfo, 0);
        }
        
        public void LeaveRoom(string userToken)
        {
            PhotonNetwork.LeaveRoom();
        }
        

        private void InitRoomInfo()
        {
            if(room != null)
                return;
            
            room = new Room(PhotonNetwork.CurrentRoom.Name, "");

            UpdateRoomOption();
            UpdatePlayerList();
        }

        private void UpdateRoomOption()
        {
            room.SetIsOpen(PhotonNetwork.CurrentRoom.IsOpen, false);
            room.SetRoomProperties((Dictionary<object, object>)PhotonNetwork.CurrentRoom.CustomProperties.Clone(), false);
            room.SetRoomPropertiesForLobby(PhotonNetwork.CurrentRoom.PropertiesListedInLobby.ToList(), false);
            room.SetMaxPlayerCount(PhotonNetwork.CurrentRoom.MaxPlayers, false);

            if (room.RoomProperties.ContainsKey(RoomOptionKey.RoomName))
            {
                room.RoomProperties.Remove(RoomOptionKey.RoomName);
            }
            
            if (room.RoomProperties.ContainsKey(RoomOptionKey.HostPlayerId) && room.RoomProperties.ContainsKey(RoomOptionKey.HostPlayerName))
            {
                var userId = room.RoomProperties[RoomOptionKey.HostPlayerId].ToString();
                var userName = room.RoomProperties[RoomOptionKey.HostPlayerName].ToString();
                room.SetHostPlayerInfo(userId, userName, false);
                room.RoomProperties.Remove(RoomOptionKey.HostPlayerId);
                room.RoomProperties.Remove(RoomOptionKey.HostPlayerName);
            }
            
            if (room.RoomProperties.ContainsKey(RoomOptionKey.KeyPlayerId) && room.RoomProperties.ContainsKey(RoomOptionKey.KeyPlayerName))
            {
                var userId = room.RoomProperties[RoomOptionKey.KeyPlayerId].ToString();
                var userName = room.RoomProperties[RoomOptionKey.KeyPlayerName].ToString();
                room.SetKeyPlayerInfo(userId, userName, false);
                room.RoomProperties.Remove(RoomOptionKey.KeyPlayerId);
                room.RoomProperties.Remove(RoomOptionKey.KeyPlayerName);
            }
            
            if (room.RoomProperties.ContainsKey(RoomOptionKey.IsVisible))
            {
                bool isVisible = (bool)room.RoomProperties[RoomOptionKey.IsVisible];
                room.SetIsVisible(isVisible, false);
                room.RoomProperties.Remove(RoomOptionKey.IsVisible);
            }
            
            if (room.RoomProperties.ContainsKey(RoomOptionKey.Password))
            {
                var password = room.RoomProperties[RoomOptionKey.Password].ToString();
                room.SetPassword(password, false);
                room.RoomProperties.Remove(RoomOptionKey.Password);
            }

            if (room.RoomProperties.ContainsKey(RoomOptionKey.BlockedPlayerIdList))
            {
                Dictionary<string, string> blockedSet = room.RoomProperties[RoomOptionKey.BlockedPlayerIdList] as Dictionary<string, string>;
                room.SetBlockedPlayerList(blockedSet, false);
                room.RoomProperties.Remove(RoomOptionKey.BlockedPlayerIdList);
            }
                
            if (room.RoomProperties.ContainsKey(RoomOptionKey.ExpectPlayerIdList))
            {
                var expectPlayerIdList = room.RoomProperties[RoomOptionKey.ExpectPlayerIdList] as HashSet<string>;
                room.SetExpectPlayerList(expectPlayerIdList, false);
                room.RoomProperties.Remove(RoomOptionKey.ExpectPlayerIdList);
            }
        }

        private void UpdatePlayerList()
        {
            int playerCount =  PhotonNetwork.PlayerList.Length; 
            
            for(int idx = 0; idx < playerCount; ++idx)
                AddPlayerToList(PhotonNetwork.PlayerList[idx]);
        }

        private void AddPlayerToList(Photon.Realtime.Player newPlayers)
        {
            Player player = new Player(newPlayers);
                
            if(room.PlayerList.ContainsKey(player.userId) == true)
                room.PlayerList[player.userId].UpdatePlayerInfo(newPlayers);
            else
                room.PlayerList.Add(player.userId, player);
        }
        
        private void RemovePlayerFromList(Photon.Realtime.Player otherPlayer)
        {
            Player player = new Player(otherPlayer);
            
            string account = player.userId;
            if (string.IsNullOrEmpty(account) == false && room.PlayerList.ContainsKey(account) == true)
                room.PlayerList.Remove(account);
        }


        #region Photon Callback

        #region InRoom

        public void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Player player = new Player(newPlayer);
            
            if(room.BlockedPlayerIdList.ContainsKey(player.userId))
                return;
                
            AddPlayerToList(newPlayer);
            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.PlayerEnteredRoom, player.userId));
        }

        public void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Player player = new Player(otherPlayer);
            
            RemovePlayerFromList(otherPlayer);
            
            if(room.BlockedPlayerIdList.ContainsKey(player.userId) == false)
                SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.PlayerLeftRoom, otherPlayer));
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(RoomOptionKey.RoomName))
            {
                propertiesThatChanged.Remove(RoomOptionKey.RoomName);
            }
            
            if (propertiesThatChanged.ContainsKey(RoomOptionKey.Password))
            {
                string password = propertiesThatChanged[RoomOptionKey.Password].ToString();
                room.SetPassword(password, false);

                propertiesThatChanged.Remove(RoomOptionKey.Password);
                
                SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.ChangePassword));
            }

            if (propertiesThatChanged.ContainsKey(RoomOptionKey.IsVisible))
            {
                bool isVisible = (bool)propertiesThatChanged[RoomOptionKey.IsVisible];
                room.SetIsVisible(isVisible, false);
                propertiesThatChanged.Remove(RoomOptionKey.IsVisible);
            }
            
            if (propertiesThatChanged.ContainsKey(RoomOptionKey.BlockedPlayerIdList))
            {
                Dictionary<string, string> blockedSet = propertiesThatChanged[RoomOptionKey.BlockedPlayerIdList] as Dictionary<string, string>;
                room.SetBlockedPlayerList(blockedSet, false);
                propertiesThatChanged.Remove(RoomOptionKey.BlockedPlayerIdList);
            }
            
            if (propertiesThatChanged.ContainsKey(RoomOptionKey.HostPlayerId) && propertiesThatChanged.ContainsKey(RoomOptionKey.HostPlayerName))
            {
                var userId = propertiesThatChanged[RoomOptionKey.HostPlayerId].ToString();
                var userName = propertiesThatChanged[RoomOptionKey.HostPlayerName].ToString();
                room.SetHostPlayerInfo(userId, userName, false);
                propertiesThatChanged.Remove(RoomOptionKey.HostPlayerId);
                propertiesThatChanged.Remove(RoomOptionKey.HostPlayerName);
            }
            
            if (propertiesThatChanged.ContainsKey(RoomOptionKey.KeyPlayerId) && propertiesThatChanged.ContainsKey(RoomOptionKey.KeyPlayerName))
            {
                var userId = propertiesThatChanged[RoomOptionKey.KeyPlayerId].ToString();
                var userName = propertiesThatChanged[RoomOptionKey.KeyPlayerName].ToString();
                room.SetKeyPlayerInfo(userId, userName, false);
                propertiesThatChanged.Remove(RoomOptionKey.KeyPlayerId);
                propertiesThatChanged.Remove(RoomOptionKey.KeyPlayerName);
            }

            Dictionary<object, object> changeProp = new Dictionary<object, object>();
            var eProp = propertiesThatChanged.GetEnumerator();
            while (eProp.MoveNext() == true)
            {
                if (eProp.Current.Value == null)
                {
                    room.RemoveRoomProperties(eProp.Current.Key.ToString(), false);
                }
                else
                {
                    room.AddRoomProperties(new KeyValuePair<object, object>(eProp.Current.Key, eProp.Current.Value), false);
                    changeProp.Add(eProp.Current.Key, eProp.Current.Value);    
                }
            }
            
            if(changeProp.Count != 0)
                SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.UpdateRoomProperties, changeProp));
        }

        public void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if(targetPlayer.CustomProperties.ContainsKey(PlayerKey.UserId) == false)
                return;
            
            string userId = targetPlayer.CustomProperties[PlayerKey.UserId].ToString();
            Player player = XRSocialSDK.GetUser(userId);

            if (player == null)
            {
                Debug.Log("Can't find user.");
                return;
            }

            if (changedProps.ContainsKey(PlayerKey.UserId))
            {
                changedProps.Remove(PlayerKey.UserId);
            }
            
            if (changedProps.ContainsKey(PlayerKey.AllowInvite))
            {
                bool allow = (bool)changedProps[PlayerKey.AllowInvite];
                player.SetAllowInvite(allow, false);

                changedProps.Remove(PlayerKey.AllowInvite);
            }
            
            var eProp = changedProps.GetEnumerator();
            while (eProp.MoveNext() == true)
            {
                if (eProp.Current.Value == null)
                    player.RemoveUserProperties(eProp.Current.Key.ToString(), false);
                else
                    player.AddUserProperties(new KeyValuePair<object, object>(eProp.Current.Key, eProp.Current.Value), false);
            }
        }

        public void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            Player player = new Player(newMasterClient);
            room.SetKeyPlayerInfo(player.userId, player.userNickname);
        }

        

        #endregion
        

        #region Matchmaking

        public void OnFriendListUpdate(List<FriendInfo> friendList) { }

        public void OnCreatedRoom()
        {
            InitRoomInfo();
            
            string userId = UserManager.Instance.userInfo.userID;
            string userName = UserManager.Instance.userInfo.userNickname;
            
            room.SetHostPlayerInfo(userId, userName);
            room.SetKeyPlayerInfo(userId, userName);
            
            room.SetRoomPropertiesForLobby();
            
            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.CreateRoom));
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.CreateRoom, (ErrorCode)returnCode));
        }

        public void OnJoinedRoom()
        {
            XRSocialSDK.myPlayer.UpdatePlayerInfo(PhotonNetwork.LocalPlayer);
            
            InitRoomInfo();

            if (room.BlockedPlayerIdList.ContainsKey(UserManager.Instance.userID))
            {
                LeaveRoom(SalinTokens.UserToken);
                return;
            }

            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.JoinRoom));
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.JoinRoom, (ErrorCode)returnCode));
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            SalinCallbacks.OnPhotonCallbackError(new PhotonEvent(PhotonAction.JoinRoom, ErrorCode.InvalidPassword));
        }

        public void OnLeftRoom()
        {
            room = null;
            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.LeaveRoom));
        }

        #endregion
        

        #endregion
    }
#else
    #region Fake class
    public class PhotonRoomManager : IRoomManageable
    {
        public Room GetCurrentRoom(string userToken) { return null; }
        public Room GetCurrentRoom() { return null; }
        public void CreateRoom(string userToken, string roomName, RoomOption roomOption) {}
        public void JoinRoomWithPassword(string userToken, string roomName, string password) { }
        public void JoinOrCreateRoom(string userToken, string roomName, RoomOption roomOption) {}
        public void JoinRoom(string userToken, string roomName) {}
        public void LeaveRoom(string userToken) {}
    }
    #endregion
#endif
}