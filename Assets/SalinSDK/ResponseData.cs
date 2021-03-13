using UnityEngine;
using System.Collections.Generic;
using LitJson;

namespace SalinSDK
{
    /// <summary>
    /// 딕셔너리를 상속받은 클래스 입니다. 제너릭 타입은 <string, object> 입니다.
    /// 코드의 가독성을 위하여 제작해둔 클래스입니다.
    /// 추가로 서버에서 응답받은 json 정보를 들고 있습니다.
    /// </summary>
    public class ResponseData : Dictionary<string, object>
    {
        public int errorCode { get; private set; }

        public JsonData jsonData { get; private set; }
        public string jsonStr { get; private set; }

        public bool isSuccess
        {
            get { return errorCode.Equals((int)ErrorCode.Success); }
        }

        public void SetErrorCode(int _errorCode)
        {
            errorCode = _errorCode;
        }

        public void SetJsonData(JsonData _jsonData)
        {
            jsonData = _jsonData;
        }

        public void SetJsonString(string _jsonStr)
        {
            jsonStr = _jsonStr;
        }
    }

}
