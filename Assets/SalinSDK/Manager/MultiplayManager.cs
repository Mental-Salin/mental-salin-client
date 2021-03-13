using UnityEngine;
using System.Collections.Generic;
namespace SalinSDK
{
    public class MultiplayManager
    {
        #region Valuable

        private Player _myPlayer = new Player();
        public Player myPlayer
        {
            get
            {
                return _myPlayer;
            }
        }
        
        public Room currentRoom
        {
            get
            {
                return roomManager.GetCurrentRoom();
            }
        }

        public bool joinRoom
        {
            get
            {
                if (currentRoom == null)
                    return false;

                return true;
            }
        }
        #endregion

        #region Room Manage

        IRoomManageable _roomManager;
        IRoomManageable roomManager
        {
            get
            {
                if (_roomManager == null)
                {
#if PHOTON_UNITY_NETWORKING
                    _roomManager = new PhotonRoomManager();
#elif UseWebRTC
                    
#endif
                }
                return _roomManager;
            }
        }

        // roon flow control
        public void CreateRoom(string roomName, RoomOption roomOption = null)
        {
            roomManager.CreateRoom(SalinTokens.UserToken, roomName, roomOption);
        }

        public void JoinRoom(string roomName)
        {
            roomManager.JoinRoom(SalinTokens.UserToken, roomName);
        }

        public void JoinRoomWithPassword(string roomName, string password)
        {
            roomManager.JoinRoomWithPassword(SalinTokens.UserToken, roomName, password);
        }

        public void JoinOrCreateRoom(string roomName, RoomOption roomOption = null)
        {
            roomManager.JoinOrCreateRoom(SalinTokens.UserToken, roomName, roomOption);
        }
        
        public void LeaveRoom()
        {
            roomManager.LeaveRoom(SalinTokens.UserToken);
        }

        #endregion
        
        
        #region Lobby Manage
        
        ILobbyManageable _lobbyManager;
        ILobbyManageable lobbyManager
        {
            get
            {
                if (_lobbyManager == null)
                {
#if PHOTON_UNITY_NETWORKING
                    _lobbyManager = new PhotonLobbyManager();
#elif UseWebRTC
                    
#endif
                }
                return _lobbyManager;
            }
        }
        
        public bool InLobby()
        {
            return lobbyManager.InLobby();
        }

        public void JoinLobby(string lobbyName = "")
        {
            lobbyManager.JoinLobby(lobbyName);
        }
        
        public void LeaveLobby()
        {
            lobbyManager.LeaveLobby();
        }
        
        public Dictionary<string, RoomInfo> GetRoomList()
        {
            return lobbyManager.GetRoomList();
        }
        
        public RoomInfo GetRoomInfoFromLobby(string roomName)
        {
            return lobbyManager.GetRoomInfoFromLobby(roomName);
        }
        
        public RoomInfo GetCachedRoomInfo(string roomName)
        {
            return lobbyManager.GetCachedRoomInfo(roomName);
        }
        
        #endregion
        
        
        #region Player Manage

        IInGamePlayerManageable _playerManager;
        IInGamePlayerManageable playerManager
        {
            get
            {
                if (_playerManager == null)
                {
#if PHOTON_UNITY_NETWORKING
                    _playerManager = new PhotonPlayerManager();
#elif UseWebRTC
                    
#endif
                }
                return _playerManager;
            }
        }

        public Player GetUser(string userId)
        {
            return playerManager.GetUser(SalinTokens.AppToken, userId);
        }

        public void UserBlocking(Player player)
        {
            playerManager.UserBlocking(SalinTokens.AppToken, player);
        }
        
        public void UserBlocking(string playerId)
        {
            playerManager.UserBlocking(SalinTokens.AppToken, playerId);
        }

        public void UserUnblock(Player player)
        {
            playerManager.UserUnblock(SalinTokens.AppToken, player);
        }
        
        public void UserUnblock(string playerId)
        {
            playerManager.UserUnblock(SalinTokens.AppToken, playerId);
        }
        
        public void UserKick(Player player)
        {
            playerManager.UserKick(SalinTokens.AppToken, player);
        }

        public void UserKick(string playerId)
        {
            playerManager.UserKick(SalinTokens.AppToken, playerId);
        }
        
        #endregion


        #region Message Send

        private IMessageSendable _messageSender;
        IMessageSendable messageSender
        {
            get
            {
                if (_messageSender == null)
                {
#if PHOTON_UNITY_NETWORKING
                    _messageSender = new PhotonMessageSender();
#elif UseWebRTC

#endif
                }
                return _messageSender;
            }
        } 

        public void SendMessage<T>(string userId, T data) where T : MessageData
        {
            messageSender.SendMessage(SalinTokens.UserToken, userId, data);
        }
        
        public void SendBroadcastMessage<T>(T data, SendTarget sendTarget) where T : MessageData
        {
            messageSender.SendBroadcastMessage(SalinTokens.UserToken, data, sendTarget);
        }
        
        #endregion
        
        
        #region Object Manage

        private IObjectManageable _objectManager;
        IObjectManageable objectManager
        {
            get
            {
                if (_objectManager == null)
                {
#if PHOTON_UNITY_NETWORKING
                    _objectManager = new PhotonObjectManager();
#elif UseWebRTC

#endif
                }
                return _objectManager;
            }
        }

        public NetworkObject CreateInstance(string prefName)
        {
            NetworkObject netObj = null;
            netObj = objectManager.CreateInstance(SalinTokens.UserToken, prefName);

            return netObj;
        }

        public NetworkObject GetInstance(int netId)
        {
            NetworkObject netObj = null;
            netObj = objectManager.GetInstance(SalinTokens.UserToken, netId);

            return netObj;
        }
        #endregion
    }
}