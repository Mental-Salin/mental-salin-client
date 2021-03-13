using System.Collections.Generic;

namespace SalinSDK
{
    public class RoomInfo
    {
        private bool _removeFromList;
        public bool RemoveFromList
        {
            get { return _removeFromList; }
            private set { _removeFromList = value; }
        }

        
        private string _roomName;
        public string RoomName
        {
            get { return _roomName; }
            private set { _roomName = value; }
        }
        
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            private set { _isOpen = value; }
        }
        
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            private set { _isVisible = value; }
        }
        
        private int _playerCount;
        public int PlayerCount
        {
            get { return _playerCount; }
            private set { _playerCount = value; }
        }
        
        private int _maxPlayerCount;
        public int MaxPlayerCount
        {
            get { return _maxPlayerCount; }
            private set { _maxPlayerCount = value; }
        }
        
        public bool HasPassword
        {
            get { return string.IsNullOrEmpty(Password); }
        }
        
        private string _password;
        public string Password
        {
            get { return _password; }
            private set { _password = value; }
        }
        
        private Dictionary<object, object> _roomProperties;
        public Dictionary<object, object> RoomProperties
        {
            get { return _roomProperties; }
            private set { _roomProperties = value; }
        }

        
        
        private Dictionary<string, string> _blockedPlayerIdList;
        public Dictionary<string, string> BlockedPlayerIdList 
        {
            get { return _blockedPlayerIdList; }
            private set { _blockedPlayerIdList = value; }
        }


        private string _hostPlayerId;
        public string HostPlayerId
        {
            get { return _hostPlayerId; }
            private set { _hostPlayerId = value; }
        }
        
        private string _hostPlayerNickname;
        public string HostPlayerNickname
        {
            get { return _hostPlayerNickname; }
            private set { _hostPlayerNickname = value; }
        }
        

        private string _keyPlayerId;
        public string KeyPlayerId
        {
            get { return _keyPlayerId; }
            private set { _keyPlayerId = value; }
        }
        
        private string _keyPlayerNickname;
        public string KeyPlayerNickname
        {
            get { return _keyPlayerNickname; }
            private set { _keyPlayerNickname = value; }
        }

        public RoomInfo()
        {
        }
        
        #if PHOTON_UNITY_NETWORKING
        public RoomInfo(Photon.Realtime.RoomInfo roomInfo)
        {
            RemoveFromList = roomInfo.RemovedFromList;
            
            RoomName = roomInfo.Name;
            
            IsOpen = roomInfo.IsOpen;

            PlayerCount = roomInfo.PlayerCount;
            MaxPlayerCount = roomInfo.MaxPlayers;

            RoomProperties = (Dictionary<object, object>)roomInfo.CustomProperties.Clone();

            if (RoomProperties.ContainsKey(RoomOptionKey.Password))
            {
                Password = RoomProperties[RoomOptionKey.Password].ToString();
                RoomProperties.Remove(RoomOptionKey.Password);
            }
            
            if (RoomProperties.ContainsKey(RoomOptionKey.IsVisible))
            {
                IsVisible = (bool)RoomProperties[RoomOptionKey.IsVisible];
                RoomProperties.Remove(RoomOptionKey.IsVisible);
            }
            
            if (RoomProperties.ContainsKey(RoomOptionKey.HostPlayerId))
            {
                HostPlayerId = RoomProperties[RoomOptionKey.HostPlayerId].ToString();
                RoomProperties.Remove(RoomOptionKey.HostPlayerId);
            }
            
            if (RoomProperties.ContainsKey(RoomOptionKey.HostPlayerName))
            {
                HostPlayerNickname = RoomProperties[RoomOptionKey.HostPlayerName].ToString();
                RoomProperties.Remove(RoomOptionKey.HostPlayerName);
            }
            
            if (RoomProperties.ContainsKey(RoomOptionKey.KeyPlayerId))
            {
                KeyPlayerId = RoomProperties[RoomOptionKey.KeyPlayerId].ToString();
                RoomProperties.Remove(RoomOptionKey.KeyPlayerId);
            }
            
            if (RoomProperties.ContainsKey(RoomOptionKey.KeyPlayerName))
            {
                KeyPlayerNickname = RoomProperties[RoomOptionKey.KeyPlayerName].ToString();
                RoomProperties.Remove(RoomOptionKey.KeyPlayerName);
            }
            
            if (RoomProperties.ContainsKey(RoomOptionKey.BlockedPlayerIdList))
            {
                BlockedPlayerIdList = RoomProperties[RoomOptionKey.BlockedPlayerIdList] as Dictionary<string, string>;
                RoomProperties.Remove(RoomOptionKey.BlockedPlayerIdList);
            }
            else
                BlockedPlayerIdList = new Dictionary<string, string>();
        }
        #endif
    }
}