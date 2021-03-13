using System.Collections.Generic;
using UnityEngine;

namespace SalinSDK
{
    public static class XRSocialSDK
    {
        #region Valuable
        
        private static ServerManager _serverManager;

        private static MultiplayManager _multiplayManager;

        private static SalinRelayServerPlayerManager _salinRelayServerPlayerManager;

        #endregion

        #region Properties

        public static bool IsJoinRoom {
            get
            {
                return _multiplayManager.joinRoom;
            }
        }
        
        public static Room currentRoom {
            get
            {
                return _multiplayManager.currentRoom;
            }
        }
        
        public static Player myPlayer {
            get
            {
                return _multiplayManager.myPlayer;
            }
        }
        
        public static MainServer MainServer
        {
            get { return _serverManager.GetMainServer();}
        }
        
        public static SocialServer SocialServer
        {
            get { return _serverManager.GetSocialServer(); }
        }
        
        public static MessageServer MessageServer
        {
            get { return _serverManager.GetMessageServer();}
        }

        public static bool IsConnected_MainServer
        {
            get { return _serverManager.IsConnectedToMainServer(); }
        }
        
        public static bool IsConnected_SocialServer
        {
            get { return _serverManager.IsConnectedToSocialServer(); }
        }
        
        public static bool IsConnected_MessageServer
        {
            get { return _serverManager.IsConnectedToMessageServer(); }
        }

        #endregion
        

        #region Constructor

        static XRSocialSDK()
        {
            _serverManager = new ServerManager();
            _multiplayManager = new MultiplayManager();
            _salinRelayServerPlayerManager = new SalinRelayServerPlayerManager();
        }

        #endregion
        
        #region Connect to server

        public static void ConnectToMainServer()
        {
            _serverManager.ConnectToMainServer();
        }
        
        public static void ConnectToSocialServer(bool autoJoinLobby = false)
        {
            _serverManager.ConnectToSocialServer(autoJoinLobby);
        }
        
        public static void ConnectToMessageServer()
        {
            _serverManager.ConnectToMessageServer();
        }

        #endregion

        #region Manage Room

        public static void CreateRoom(string roomName, RoomOption roomOption = null)
        {
            _multiplayManager.CreateRoom(roomName, roomOption);
        }

        public static void JoinRoom(string roomName)
        {
            _multiplayManager.JoinRoom(roomName);
        }

        public static void JoinRoomWithPassword(string roomName, string password)
        {
            _multiplayManager.JoinRoomWithPassword(roomName, password);
        }

        public static void LeaveRoom()
        {
            _multiplayManager.LeaveRoom();
        }

        #endregion
        
        
        #region Manage Lobby

        public static bool InLobby()
        {
            return _multiplayManager.InLobby();
        }
        
        public static void JoinLobby(string lobbyName = "")
        {
            _multiplayManager.JoinLobby(lobbyName);
        }

        public static Dictionary<string, RoomInfo> GetRoomList()
        {
            return _multiplayManager.GetRoomList();
        }
        
        public static RoomInfo GetRoomInfoFromLobby(string roomName)
        {
            return _multiplayManager.GetRoomInfoFromLobby(roomName);
        }
        
        public static RoomInfo GetCachedRoomInfo(string roomName)
        {
            return _multiplayManager.GetCachedRoomInfo(roomName);
        }
        #endregion
        
        
        #region Manage player

        public static Player GetUser(string userId)
        {
            return _multiplayManager.GetUser(userId);
        }

        public static void UserBlocking(Player player)
        {
            _multiplayManager.UserBlocking(player);
        }

        public static void UserBlocking(string userId)
        {
            _multiplayManager.UserBlocking(userId);
        }
        
        public static void UserUnblock(Player player)
        {
            _multiplayManager.UserUnblock(player);
        }

        public static void UserUnblock(string userId)
        {
            _multiplayManager.UserUnblock(userId);
        }
        
        public static void UserKick(Player player)
        {
            _multiplayManager.UserKick(player);
        }

        public static void UserKick(string userId)
        {
            _multiplayManager.UserKick(userId);
        }
        
        #endregion

        #region Send Message
        public static void SendMessage<T>(string userId, T data) where T : MessageData
        {
            _multiplayManager.SendMessage(userId, data);
        }
        
        public static void SendBroadcastMessage<T>(T data, SendTarget sendTarget = SendTarget.ToOthers) where T : MessageData
        {
            _multiplayManager.SendBroadcastMessage(data, sendTarget);
        }

        #endregion
        
        #region Invite player

        public static void InviteFriend(string userID, string hostName = "")
        {
            if (_multiplayManager.joinRoom == false)
            {
                Debug.Log("You are not in a Room. you have to join room first");
                return;
            }

            string roomName = _multiplayManager.currentRoom.RoomName;
            _salinRelayServerPlayerManager?.InvitePlayerToRoom(userID, roomName, hostName);
        }
        
        public static void RespondInviteRoom(string userID, string roomName, bool acceptInvite)
        {
            _salinRelayServerPlayerManager?.RespondInviteRoom(userID, roomName, acceptInvite);
        }

        #endregion

        #region Manage object 

        public static NetworkObject CreateInstance(string prefName)
        {
            if (_multiplayManager.joinRoom == false)
            {
                Debug.Log("You are not in a Room. you have to join room first");
                return null;
            }
            
            return _multiplayManager.CreateInstance(prefName);
        }

        public static NetworkObject GetInstance(int netId)
        {
            if (_multiplayManager.joinRoom == false)
            {
                Debug.Log("You are not in a Room. you have to join room first");
                return null;
            }
            
            return _multiplayManager.GetInstance(netId);
        }

        #endregion
    }
}