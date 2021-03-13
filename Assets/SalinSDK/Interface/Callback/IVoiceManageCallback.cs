using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SalinSDK
{
    interface IVoiceManageCallback
    {
        void OnVoiceConnect();
        void OnVoiceConnectFail(ErrorCode errorCode);

        void OnVoiceDisconnect();

        void OnVoiceUserConnect(string _nickName, string _userID);
        void OnVoiceUserConnectFail(ErrorCode errorCode);
        void OnVoiceUserDisconnect(string _userID);
        void OnVoiceUserDisconnectFail(ErrorCode errorCode);
    }
}
