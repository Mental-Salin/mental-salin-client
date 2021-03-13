/*
작성자 : 김승훈



* Server Request
    Salin.Network.Protocol.LoginReq req = new Salin.Network.Protocol.LoginReq();
    req.userid = userid;
    Salin.Network.Protocol.Encode<Salin.Network.Protocol.LoginReq>();

** changelog **

2018-08-08
    * 새로 추가했습니다.
*/

#define USING_LITJSON

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using socket.io;
using UniRx;

#if USING_LITJSON
using LitJson;
#elif USING_MINIJSON
using MiniJSON;
#elif USING_JSON_NET
using Newtonsoft.Json;
#endif

namespace SalinSDK
{
    public class RelayServer : MessageServer
    {
        #region Valuable

        private Socket socket = null;
        private string userId = "";
        public delegate void RecivedCallback(string actionName, string data);
        private RecivedCallback recivedCallbackDelegate;

        #endregion
        
        #region Manage Connection

        public bool Connect(string appId, string userid, Action<bool> connnectCheckAction = null)
        {
            this.userId = userid;
            Protocol.VersionInfo verinfo = new Protocol.VersionInfo();
            verinfo.OS = Protocol.VersionInfo.eOS.Android;
            verinfo.Major = 0;
            verinfo.Minor = 0;
            verinfo.BuildRev = 0;
            verinfo.VC = 0;
            verinfo.PtVer = Protocol.Definition.Ver;

            Protocol.ConnectReq req = new Protocol.ConnectReq(this.userId, verinfo);
            string url = "http://" + "54.250.244.221:8080/?" + req.GetQueryString();      // live for softbank 
            socket = Socket.Connect(url);
            if (socket)
            {
                socket.On(SystemEvents.connect, () => {
                    Debug.Log("Connected RelayServer");

                    connnectCheckAction(true);
                    
                    SalinCallbacks.OnRelayServerCallbackEvent(RelayServerKey.Connect);

                    socket.On("error", (string data) => {
                        Debug.Log("error : " + data);
                    });

                    socket.On("server-error", OnReciveError);

                    socket.On("event", OnReciveEvent);
                });

                socket.On(SystemEvents.disconnect, () => {
                    Debug.Log("Disconnect RelayServer");

                    connnectCheckAction(false);
                    
                    SalinCallbacks.OnRelayServerCallbackEvent(RelayServerKey.Disconnect);

                    socket.On("error", (string data) => {
                        Debug.Log("error : " + data);
                    });

                    socket.On("server-error", OnReciveError);

                    socket.On("event", OnReciveEvent);
                });
            }
            return socket != null ? true : false;
        }

        public void Close()
        {
            
        }

        #endregion

        #region Manage Callback
        
        private void OnReciveEvent(string jsonString)
        {
            Debug.Log("OnReciveEvent (message) : " + jsonString);

            string actionName = GetActionName(jsonString);
            Debug.Log("ActionName=" + actionName);

            SalinCallbacks.OnRelayServerCallbackEvent(actionName, jsonString);
        }

        private void OnReciveError(string jsonString)
        {
            Debug.Log("OnReciveError (message) : " + jsonString);

            string actionName = GetActionName(jsonString);
            Debug.Log("ActionName=" + actionName);

            Debug.Log("received (server-error) : " + jsonString);
            
            string ID = null;
            int idx;
            idx = jsonString.IndexOf(":");
            ID = jsonString.Substring(idx + 2);
            idx = ID.IndexOf(",");
            ID = ID.Remove(idx - 1);
            
            idx = jsonString.IndexOf("error") + 8;
            string errorCode = jsonString.Substring(idx, jsonString.Length - (idx + 2));

            SalinCallbacks.OnRelayServerCallbackError(actionName, errorCode);
        }
        
        // 메시지 Action Name을 얻습니다.
        public string GetActionName(string jsonString)
        {
            int length = jsonString.Length;
            if (length < 1)
                return "";
            int indexOf = jsonString.IndexOf("\"action\":\"");
            Debug.Log("indexOf=" + indexOf.ToString());
            if (indexOf == -1)
                return "";

            int startIndex = indexOf + 10;
            int endIndex = -1;
            for (int i = startIndex; i < length; i++)
            {
                if (jsonString[i] == '"')
                {
                    endIndex = i;
                    break;
                }
            }
            return jsonString.Substring(startIndex, endIndex - startIndex);
        }

        #endregion

        #region Send Protocol Base
        public bool SendToUser<T>(T requestNode)
        {
            if (requestNode == null)
            {
                return false;
            }

            Protocol.ProtocolBase protocolBase = requestNode as Protocol.ProtocolBase;
            protocolBase.senderId = this.userId;
            string requestJsonString = Util.Encode<T>(requestNode);
            socket.EmitJson("to", requestJsonString);
            
            return true;
        }

        public bool SendToAll<T>(T requestNode)
        {
            if (requestNode == null)
            {
                return false;
            }

            Protocol.ProtocolBase protocolBase = requestNode as Protocol.ProtocolBase;
            protocolBase.senderId = this.userId;

            string requestJsonString = Util.Encode<T>(requestNode);

            Debug.Log("requestJsonString=" + requestJsonString);

            socket.EmitJson("broadcast", requestJsonString);

            return true;
        }

        #endregion

        #region Relay Server Protocol

        public class Protocol
        {

            #region Essential Protocol
            public class Definition
            {
                public const string SecretKey = "";
                public const int Ver = 20180808;
            }

            public class ProtocolBase
            {
                protected ProtocolBase(string _action)
                {
                    this.action = _action;
                }

                public string action = "";
                public string senderId = "";
            }

            public class TargetProcotolBase : ProtocolBase
            {
                protected TargetProcotolBase(string _action)
                    : base(_action)
                {

                }

                public string targetUserId = "";
            }

            public class CheckLatency
            {
                public int serverProcessMsTime = 0;    // 서버에서 메시지 처리한 시간(MS)
            }

            public class VersionInfo
            {
                public enum eOS
                {
                    Android = 1,
                    iOS = 2,
                };

                public eOS OS = eOS.Android;        // OS
                public int Major = 0;               // Majer
                public int Minor = 0;               // Minor
                public int BuildRev = 0;            // SVN(GIT) Revision
                public int VC = 0;                  // Version Code
                public int PtVer = Definition.Ver;  // Protocol Version
            }

            // login.
            [Action("Connect")]
            [EscapeDataString]
            public class ConnectReq
            {
                public ConnectReq(string _userId, VersionInfo _ver)
                {
                    this.userId = _userId;
                    this.Ver = _ver;
                }
                public string userId = "";              // User ID
                public VersionInfo Ver;                 // Version Information

                public string GetQueryString()
                {
                    return "userid=" + this.userId + "&ver=" + Util.Encode<VersionInfo>(this.Ver);
                }
            }

            public class ConnectRes
            {
                public bool? is_Result { get; set; }
            }

            // 
            [Action("broadcast")]
            public class BroadcastMessageReq : ProtocolBase
            {
                public BroadcastMessageReq() :
                    base("BroadcastMessageReq")
                {

                }

                public string message = "";

                public override string ToString()
                {
                    return "message=" + this.message;
                }
            }

            [Action("to")]
            public class ToMesssageReq : TargetProcotolBase
            {
                public ToMesssageReq() :
                   base("ToMesssageReq")
                {

                }

                public string message = "";
            }

            #endregion

            #region Custom Protocol

            // 친구 초대 메시지 요청합니다.
            public class InviteRoom : TargetProcotolBase
            {
                public InviteRoom() :
                    base("InviteRoom")
                {
                }

                public string roomName;
                public string hostName;
            }

            public class RespondInviteRoom : TargetProcotolBase
            {
                public RespondInviteRoom() :
                    base("RespondInviteRoom")
                {
                }
                
                public string roomName;
                public bool acceptInvite = false;
            }
            
            #endregion
        }

        #endregion

        #region Request Protocol

        public void SendMessageToTarget(string targetUserID, string message)
        {
            Protocol.ToMesssageReq req = new Protocol.ToMesssageReq();
            req.targetUserId = targetUserID;
            req.message = message;
            SendToUser<Protocol.ToMesssageReq>(req);
        }

        public void InvitePlayerToRoom(string targetUserID, string roomName, string hostName = "")
        {
            Protocol.InviteRoom req = new Protocol.InviteRoom();
            req.targetUserId = targetUserID;
            req.roomName = roomName;
            req.hostName = hostName;
            SendToUser<Protocol.InviteRoom>(req);
        }

        public void RespondInviteRoom(string targetUserID, string roomName, bool acceptInvite)
        {
            Protocol.RespondInviteRoom req = new Protocol.RespondInviteRoom();
            req.targetUserId = targetUserID;
            req.roomName = roomName;
            req.acceptInvite = acceptInvite;
            SendToUser<Protocol.RespondInviteRoom>(req);
        }
        
        #endregion
    }

    #region Error Code

    public enum eReturnCode
    {
        Error = 500,                        // 500 Error
    }
    
    #endregion

    #region Attribute

    public class ActionAttribute : Attribute
    {
        string action = null;

        public ActionAttribute(string action)
        {
            this.action = action;
        }

        public string GetAction()
        {
            return action;
        }
    }

    public class EscapeDataStringAttribute : Attribute { }

    public class IgnoreParsingErrorAttribute : Attribute { }

    #endregion

    #region Utility

    public class Util
    {
        public static string ByteArrayToString(byte[] ba)
        {
            //return BitConverter.ToString(ba);

            StringBuilder hex = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                hex.Append(ba[i].ToString("x2"));
                hex.Append(" ");
            }
            return hex.ToString();
        }

        public static string Encode<T>(T obj)
        {
#if USING_LITJSON
            return JsonMapper.ToJson(obj);
#elif USING_MINIJSON
            return MiniJSON.Json.Serialize(obj);
#elif USING_JSON_NET
            string text = JsonConvert.SerializeObject(obj);
            Debug.Log("Encode(json) :" + text);
            return text;
#else
            return "";
#endif
        }

        public static T Decode<T>(string str)
        {
#if USING_LITJSON
            return JsonMapper.ToObject<T>(str);
#elif USING_MINIJSON
            return MiniJSON.Json.Deserialize(str);
#elif USING_JSON_NET
            return JsonConvert.DeserializeObject<T>(str);
#else
            return default(T);
#endif

        }

        public static void Dump<T>(T myClass)
        {
            if (null == myClass)
                return;

            Type myClassType = myClass.GetType();
            System.Reflection.PropertyInfo[] properties = myClassType.GetProperties();

            string result = "";
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                System.Object value = property.GetValue(myClass, null);
                if (null != value)
                    result += property.Name + "=" + property.GetValue(myClass, null) + " ";
            }

            UnityEngine.Debug.Log(result);
        }

        public static string ToString<T>(T myClass)
        {
            if (null == myClass)
                return "";

            Type myClassType = myClass.GetType();
            System.Reflection.PropertyInfo[] properties = myClassType.GetProperties();

            string result = "";
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                System.Object value = property.GetValue(myClass, null);
                if (null != value)
                    result += property.Name + "=" + property.GetValue(myClass, null) + " ";
            }

            return result;
            //UnityEngine.Debug.Log(result);
        }
    }

    #endregion

    #region etc.

    public class ErrorRes
    {
        public enum eServerStatus
        {
            Ok,                         // 정상
            ServerChecking,             // 서버 점검 중
            ServerEmergency,            // 서버 긴급 점검
            EndOfService,               // 서비스 종료
        }
        public int ServerStatus = (int)eServerStatus.Ok;    // 서버 상태.
        public string Msg = "";                             // 
        public string Param = "";                           //
    }



    public class ResourcesPatchInfo
    {
        public int Ver = 0;                                 // Version
        public string URL = "";                             // URL
    }



    // 서버 설정 정보 얻기
    [Action("ServerStatusInfo")]
    public class ServerStatusInfoReq
    {

    }

    public class ServerStatusInfoRes
    {
        public int? is_health { get; set; }        // 
    }

    #endregion
}
