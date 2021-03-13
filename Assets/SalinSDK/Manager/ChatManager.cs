using System;
using UnityEngine;

#if PHOTON_UNITY_NETWORKING

using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;

#endif

namespace SalinSDK
{
    public static class ChatManager
    {
        static ChatListener _chatListener;
        static ChatListener chatListener
        {
            get
            {
                if(_chatListener == null)
                {
                    return null;
                }
                return _chatListener;
            }
        }

        static string channelName;
        static string nickName;

        /// <summary>
        /// 채팅 서버 연결 함수 
        /// ChatManage에 다른 기능들은 Connect 이후 사용 가능합니다.
        /// </summary>
        /// <param name="_channelName">접속할 채널 이름</param>
        /// <param name="_nickName">닉네임</param>
        public static void Connect(string _channelName, string _nickName)
        {
            nickName = _nickName;
            channelName = _channelName;

            if(_chatListener == null)
            {
                _chatListener = new ChatListener();
            }
            chatListener.Connect(_channelName, _nickName);
        }

        /// <summary>
        /// 서버와 연결되어있는지 확인하는 함수입니다. 
        /// </summary>
        /// <returns>서버와 연결되어 있으면 True 아니면 False를 반환합니다.</returns>
        public static bool IsConnect()
        {
            if(chatListener != null)
            {
                return chatListener.IsConnect();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 서버와의 연결을 끊습니다. 
        /// </summary>
        public static void Disconnect()
        {
            chatListener.Disconnect();
        }

        /// <summary>
        /// 채널에 접속해있는 사용자들에게 메시지를 보냅니다.
        /// </summary>
        /// <param name="_message">보낼 메시지</param>
        public static void SendMessageInRoom(string _message)
        {
            chatListener.SendPublicMessage(_message);
        }

        /// <summary>
        /// 특정 사용자에게만 메시지를 보냅니다.
        /// </summary>
        /// <param name="_target">사용자 닉네임</param>
        /// <param name="_message">보낼 메시지</param>
        public static void SendWhisperMessageTarget(string _target, string _message)
        {
            chatListener.SendPrivateMessage(_target, _message);
        }

        /// <summary>
        /// 채팅이 들어왔는지 확인하는 함수입니다. 
        /// 이 함수는 지속적으로 불려야 합니다.
        /// </summary>
        public static void Service()
        {
            if (chatListener == null) return;
            chatListener.Service();
        } 

        public static void Reset()
        {
            _chatListener = null;
        }
    }

#if PHOTON_UNITY_NETWORKING
    public class ChatListener : IChatClientListener
    {
        AppSettings appSettings;
        ChatClient chatClient;

        private string channelName;
        private string nickName;

        bool isConnect;

        public ChatListener()
        {
            this.appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
            ChannelCreationOptions.Default.PublishSubscribers = true;
            isConnect = false;
        }

        public void Connect(string _channelName, string _nickName)
        {
            this.channelName = _channelName;
            this.nickName = _nickName;
            this.chatClient = new ChatClient(this);
#if !UNITY_WEBGL
            this.chatClient.UseBackgroundWorkerForSending = true;
#endif
            chatClient.Connect(appSettings.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(_nickName));
        }

        public bool IsConnect()
        {
            return isConnect;
        }

        public void Disconnect()
        {
            chatClient.Disconnect(ChatDisconnectCause.DisconnectByClientLogic);
        }

        public void OnConnected()
        {
          this.chatClient.Subscribe(this.channelName,0, 0);
                   
        }
        /// <summary>
        /// ChatManage가 Disconnect를 호출 한뒤 _chatListener를 바로 null값을 넣어주면
        /// OnDisconnect를 넣어 줄수 없기 때문에 _chatListener초기화 하는 함수를 여기서 불러줌
        /// </summary>
        public void OnDisconnected()
        {
            SalinCallbacks.OnDisconnectChat();
            chatClient = null;
            isConnect = false;
            ChatManager.Reset();
        }
        public void SendPublicMessage(string message)
        {
            this.chatClient.PublishMessage(this.channelName, message);
        }
        public void SendPrivateMessage(string _target, string _message)
        {
            this.chatClient.SendPrivateMessage(_target, _message);
        }
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            if (channelName.Equals(this.channelName))
            {
                SalinCallbacks.OnReceiveMessage(senders, messages, true);
            }
        }
        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            string[] senders = { sender, };
            object[] messages = { message, };
            SalinCallbacks.OnReceiveMessage(senders, messages, false);
        }
        public void OnSubscribed(string[] channels, bool[] results)
        {
            SalinCallbacks.OnConnectChat();
            isConnect = true;
        }
        public void OnUnsubscribed(string[] channels)
        {
            isConnect = false;
        }
        public void Service()
        {
            if (chatClient != null)
            {
                chatClient.Service();
            }
        }
        //유저 방 입장
        public void OnUserSubscribed(string channel, string user)
        {
            SalinCallbacks.OnUserConnectChat(channel, user);
        }
        //유저 방 나감
        public void OnUserUnsubscribed(string channel, string user)
        {
            SalinCallbacks.OnUserDisconnectChat(channel, user);
        }

    #region Not Use

        public void DebugReturn(DebugLevel level, string message)
        {
        }
        public void OnChatStateChange(ChatState state)
        {

        }
        //? 여기서 처리해야할지 친구 관련쪽에서 처리해야할지 모르겠음..
        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
        }

    #endregion
    }

#else
    #region FAKE CLASS ChatListener
    public class ChatListener
    {
        void LogError()
        {
            Debug.LogError("Not import Photon Chat Server, Can't use ChatListener");
        }
        public void Connect(string _channelName, string _nickName){ LogError(); }
        public void Disconnect(){ LogError(); }
        public void SendMessageInRoom(string _message){ LogError(); }
        public void SendPublicMessage(string _message){ LogError(); }
        public void SendPrivateMessage(string _target, string _message){ LogError(); }
        public void Service(){ LogError(); }
        public bool IsConnect() { return false; }
    }
    #endregion
#endif

}

