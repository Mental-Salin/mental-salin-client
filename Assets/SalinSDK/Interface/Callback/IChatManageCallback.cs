using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalinSDK
{
    interface IChatManageCallback
    {
        void OnChatConnect();
        void OnChatConnectFail(ErrorCode errorCode);

        void OnChatDisconnect();

        void OnChatReceiveMessage(string[] senders, object[] messages, bool isPublic);

        void OnChatUserConnect(string channel, string user);
        void OnChatUserDisconnect(string channel, string user);
    }
}
