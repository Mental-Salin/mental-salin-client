namespace SalinSDK
{
#if PHOTON_UNITY_NETWORKING
    using Photon.Pun;
    using Photon.Realtime;
    using ExitGames.Client.Photon;
    public static class PhotonUtility
    {
        public static RoomOptions ConvertPhotonRoomOption(RoomOption sdkRO)
        {
            if (sdkRO == null)
                return new RoomOptions();
            
            RoomOptions photonRO = new RoomOptions();
            
            photonRO.IsOpen = sdkRO.IsOpen;
            photonRO.MaxPlayers = (byte)sdkRO.MaxPlayerCount;
            
            if(sdkRO.RoomPropertiesForLobby != null)
                photonRO.CustomRoomPropertiesForLobby = sdkRO.RoomPropertiesForLobby.ToArray();

            if (sdkRO.RoomProperties != null)
                photonRO.CustomRoomProperties = (Hashtable)sdkRO.RoomProperties;
            else
                photonRO.CustomRoomProperties = new Hashtable();
            
            
            if (photonRO.CustomRoomProperties != null)
            {
                if(photonRO.CustomRoomProperties.ContainsKey(RoomOptionKey.IsVisible) == true)
                    photonRO.CustomRoomProperties[RoomOptionKey.IsVisible] = sdkRO.IsVisible;
                else
                    photonRO.CustomRoomProperties.Add(RoomOptionKey.IsVisible, sdkRO.IsVisible);

                if (string.IsNullOrEmpty(sdkRO.Password) == false)
                {
                    if(photonRO.CustomRoomProperties.ContainsKey(RoomOptionKey.Password) == true)
                        photonRO.CustomRoomProperties[RoomOptionKey.Password] = sdkRO.Password;
                    else
                        photonRO.CustomRoomProperties.Add(RoomOptionKey.Password, sdkRO.Password);
                }
            
                if (sdkRO.BlockedPlayerIdList != null)
                {
                    if(photonRO.CustomRoomProperties.ContainsKey(RoomOptionKey.BlockedPlayerIdList) == true)
                        photonRO.CustomRoomProperties[RoomOptionKey.BlockedPlayerIdList] = sdkRO.BlockedPlayerIdList;
                    else
                        photonRO.CustomRoomProperties.Add(RoomOptionKey.BlockedPlayerIdList, sdkRO.BlockedPlayerIdList);
                }
            }

            return photonRO;
        }
    }
#else
    #region Fake class
    public static class PhotonUtility
    {
    }
    #endregion
#endif
}