using System;
using System.Collections;
using System.Collections.Generic;
using SalinSDK;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class RoomChat : SalinCallbacks
{
    public Button btnChat;
    public InputField inputChat;
    public Text chatText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!ChatManager.IsConnect())
        {
            MalinLog.Get().ShowLog("Chat server is not connected. Try to connect..");
            ChatManager.Connect(Constants.DemoChatChannel, XRSocialSDK.myPlayer.userNickname);
        }
        else
        {
            MalinLog.Get().ShowLog("Chat server is already connected.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnChatConnect()
    {
        MalinLog.Get().ShowLog("Chat server is now connected.");
    }

    public override void OnChatDisconnect()
    {
        MalinLog.Get().ShowLog("Chat server is now disconnected.");
    }

    public override void OnChatReceiveMessage(string[] senders, object[] messages, bool isPublic)
    {
        var min = Math.Min(senders.Length, messages.Length);

        for (var i = 0; i < min; i++)
        {
            AppendChatMessage($"{senders[i]} : {messages[i]}");
        }
    }

    private void AppendChatMessage(string msg)
    {
        chatText.text += msg + "\n";
    }

    public override void OnChatSendMessageSuccess()
    {
        MalinLog.Get().ShowLog("Success to send chat message.");
    }

    public override void OnChatUserConnect(string channel, string username)
    {
        MalinLog.Get().ShowLog($"Chat user connected : {username}");
        AppendChatMessage($"{username} Connected.");
    }

    public override void OnChatUserDisconnect(string channel, string username)
    {
        MalinLog.Get().ShowLog($"Chat user leave : {username}");
        AppendChatMessage($"{username} Disconnected.");

    }

    public override void OnChatError(ErrorCode errorCode)
    {
        MalinLog.Get().ShowLog($"Chat error occured : {errorCode}");
    }
    
    public void SendChatMessage()
    {
        var msg = inputChat.text;
        ChatManager.SendMessageInRoom(msg);
        inputChat.text = "";
    }
}
