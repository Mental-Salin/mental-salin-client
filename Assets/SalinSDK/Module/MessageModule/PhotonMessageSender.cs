using System;

namespace SalinSDK
{
#if PHOTON_UNITY_NETWORKING
    
    using Photon.Pun;
    using UnityEngine;
    
    public class PhotonMessageSender : IMessageSendable
    {
        private PhotonView _pv;
        private PhotonView pv
        {
            get
            {
                if (PhotonNetwork.InRoom == false)
                {
                    Debug.Log("You are not in a Room. you have to join room first");
                    return null;
                }

                if (_pv == null)
                {
                    GameObject obj = GameObject.Find("photonViewForRPC");
                    if(obj == null)
                        obj = new GameObject("photonViewForRPC");
                    
                    _pv = obj.GetComponent<PhotonView>();
                    if(_pv == null)
                        _pv = obj.AddComponent<PhotonView>();
                    
                    
                    if(obj.GetComponent<MessageSenderObj>() == null)
                        obj.AddComponent<MessageSenderObj>();
                    
                    _pv.ViewID = 7777;
                }

                return _pv;
            }
        }
        
        public void SendMessage<T>(string userToken, string targetUserId, T data) where T : MessageData
        {
            data.targetrId = targetUserId;
            SendRPC<T>(data);
        }
        
        public void SendMessage<T>(string userToken, Player player, T data) where T : MessageData
        {
            SendMessage<T>(userToken, player.userId, data);
        }

        public void SendBroadcastMessage<T>(string userToken, T data, SendTarget sendTargets) where T : MessageData
        {
            SendRPC<T>(data, sendTargets);
        }

        private void SendRPC<T>(T data, SendTarget sendTargets = SendTarget.ToTarget) where T : MessageData
        {
            data.senderId = UserManager.Instance.userID;
            string requestJsonString = MessageSenderUtil.Encode<T>(data);

            RpcTarget target = RpcTarget.Others;

            switch (sendTargets)
            {
                case SendTarget.ToAll:
                    target = RpcTarget.All;
                    break;
                    
                case SendTarget.ToOthers:
                    target = RpcTarget.Others;
                    break;
                
                default:
                    target = RpcTarget.Others;
                    break;
            }
            
            pv?.RPC("RecvRPCData", target, requestJsonString, data.action);
        }
    }

    public class MessageSenderObj : MonoBehaviour
    {
        [PunRPC]
        public void RecvRPCData(string data, string action)
        {
            string typeName = this.GetType().Namespace + "." + action;
            var obj = MessageSenderUtil.Decode(data, Type.GetType(typeName));

            SalinCallbacks.OnPhotonCallbackEvent(new PhotonEvent(PhotonAction.Message, obj));
        }
    }

    public class MessageSenderUtil
    {
        public static string Encode<T>(T obj) where T : MessageData
        {
            return JsonUtility.ToJson(obj);
        }

        public static T Decode<T>(string str) where T : MessageData
        {
            return JsonUtility.FromJson<T>(str);
        }

        public static MessageData Decode(string str, Type type)
        {
            var obj = JsonUtility.FromJson(str, type);

            if (obj is MessageData)
                return obj as MessageData;
                    
            return null;
        }
    }
    
#else
    #region Fake class
    public class PhotonMessageSender : IMessageSendable
    {
        public void SendMessage<T>(string userToken, string targetUserId, T data) where T : MessageData { }
        public void SendMessage<T>(string userToken, Player player, T data) where T : MessageData { }
        public void SendBroadcastMessage<T>(string userToken, T data, SendTarget sendTarget) where T : MessageData { }
    }
    #endregion
#endif
}