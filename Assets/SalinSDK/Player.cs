using System;
using System.Collections.Generic;
using UnityEngine;
#if PHOTON_UNITY_NETWORKING
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
#endif
namespace SalinSDK
{
    public class Player
    {
        public string userId;
        public string userNickname;
        
        public Dictionary<object, object> userProperties { get; private set; }
        public int constantCodeInRoom { get; private set; }
        public int dynamicCodeInRoom { get; private set; }
        
        public bool isMyPlayer { get; private set; }
        public bool isHostPlayer 
        {
            get
            {
                if (XRSocialSDK.currentRoom == null)
                    return false;
                return XRSocialSDK.currentRoom.HostPlayerId.Equals(userId);
            }
        }
        public bool isKeyPlayer 
        {
            get
            {
                if (XRSocialSDK.currentRoom == null)
                    return false;
                return XRSocialSDK.currentRoom.KeyPlayerId.Equals(userId);
            }
        }
        public bool allowInvite { get; private set; }

        public Player()
        {
            allowInvite = true;
            userProperties = new Dictionary<object, object>();
        }
        
        public Player(UserInfo info) : this()
        {
            userId = info.userID;
            userNickname = info.userNickname;
            isMyPlayer = userId.Equals(UserManager.Instance.userID);
        }
        
        public void UpdatePlayerInfo(UserInfo info)
        {
            userId = info.userID;
            userNickname = info.userNickname;
            isMyPlayer = userId.Equals(UserManager.Instance.userID);
        }

        public void SetAllowInvite(bool allow, bool reqSyncData = true)
        {
            allowInvite = allow;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == false)
                return;

            Hashtable ht = new Hashtable();
            ht.Add(PlayerKey.AllowInvite, allowInvite);
            
            if(userId == UserManager.Instance.userID)
                PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
            else
            {
                Photon.Realtime.Player photonPlayer = Photon.Pun.PhotonNetwork.CurrentRoom.GetPlayer(dynamicCodeInRoom);
                photonPlayer.SetCustomProperties(ht);
            }
#endif
        }

        
        public void SetUserProperties(Dictionary<object, object> userProperties, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            this.userProperties = userProperties;
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
            {
                Hashtable ht = new Hashtable();

                var eProp = userProperties.GetEnumerator();
                while (eProp.MoveNext() == true)
                    ht.Add(eProp.Current.Key, eProp.Current.Value);

                if(ht.Count == 0)
                    return;
                
                if(userId == UserManager.Instance.userID)
                    PhotonNetwork.LocalPlayer.SetCustomProperties((Hashtable)userProperties);
                else
                {
                    Photon.Realtime.Player photonPlayer = Photon.Pun.PhotonNetwork.CurrentRoom.GetPlayer(dynamicCodeInRoom);
                    photonPlayer.SetCustomProperties((Hashtable)userProperties);
                }
            }
#endif
        }
        
        public void AddUserProperties(KeyValuePair<object, object> userProperty, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;

            if (this.userProperties.ContainsKey(userProperty.Key) == true)
            {
                Debug.Log("The key is already contains UserProperties. Update the value");
                this.userProperties[userProperty.Key] = userProperty.Value;
            }
            else
            {
                this.userProperties.Add(userProperty.Key, userProperty.Value);    
            }
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
            {
                if(userId == UserManager.Instance.userID)
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable(){{userProperty.Key, userProperty.Value}});
                else
                {
                    Photon.Realtime.Player photonPlayer = Photon.Pun.PhotonNetwork.CurrentRoom.GetPlayer(dynamicCodeInRoom);
                    photonPlayer.SetCustomProperties(new Hashtable(){{userProperty.Key, userProperty.Value}});
                }
            }
#endif
        }

        public void RemoveUserProperties(string userPropertyKey, bool reqSyncData = true)
        {
            if(SalinTokens.ValidateTokenUserToken() == false)
                return;
            
            if (userProperties.ContainsKey(userPropertyKey) == false)
            {
                Debug.Log("The key is not in the UserProperties.");
                return;
            }

            userProperties.Remove(userPropertyKey);
            
#if PHOTON_UNITY_NETWORKING
            if (reqSyncData == true)
            {
                if(userId == UserManager.Instance.userID)
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable(){{userPropertyKey, null}});
                else
                {
                    Photon.Realtime.Player photonPlayer = Photon.Pun.PhotonNetwork.CurrentRoom.GetPlayer(dynamicCodeInRoom);
                    photonPlayer.SetCustomProperties(new Hashtable(){{userPropertyKey, null}});
                }
            }
#endif
        }
        
        
#if PHOTON_UNITY_NETWORKING
        public Player(Photon.Realtime.Player photonPlayer)
        {
            userProperties = (Dictionary<object, object>)photonPlayer.CustomProperties.Clone();
            
            userNickname = photonPlayer.NickName;
            dynamicCodeInRoom = photonPlayer.ActorNumber;

            if (userProperties.ContainsKey(PlayerKey.UserId))
            {
                userId = userProperties[PlayerKey.UserId].ToString();
                userProperties.Remove(PlayerKey.UserId);
            }

            if (userProperties.ContainsKey(PlayerKey.AllowInvite))
            {
                allowInvite = (bool)userProperties[PlayerKey.AllowInvite];
                userProperties.Remove(PlayerKey.AllowInvite);
            }
            
            isMyPlayer = userId.Equals(UserManager.Instance.userID);
        }

        public void UpdatePlayerInfo(Photon.Realtime.Player photonPlayer)
        {
            userProperties = (Dictionary<object, object>)photonPlayer.CustomProperties.Clone();
            
            userNickname = photonPlayer.NickName;
            dynamicCodeInRoom = photonPlayer.ActorNumber;
            
            if (userProperties.ContainsKey(PlayerKey.UserId))
            {
                userId = userProperties[PlayerKey.UserId].ToString();
                userProperties.Remove(PlayerKey.UserId);
            }

            if (userProperties.ContainsKey(PlayerKey.AllowInvite))
            {
                allowInvite = (bool)userProperties[PlayerKey.AllowInvite];
                userProperties.Remove(PlayerKey.AllowInvite);
            }
            
            isMyPlayer = userId.Equals(UserManager.Instance.userID);
        }
#endif
    }
}