using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 최종 수정일 
/// 기존 사용하고 있는 데이터 구조 수정될수 있습니다.
/// </summary>
namespace SalinSDK
{
    public enum HTTPMethod
    {
        NONE,
        GET,
        POST,
        PUT,
        DELETE
    }

    public enum RequestDataType
    {
        NONE,
        //Connect
        APPTOKEN,
        //AccountManage
        LOGIN,
        LOGOUT,
        SIGNUP,

        //FriendManage
        UPDATESTATE,
        REQUESTFRIEND,
        RESPONSEFRIEND,
        
        FRIENDLIST,
        REMOVEFRIEND,
        SEARCHFRIEND,
        SEARCHFRIENDLIST,

        
    }

    /// <summary>
    /// 요청할 데이터를 모아둔 클래스입니다.
    /// </summary>
    public struct RequestData
    {
        private HTTPMethod httpMethod;
        private RequestDataType reqDataType;
        private Dictionary<string, string> fieldDic;
        private Dictionary<string, string> headerDic;
        private string reqStr;
        //private Action<ResponseData> protocolCallback;
        //private Action<bool> successCallback;
        private bool needBlock;

        /// <summary>
        /// 해당 구조체를 생성합니다.
        /// 이 떄 httpMethod는 반드시 필요합니다.
        /// 이 외의 데이터들은 이후에 세팅합니다.
        /// </summary>
        /// <param name="_httpMethod"> 통신방식을 지정합니다. </param>
        /// <param name="_needMyInfo"> 내 유저id값의 필요 유무를 지정합니다. 기본값은 true 입니다.</param>
        public RequestData(HTTPMethod _httpMethod, RequestDataType _requestType, bool _needMyInfo = true)
        {
            fieldDic = new Dictionary<string, string>();
            headerDic = new Dictionary<string, string>();
            httpMethod = _httpMethod;
            reqDataType = _requestType;

            reqStr = string.Empty;
            needBlock = true;

        }


        // HttpMethod Get
        public HTTPMethod GetHttpMethod()
        {
            return httpMethod;
        }

        public RequestDataType GetRequestDataType()
        {
            return reqDataType;
        }

        // ReqStr Get/Set
        public void SetReqStr(string _reqStr)
        {
            reqStr = _reqStr;
        }

        public string GetReqStr()
        {
            return reqStr;
        }


        // FieldDatas Get/Set
        public void AddField(string key, string value)
        {
            fieldDic.Add(key, value);
        }

        public Dictionary<string, string> GetFieldDatas()
        {
            return fieldDic;
        }

        // FieldDatas Get/Set
        public void AddHeader(string key, string value)
        {
            headerDic.Add(key, value);
        }

        public Dictionary<string, string> GetHeaderDatas()
        {
            return headerDic;
        }

        // NeedBlock Get/Set
        public void SetBlock(bool block)
        {
            needBlock = block;
        }

        public bool GetBlock()
        {
            return needBlock;
        }

        /// <summary>
        /// 자기 자신을 Transmitter에 보냅니다.
        /// </summary>
        public void SendRequest()
        {
            if (string.IsNullOrEmpty(SalinTokens.AppToken) &&
                reqDataType != RequestDataType.APPTOKEN)
            {
                Debug.LogWarning("appToken 값이 비어있습니다. appToken 값을 받은 후 다시 시도해주세요");
                return;
            }
            AddField(SalinAPIKey.apptoken, SalinTokens.AppToken);
            Transmitter.Instance.EnqueueReqQ(this);
        }

    }
}