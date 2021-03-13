using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SalinSDK;
using UnityEngine.UI;
using System;

public class DemoChatReceiveCallback : SalinCallbacks
{
    public DemoChatUIControl ChatUICon;

    [NonSerialized]
    public string channelname;
    [NonSerialized]
    public string mynickname;
    [NonSerialized]
    public bool IsOnlyChat = false;

    // Start is called before the first frame update
    public void Update()
    {
        ChatManager.Service();
    }

    #region Chat
    public override void OnChatConnect()
    {
        if (!IsOnlyChat)
        {
            channelname = XRSocialSDK.currentRoom.RoomName;
            mynickname = UserManager.Instance.userInfo.userNickname;
        }

        Debug.Log("OnChatConnect");
        string msg = "<b><color=#ffa500ff><" + channelname + "> 채널에 접속했습니다.</color></b>" + "\n";
        ChatUICon.UpdateChatBox(msg);
    }

    public override void OnChatConnectFail(ErrorCode errorCode)
    {
        string msg = "<b><color=#ff0000ff>" + channelname + "채널 접속에 실패했습니다.</color></b>" + "\n";
        ChatUICon.UpdateChatBox(msg);
    }
    public override void OnChatDisconnect()
    {
        string msg = "<b><color=#ff0000ff>" + channelname + "채널 연결이 끊겼습니다.</color></b>" + "\n";

        if (ChatUICon == null)
            return;

        ChatUICon.UpdateChatBox(msg);

        
    }

    public override void OnChatReceiveMessage(string[] senders, object[] messages, bool isPublic)
    {
        string message = "";

        if (isPublic)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                if(senders[i] == mynickname)
                    message += "<b><i>" + senders[i] + ":" + messages[i] + "</i></b>\n";
                else
                    message += senders[i] + ":" + messages[i] + "\n";
            }
        }
        else
        {
            for (int i = 0; i < senders.Length; i++)
            {
                if (senders[i] == mynickname)
                    message += "[내가 보낸 귓속말] <b><color=blue>" + senders[i] + ":" + messages[i] + "</color></b>\n";
                else
                    message += "[귓속말] <b><color=green>" + senders[i] + ":" + messages[i] + "</color></b>\n";
            }
        }

        ChatUICon.UpdateChatBox(message);
    }
    public override void OnChatUserConnect(string channel, string user)
    {
        string msg = "<b><color=#008080ff>" + user + "님이 접속했습니다.</color></b>" + "\n";
        ChatUICon.UpdateChatBox(msg);
    }
    public override void OnChatUserDisconnect(string channel, string user)
    {
        string msg = "<b><color=#008080ff>" + user + "님이 퇴장했습니다.</color></b>" + "\n";
        ChatUICon.UpdateChatBox(msg);
    }
    #endregion

}
