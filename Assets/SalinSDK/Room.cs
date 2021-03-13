using System.Collections.Generic;
using UnityEngine;

#if PHOTON_UNITY_NETWORKING
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
#endif

namespace SalinSDK
{
    public class Room
    {
        #region Room Info

        private string _roomName;
        public string RoomName
        {
            get { return _roomName;}
            private set { _roomName = value; }
        }
        private  string _roomId;
        public string RoomId
        {
            get { return _roomId;}
            private set { _roomId = value; }
        }

        #endregion

        #region Room Option

        private Dictionary<object, object> _roomProperties = new Dictionary<object, object>();
        public Dictionary<object, object> RoomProperties
        {
            get { return _roomProperties;}
            private set { _roomProperties = value; }
        } 
        
        private List<string> _roomPropertiesForLobby = new List<string>();
        public List<string> RoomPropertiesForLobby
        {
            get { return _roomPropertiesForLobby;}
            private set { _roomPropertiesForLobby = value; }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible;}
            private set { _isVisible = value; }
        }
        
        private bool _isOpen = true;
        public bool IsOpen
        {
            get { return _isOpen;}
            private set { _isOpen = value; }
        }
        
        private int _maxPlayerCount;
        public int MaxPlayerCount
        {
            get { return _maxPlayerCount;}
            private set { _maxPlayerCount = value; }
        }
        
        private Dictionary<string, string> _blockedPlayerIdList = new Dictionary<string, string>();
        public Dictionary<string, string> BlockedPlayerIdList
        {
            get { return _blockedPlayerIdList;}
            private set { _blockedPlayerIdList = value; }
        }

        private HashSet<string> _expectPlayerIdList = new HashSet<string>();
        public HashSet<string> ExpectPlayerIdList
        {
            get { return _expectPlayerIdList;}
            private set { _expectPlayerIdList = value; }
        }
        
        private string _password;
        public string Password
        {
            get { return _password;}
            private set { _password = value; }
        }

        #endregion

        #region Player

        private Dictionary<string, Player> _playerList = new Dictionary<string, Player>();
        public Dictionary<string, Player> PlayerList 
        {
            get { return _playerList;}
            set { _playerList = value; }
        }
        
        private string _hostPlayerId;
        public string HostPlayerId
        {
            get { return _hostPlayerId;}
            set { _hostPlayerId = value; }
        }
        
        private string _hostPlayerNickname;
        public string HostPlayerNickname
        {
            get { return _hostPlayerNickname;}
            set { _hostPlayerNickname = value; }
        }
        
        private string _keyPlayerId;
        public string KeyPlayerId
        {
            get { return _keyPlayerId;}
            set { _keyPlayerId = value; }
        }

        private string _keyPlayerNickname;
        public string KeyPlayerNickname
        {
            get { return _keyPlayerNickname;}
            set { _keyPlayerNickname = value; }
        }
        
        #endregion

        public Room()
        {
        }

        public Room(string roomName, string roomId)
        {
            RoomName = roomName;
            RoomId = roomId;

            HostPlayerId = "";
            HostPlayerNickname = "";
            KeyPlayerId = "";
            KeyPlayerNickname = "";
        }
        
        public Room(string roomName, string roomId, RoomOption roomOption) : this(roomName, roomId)
        {
            RoomProperties = roomOption.RoomProperties;
            RoomPropertiesForLobby = roomOption.RoomPropertiesForLobby;
            IsVisible = roomOption.IsVisible;
            IsOpen = roomOption.IsOpen;
            MaxPlayerCount = roomOption.MaxPlayerCount;
            BlockedPlayerIdList = roomOption.BlockedPlayerIdList;
            ExpectPlayerIdList = roomOption.ExpectPlayerIdList;
            Password = roomOption.Password;
        }

        public void SetRoomProperties(Dictionary<object, object> roomProperties, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            RoomProperties = roomProperties;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetCustomProperties((Hashtable)RoomProperties);
#endif
        }
        
        public void AddRoomProperties(KeyValuePair<object, object> roomProperties, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;

            if (RoomProperties.ContainsKey(roomProperties.Key) == true)
            {
                Debug.Log("The key is already contains RoomProperties. Update the value");
                RoomProperties[roomProperties.Key] = roomProperties.Value;
            }
            else
            {
                RoomProperties.Add(roomProperties.Key, roomProperties.Value);    
            }
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{roomProperties.Key, roomProperties.Value}});
#endif
        }

        public void RemoveRoomProperties(string roomPropertiesKey, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            if (RoomProperties.ContainsKey(roomPropertiesKey) == false)
            {
                Debug.Log("The key is not in the RoomProperties.");
                return;
            }

            RoomProperties.Remove(roomPropertiesKey);
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{roomPropertiesKey, null}});
#endif
        }
        
        public void SetRoomPropertiesForLobby(List<string> roomPropertiesForLobby = null, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            RoomPropertiesForLobby = roomPropertiesForLobby;
            
            if(RoomPropertiesForLobby == null)
                RoomPropertiesForLobby = new List<string>();
                
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.Password) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.Password);
            
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.IsVisible) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.IsVisible);
            
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.RoomName) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.RoomName);
            
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.BlockedPlayerIdList) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.BlockedPlayerIdList);
            
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.HostPlayerId) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.HostPlayerId);
            
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.HostPlayerName) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.HostPlayerName);
            
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.KeyPlayerId) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.KeyPlayerId);
            
            if(RoomPropertiesForLobby.Contains(RoomOptionKey.KeyPlayerName) == false)
                RoomPropertiesForLobby.Add(RoomOptionKey.KeyPlayerName);
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetPropertiesListedInLobby(RoomPropertiesForLobby.ToArray());
#endif
        }

        public void SetIsVisible(bool isVisible, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            IsVisible = isVisible;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{RoomOptionKey.IsVisible, isVisible}});
#endif
        }

        public void SetIsOpen(bool isOpen, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            IsOpen = isOpen;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.IsOpen = IsOpen;
#endif
        }

        public void SetMaxPlayerCount(int maxPlayerCount, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            MaxPlayerCount = maxPlayerCount;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.MaxPlayers = (byte)MaxPlayerCount;
#endif
        }

        public void SetBlockedPlayerList(Dictionary<string, string> blockedPlayerIdList, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            BlockedPlayerIdList = blockedPlayerIdList;
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{RoomOptionKey.BlockedPlayerIdList, BlockedPlayerIdList}});
#endif
        }

        public void AddBlockedPlayer(string playerId, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            if (BlockedPlayerIdList.ContainsKey(playerId) == true)
            {
                Debug.Log("Target player is already contains BlockedPlayerList");
                return;
            }

            BlockedPlayerIdList.Add(playerId, playerId);
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{RoomOptionKey.BlockedPlayerIdList, BlockedPlayerIdList}});
#endif
        }
        
        public void AddBlockedPlayer(Player player, bool reqSyncData = true)
        {
            AddBlockedPlayer(player.userId, reqSyncData);
        }

        public void RemoveBlockedPlayer(string playerId, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            if (BlockedPlayerIdList.ContainsKey(playerId) == false)
            {
                Debug.Log("Target player is not in the BlockedPlayerList");
                return;
            }
            
            BlockedPlayerIdList.Remove(playerId);
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{RoomOptionKey.BlockedPlayerIdList, BlockedPlayerIdList}});
#endif
        }
        
        public void RemoveBlockedPlayer(Player player, bool reqSyncData = true)
        {
            RemoveBlockedPlayer(player.userId, reqSyncData);
        }
        
        public void SetExpectPlayerList(HashSet<string> expectPlayerIdList, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            ExpectPlayerIdList = expectPlayerIdList;
        }

        public bool AddExpectPlayer(Player player, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return false;
            
            if (ExpectPlayerIdList.Contains(player.userId) == true)
            {
                Debug.Log("Target player is already contains ExpectPlayerList");
                return false;
            }

            return ExpectPlayerIdList.Add(player.userId);
        }

        public bool RemoveExpectPlayer(Player player, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return false;
            
            if (ExpectPlayerIdList.Contains(player.userId) == false)
            {
                Debug.Log("Target player is not in the ExpectPlayerList");
                return false;
            }
            
            return ExpectPlayerIdList.Remove(player.userId);
        }
        
        public void SetPassword(string password, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            Password = password;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
            {
                Hashtable ht = new Hashtable();
                ht.Add(RoomOptionKey.Password, Password);
                
                PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
            }
#endif
        }

        [System.Obsolete("This method is legacy method. don't use this method")]
        public void SetHostPlayerInfo(string userId, string userName, bool reqSyncData = true)
        {
            HostPlayerId = userId;
            HostPlayerNickname = userName;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
            {
                Hashtable ht = new Hashtable();
                ht.Add(RoomOptionKey.HostPlayerId, userId);
                ht.Add(RoomOptionKey.HostPlayerName, userName);

                PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
            }
#endif
        }

        [System.Obsolete("This method is legacy method. don't use this method")]
        public void SetKeyPlayerInfo(string userId, string userName, bool reqSyncData = true)
        {

#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
            {
                if (PlayerList.ContainsKey(userId) == true)
                {
                    // int actorId = PlayerList[userId].dynamicCodeInRoom;
                    // var photonPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorId);
                    //
                    // if (photonPlayer == null || PhotonNetwork.SetMasterClient(photonPlayer) == false)
                    // {
                    //     Debug.Log("Failed to change the key player.");
                    //     return;
                    // }

                    Hashtable ht = new Hashtable();
                    ht.Add(RoomOptionKey.KeyPlayerId, userId);
                    ht.Add(RoomOptionKey.KeyPlayerName, userName);

                    PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
                }
            }
#endif
            KeyPlayerId = userId;
            KeyPlayerNickname = userName;
        }
    }
}