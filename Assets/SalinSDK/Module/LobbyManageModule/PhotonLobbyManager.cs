using System.Collections.Generic;
using UnityEngine;

namespace SalinSDK
{
#if PHOTON_UNITY_NETWORKING
    using Photon.Pun;
    using Photon.Realtime;
    
    public class PhotonLobbyManager : ILobbyManageable, ILobbyCallbacks
    {
        private bool InitLobby = false;
        private Dictionary<string, RoomInfo> RoomList = new Dictionary<string, RoomInfo>();
        private Dictionary<string, RoomInfo> CachedRoomList = new Dictionary<string, RoomInfo>();
        
        public PhotonLobbyManager()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public bool InLobby()
        {
            return PhotonNetwork.InLobby;
        }

        public void JoinLobby(string lobbyName = "")
        {
            InitLobby = false;
            
            if(lobbyName == "")
                PhotonNetwork.JoinLobby();
            else
                PhotonNetwork.JoinLobby(new TypedLobby(lobbyName, LobbyType.Default));
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        public Dictionary<string, RoomInfo> GetRoomList()
        {
            if (InLobby() == false)
                return null;
            
            return RoomList;
        }
        
        public RoomInfo GetRoomInfoFromLobby(string roomName)
        {
            if (InLobby() == false)
                return null;

            if (RoomList.ContainsKey(roomName) == false)
                return null;
            
            return RoomList[roomName];
        }

        public RoomInfo GetCachedRoomInfo(string roomName)
        {
            if (InLobby() == false)
                return null;

            if (CachedRoomList.ContainsKey(roomName) == false)
                return null;
            
            return CachedRoomList[roomName];
        }
        
        #region Photon Callback
        
        public void OnJoinedLobby()
        {
            InitLobby = false;
            //SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.JoinLobby));
        }

        public void OnLeftLobby()
        {
            InitLobby = false;
            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.LeaveLobby));
        }

        public void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
        {
            foreach (var photonRoomInfo in roomList)
            {
                RoomInfo roomInfo = new RoomInfo(photonRoomInfo);


                if (photonRoomInfo.RemovedFromList == true)
                {
                    if (RoomList.ContainsKey(photonRoomInfo.Name) == true)
                        RoomList.Remove(photonRoomInfo.Name);
                    
                    if (CachedRoomList.ContainsKey(photonRoomInfo.Name) == true)
                        CachedRoomList.Remove(photonRoomInfo.Name);
                }
                else
                {
                    if (roomInfo.IsVisible == true)
                    {
                        if (RoomList.ContainsKey(photonRoomInfo.Name) == true)
                            RoomList[photonRoomInfo.Name] = roomInfo;
                        else
                            RoomList.Add(photonRoomInfo.Name, roomInfo);
                    }

                    if (CachedRoomList.ContainsKey(photonRoomInfo.Name) == true)
                        CachedRoomList[photonRoomInfo.Name] = roomInfo;
                    else
                        CachedRoomList.Add(photonRoomInfo.Name, roomInfo);
                }
            }

            if (InitLobby == false)
            {
                InitLobby = true;
                SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.JoinLobby));
            }

            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.UpdateRoomList));
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }
        
        #endregion
        
    }
#else
    public class PhotonLobbyManager : ILobbyManageable
    {
        public bool InLobby() { return false; }
        public void JoinLobby(string lobbyName) { }
        public void LeaveLobby() { }
        public Dictionary<string, RoomInfo> GetRoomList() { return null; }
        public RoomInfo GetRoomInfoFromLobby(string roomName) { return null; }
        public RoomInfo GetCachedRoomInfo(string roomName) { return null; }
    }
#endif
}