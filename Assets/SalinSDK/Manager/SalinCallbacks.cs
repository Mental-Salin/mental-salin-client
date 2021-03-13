#define USE_RELAY_SERVER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using SalinSDK;
using SalinSDK.ExtensionMethod;
using RelayProtocol = SalinSDK.RelayServer.Protocol;

namespace SalinSDK
{
    public class SalinCallbacks : MonoBehaviour, IAccountManageCallback, IConnectionCallback, IFriendManageCallback, IObjectManageCallback, IPlayerManageCallback, IRoomManageCallback, IOutGamePlayerManageCallback, IChatManageCallback, IVoiceManageCallback, ILobbyManageCallback
    {
        #region Valuable
        
        private static List<SalinCallbacks> callbacks = new List<SalinCallbacks>();
        
        #endregion
        
        // Callbacks        
        #region FriendManage Callback
        public virtual void OnMyFriendList(List<Friend> friendList)
        {

        }

        public virtual void OnMyFriendListFail(ErrorCode errorCode)
        {

        }

        public virtual void OnAddFriend(UserInfo info)
        {

        }

        public virtual void OnAddFriendFail(ErrorCode errorCode)
        {

        }

        public virtual void OnRemoveFriend(UserInfo info)
        {

        }

        public virtual void OnRemoveFriendFail(ErrorCode errorCode)
        {

        }

        public virtual void OnSearchFriend(UserInfo info)
        {

        }

        public virtual void OnSearchFriendFail(ErrorCode errorCode)
        {

        }

        public virtual void OnSearchFriendList(List<Friend> friendList)
        {

        }

        public virtual void OnSearchFriendListFail(ErrorCode errorCode)
        {

        }
        public virtual void OnRequestFriend(UserInfo info)
        {

        }
        public virtual void OnRequestFriendFail(ErrorCode errorCode)
        {

        }

        public virtual void OnResponseFriend(UserInfo info)
        {

        }
        public virtual void OnResponseFriendFail(ErrorCode errorCode)
        {

        }

        public virtual void OnUpdateState(List<Friend> friendList)
        {

        }
        public virtual void OnUpdateStateFail(ErrorCode errorCode)
        {

        }
        #endregion

        #region Connection Callback

        public virtual void OnConnectedMainServer(string appToken)
        {

        }

        public virtual void OnConnectedMainServerFail(ErrorCode errorCode)
        {

        }

        public virtual void OnConnectedSocialServer()
        {

        }

        public virtual void OnConnectedSocialServerFail(DisconnectCause disconnectCause)
        {

        }

        public virtual void OnDisconnectedSocialServer(DisconnectCause disconnectCause)
        {
            
        }

        public virtual void OnConnectedMessageServer()
        {

        }

        public virtual void OnConnectedMessageServerFail(ErrorCode errorCode)
        {

        }

        public virtual void OnDisconnectedMessageServer()
        {
            
        }

        #endregion

        #region AccountManage Callback

        public virtual void OnLogIn(UserInfo info)
        {

        }

        public virtual void OnLogInFail(ErrorCode errorCode)
        {

        }

        public virtual void OnLogOut()
        {

        }

        public virtual void OnLogOutFail(ErrorCode errorCode)
        {

        }

        public virtual void OnSignUp()
        {

        }

        public virtual void OnSignUpFail(ErrorCode errorCode)
        {

        }

        #endregion

        #region ObjectManage Callback

        public virtual void OnSyncInstance(int netId)
        { }

        public virtual void OnSyncInstanceFail(ErrorCode errorCode)
        { }

        public virtual void OnRoomPropertiesChanged(KeyValuePair<string, object> roomProperties)
        { }

        public virtual void OnSendMessage()
        { }

        public virtual void OnSendMessageFail(ErrorCode errorCode)
        { }

        public virtual void OnSendBroadcastMessage()
        { }

        public virtual void OnSendBroadcastMessageFail(ErrorCode errorCode)
        { }

        public virtual void OnReceiveMessage(string message)
        { }

        #endregion

        #region PlayerManage Callback

        public virtual void OnUserBlock()
        { }
        public virtual void OnUserBlockFail(ErrorCode errorCode)
        { }
        public virtual void OnUserKick(Player kickedPlayer)
        { }
        public virtual void OnUserKickFail(ErrorCode errorCode)
        { }

        #endregion

        #region RoomManage Callback
        public virtual void OnCreateRoom()
        { }
        public virtual void OnCreateRoomFail(ErrorCode errorCode)
        { }
        public virtual void OnJoinRoom()
        { }
        public virtual void OnJoinRoomFail(ErrorCode errorCode)
        { }
        public virtual void OnLeaveRoom()
        { }
        public virtual void OnChangePassword()
        { }

        public virtual void OnUpdateRoomProperties(Dictionary<object, object> changeProp)
        { }
        public virtual void OnPlayerEnteredRoom(Player enterPlayer)
        { }
        public virtual void OnPlayerLeftRoom(Player leftPlayer)
        { }
        #endregion

        
        #region LobbyManage Callback
        
        public virtual void OnJoinedLobby()
        {
        }

        public virtual void OnLeftLobby()
        {
        }

        public virtual void OnRoomListUpdate()
        {
        }

        #endregion
        
        
        #region Message Callback
        
        public virtual void OnReceiveMessage<T>(T data) where T : MessageData
        { }
        
        #endregion
        
        #region Voice callback
        public virtual void OnVoiceConnect()
        { }
        public virtual void OnVoiceConnectFail(ErrorCode errorCode)
        { }
        public virtual void OnVoiceDisconnect()
        { }
        public virtual void OnVoiceUserConnect(string _nickName, string _userID)
        { }
        public virtual void OnVoiceUserConnectFail(ErrorCode errorCode)
        { }
        public virtual void OnVoiceUserDisconnect(string _userID)
        { }
        public virtual void OnVoiceUserDisconnectFail(ErrorCode errorCode)
        { }
        #endregion

        #region Chat Callback
        public virtual void OnChatConnect()
        { }
        public virtual void OnChatConnectFail(ErrorCode errorCode)
        { }
        public virtual void OnChatDisconnect()
        { }
        public virtual void OnChatReceiveMessage(string[] senders, object[] messages, bool isPublic)
        { }
        public virtual void OnChatUserConnect(string channel, string user)
        { }
        public virtual void OnChatUserDisconnect(string channel, string user)
        { }

        #endregion

        #region Out Game PlayerManage Callback
        public virtual void OnReceiveInvitePlayerToRoom(string senderId, string roomName, string hostName)
        {
            
        }

        public virtual void OnReceiveRespondInviteRoom(string senderId, string roomName, bool acceptInvite)
        {
            
        }

        public virtual void OnUserNotFound(string action, string error)
        {
            
        }

        #endregion


        // Callback logic

        #region Apex Api Callback Logic

        #region FunctionEventCallback

        #region Voice Event
        public static void OnConnectVoice()
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnVoiceConnect();
            }
        }
        public static void OnConnectVoiceFail(ErrorCode errorCode)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnVoiceConnectFail(errorCode);
            }
        }
        public static void OnDisconnectVoice()
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnVoiceDisconnect();
            }
        }
        public static void OnUserConnectVoice(string _nickName, string _userID)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnVoiceUserConnect(_nickName, _userID);
            }
        }
        public static void OnUserConnectVoiceFail(ErrorCode errorCode)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnVoiceUserConnectFail(errorCode);
            }
        }
        public static void OnUserDisconnectVoice(string _userID)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnVoiceUserDisconnect(_userID);
            }
        }
        public static void OnUserDisconnectVoice(ErrorCode errorCode)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnVoiceUserDisconnectFail(errorCode);
            }
        }
        #endregion

        #region Chat Event
        public static void OnConnectChat()
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))  
            {
                item.OnChatConnect();
            }
        }
        public static void OnConnectChatFail(ErrorCode errorCode)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnChatConnectFail(errorCode);
            }
        }

        public static void OnDisconnectChat()
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnChatDisconnect();
            }
        }

        public static void OnReceiveMessage(string[] senders, object[] messages, bool isPublic)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnChatReceiveMessage(senders,messages,isPublic);
            }
        }

        public static void OnUserConnectChat(string channel, string user)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnChatUserConnect(channel, user);
            }
        }

        public static void OnUserDisconnectChat(string channel, string user)
        {
            foreach (var item in new List<SalinCallbacks>(callbacks))
            {
                item.OnChatUserDisconnect(channel, user);
            }
        }
        #endregion

        #endregion


        /// <summary>
        /// Transmitter에서 데이터, 데이터 타입등을 받아옵니다.
        /// </summary>
        /// <param name="responData">데이터</param>
        /// <param name="reqType">데이터 타입</param>
        public static void OnEvent(ResponseData responData, RequestDataType reqType)
        {
            switch (reqType)
            {
                case RequestDataType.APPTOKEN:
                    {
                        string appToken = string.Empty;
                        if (responData.jsonData.JsonDataContainsKey(SalinAPIKey.apptoken))
                        {
                            appToken = responData.jsonData[SalinAPIKey.apptoken].ToString();
                        }
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            item.OnConnectedMainServer(appToken);
                        }
                    }
                    break;
                case RequestDataType.LOGIN:
                    {
                        UserInfo user = UserInfo.Convert(responData.jsonData);
                        UserManager.Instance.UpdateUserInfo(user);
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            item.OnLogIn(user);
                        }
                        XRSocialSDK.myPlayer.UpdatePlayerInfo(user);
                    }
                    break;
                case RequestDataType.LOGOUT:
                    {
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            item.OnLogOut();
                        }
                        UserManager.Instance.UpdateUserInfo(null);
                    }
                    break;
                case RequestDataType.SIGNUP:
                    {
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            item.OnSignUp();
                        }
                    }
                    break;
                case RequestDataType.REQUESTFRIEND:
                    {
                        UserInfo userInfo = new UserInfo();
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            item.OnRequestFriend(userInfo);
                        }
                    }
                    break;
                case RequestDataType.RESPONSEFRIEND:
                    {
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            UserInfo userInfo = new UserInfo();
                            item.OnResponseFriend(userInfo);
                        }
                    }
                    break;
                case RequestDataType.UPDATESTATE:
                    {
                        JsonData json = responData.jsonData;
                        if (json.JsonDataContainsKey(JsonKey.friend))
                        {
                            List<Friend> tmpList = Friend.ConvertToList(json[JsonKey.friend]);

                            foreach (var item in new List<SalinCallbacks>(callbacks))
                            {
                                item.OnUpdateState(tmpList);
                            }
                        }
                    }
                    break;
                case RequestDataType.FRIENDLIST:
                    {

                    }
                    break;
                case RequestDataType.REMOVEFRIEND:
                    {
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            item.OnRemoveFriend(new UserInfo());
                        }
                    }
                    break;
                case RequestDataType.SEARCHFRIEND:
                    {
                        foreach (var item in new List<SalinCallbacks>(callbacks))
                        {
                            UserInfo userInfo = new UserInfo();
                            item.OnSearchFriend(userInfo);
                        }
                    }
                    break;
                case RequestDataType.SEARCHFRIENDLIST:
                    {
                        JsonData json = responData.jsonData;

                        if (json.JsonDataContainsKey(JsonKey.user))
                        {
                            List<Friend> tmpList = Friend.ConvertToList(json[JsonKey.user]);
                            foreach (var item in new List<SalinCallbacks>(callbacks))
                            {
                                item.OnSearchFriendList(tmpList);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 데이터 오류 이벤트
        /// 데이터 통신이 실패하면 들어오는 함수입니다.
        /// </summary>
        /// <param name="errorCode">에러 코드</param>
        /// <param name="reqType">데이터 타입</param>
        public static void OnError(ErrorCode errorCode, RequestDataType reqType)
        {
            string errorMsg = Enum.GetName(typeof(ErrorCode), errorCode);
            Debug.LogWarning(errorMsg);
            switch (reqType)
            {
                case RequestDataType.NONE:
                    break;
                case RequestDataType.LOGIN:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnLogInFail(errorCode);
                    }
                    break;
                case RequestDataType.LOGOUT:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnLogOutFail(errorCode);
                    }
                    break;
                case RequestDataType.SIGNUP:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnSignUpFail(errorCode);
                    }
                    break;
                case RequestDataType.UPDATESTATE:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnUpdateStateFail(errorCode);
                    }
                    break;
                case RequestDataType.REQUESTFRIEND:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnRequestFriendFail(errorCode);
                    }
                    break;
                case RequestDataType.RESPONSEFRIEND:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnResponseFriendFail(errorCode);
                    }
                    break;
                case RequestDataType.FRIENDLIST:
   
                    break;
                case RequestDataType.REMOVEFRIEND:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnRemoveFriendFail(errorCode);
                    }
                    break;
                case RequestDataType.SEARCHFRIEND:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnSearchFriendFail(errorCode);
                    }
                    break;
                case RequestDataType.SEARCHFRIENDLIST:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                    {
                        item.OnSearchFriendListFail(errorCode);
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Salin Relayserver Callback Logic
#if USE_RELAY_SERVER
        public static void OnRelayServerCallbackEvent(string actionName, string data = "")
        {
            switch (actionName)
            {
                case RelayServerKey.Connect:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnConnectedMessageServer();
                    break;
            
                case RelayServerKey.Disconnect:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnDisconnectedMessageServer();
                    break;
            
                case "InviteRoom":
                    if(XRSocialSDK.myPlayer.allowInvite == false)
                        return;
                    RelayProtocol.InviteRoom inviteRoom = Util.Decode<RelayProtocol.InviteRoom>(data);
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnReceiveInvitePlayerToRoom(inviteRoom.senderId, inviteRoom.roomName, inviteRoom.hostName);
                    break;
            
                case "RespondInviteRoom":
                    RelayProtocol.RespondInviteRoom respondInviteRoom = Util.Decode<RelayProtocol.RespondInviteRoom>(data);
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnReceiveRespondInviteRoom(respondInviteRoom.senderId, respondInviteRoom.roomName, respondInviteRoom.acceptInvite);
                    break;
            }
        }
        public static void OnRelayServerCallbackError(string actionName, string error)
        {
            switch (error)
            {
                case "user not found":
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnUserNotFound(actionName, error);
                    break;
            }
        }
#endif
        #endregion
        
        #region Photon Callback Logic
#if PHOTON_UNITY_NETWORKING
        public static void OnPhotonCallbackEvent(PhotonEvent photonEvent)
        {
            switch (photonEvent.action)
            {
                case PhotonAction.Connect:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnConnectedSocialServer();
                    break;
            
                case PhotonAction.Disconnect:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnDisconnectedSocialServer(photonEvent.disconnectCause);
                    break;
                
                case PhotonAction.CreateRoom:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnCreateRoom();
                    break;
            
                case PhotonAction.JoinRoom:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnJoinRoom();
                    break;
            
                case PhotonAction.LeaveRoom:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnLeaveRoom();
                    break;
                
                case PhotonAction.JoinLobby:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnJoinedLobby();
                    break;
            
                case PhotonAction.LeaveLobby:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnLeftLobby();
                    break;
                
                case PhotonAction.UpdateRoomList:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnRoomListUpdate();
                    break;
                
                case PhotonAction.ChangePassword:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnChangePassword();
                    break;
                
                case PhotonAction.BlockPlayer:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnUserBlock();
                    break;
                
                case PhotonAction.KickPlayer:
                    Player kickedPlayer = new Player(photonEvent.pData);
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnUserKick(kickedPlayer);
                    break;
                
                case PhotonAction.UpdateRoomProperties:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnUpdateRoomProperties(photonEvent.prop);
                    break;
                
                case PhotonAction.PlayerEnteredRoom:
                    Player enterPlayer = XRSocialSDK.GetUser(photonEvent.data);
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnPlayerEnteredRoom(enterPlayer);
                    break;
                
                case PhotonAction.PlayerLeftRoom:
                    Player leftPlayer = new Player(photonEvent.pData);
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnPlayerLeftRoom(leftPlayer);
                    break;
                
                case PhotonAction.Message:
                    if(string.IsNullOrEmpty(photonEvent.mData.targetrId) == false &&
                       photonEvent.mData.targetrId != UserManager.Instance.userID)
                        return;
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnReceiveMessage(photonEvent.mData);
                    break;
            }
        }

        public static void OnPhotonCallbackError(PhotonEvent photonEvent)
        {
            
            switch (photonEvent.action)
            {
                case PhotonAction.Connect:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnConnectedSocialServerFail(photonEvent.disconnectCause);
                    break;
            
                case PhotonAction.CreateRoom:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnCreateRoomFail(photonEvent.errorCode);
                    break;
            
                case PhotonAction.JoinRoom:
                    foreach (var item in new List<SalinCallbacks>(callbacks))
                        item.OnJoinRoomFail(photonEvent.errorCode);
                    break;
            }
        }
#endif
        #endregion
        
        
        // MonoBehaviour Lifecycle 
        
        public void OnEnable()
        {
            callbacks.Add(this);
        }
        public void OnDisable()
        {
            callbacks.Remove(this);
        }

        
    }
}

    #region Photon Event 
#if PHOTON_UNITY_NETWORKING
    public struct PhotonEvent
    {
        public PhotonAction action;
        public DisconnectCause disconnectCause;
        public ErrorCode errorCode;

        public string data;
        public Dictionary<object, object> prop;
        public MessageData mData;
        public Photon.Realtime.Player pData;
        
        public PhotonEvent(PhotonAction action)
        {
            this.action = action;
            disconnectCause = DisconnectCause.None;
            errorCode = ErrorCode.None;
            data = "";
            prop = new Dictionary<object, object>();
            mData = null;
            pData = null;
        }
        
        public PhotonEvent(PhotonAction action, ErrorCode errorCode): this(action)
        {
            this.errorCode = ErrorCode.None;
        }
        
        public PhotonEvent(PhotonAction action, DisconnectCause disconnectCause) : this(action)
        {
            this.disconnectCause = DisconnectCause.None;
        }
        
        public PhotonEvent(PhotonAction action, string data): this(action)
        {
            this.data = data;
        }
        
        public PhotonEvent(PhotonAction action, Dictionary<object, object> prop): this(action)
        {
            this.prop = prop;
        }
        
        public PhotonEvent(PhotonAction action, MessageData mData): this(action)
        {
            this.mData = mData;
        }
        
        public PhotonEvent(PhotonAction action, Photon.Realtime.Player pData): this(action)
        {
            this.pData = pData;
        }
    }
#endif
    #endregion