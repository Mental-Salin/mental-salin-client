using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SalinSDK;

public class DemoChatUI : MonoBehaviour
{
    public Button ConnectButton;
    public Button DisconnectButton;

    public InputField ChannelInputField;
    public InputField NicknameInputField;

    public DemoChatReceiveCallback callback;

    public InputField ChatTextInputField;

    public InputField FriendInputField;
    public InputField WhisperTextInputField;

    public Button ChatSendButton;
    public Button WhisperSendButton;

    void Start()
    {
        //Connect Button Listener

        ConnectButton.onClick.AddListener(ClickConnectButton);
        DisconnectButton.onClick.AddListener(ClickDisconnectButton);
        ChatSendButton.onClick.AddListener(SendChat);
        WhisperSendButton.onClick.AddListener(SendWhisperChat);

    }

    #region button Listener
    public void ClickConnectButton()
    {
        if (!string.IsNullOrWhiteSpace(ChannelInputField.text) && !string.IsNullOrWhiteSpace(NicknameInputField.text))
        {
            callback.IsOnlyChat = true;
            callback.channelname = ChannelInputField.text;
            callback.mynickname = NicknameInputField.text;

            ChatManager.Connect(callback.channelname, callback.mynickname);
            ChannelInputField.interactable = false;
            NicknameInputField.interactable = false;
            ConnectButton.interactable = false;
            DisconnectButton.interactable = true;
        }
    }

    public void ClickDisconnectButton()
    {
        ChatManager.Disconnect();
        DisconnectButton.interactable = false;

        ChannelInputField.interactable = true;
        NicknameInputField.interactable = true;
        ChannelInputField.text = "";
        NicknameInputField.text = "";
        ConnectButton.interactable = true;
    }

    public void SendChat()
    {
        if (string.IsNullOrWhiteSpace(ChatTextInputField.text))
            return;

        ChatManager.SendMessageInRoom(ChatTextInputField.text);
        ChatTextInputField.text = "";
    }

    public void SendWhisperChat()
    {
        if (string.IsNullOrWhiteSpace(FriendInputField.text) && string.IsNullOrWhiteSpace(WhisperTextInputField.text))
            return;

        ChatManager.SendWhisperMessageTarget(FriendInputField.text, WhisperTextInputField.text);
        FriendInputField.text = "";
        WhisperTextInputField.text = "";
    }
    #endregion

}
