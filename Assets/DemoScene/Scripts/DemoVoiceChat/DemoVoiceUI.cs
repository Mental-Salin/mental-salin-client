using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;

public class DemoVoiceUI : MonoBehaviour
{
    public Button ConnectButton;
    public Button DisconnectButton;

    public InputField ChannelInputField;
    public InputField NicknameInputField;
    public InputField IdInputField;

    public InputField SoundIdInputField;
    public Text LogTextBox;
    public Scrollbar VerticalScroll;

    public Toggle MicToggle;
    public Toggle SoundToggle;

    private void Start()
    {
        
    }

    public void ClickConnectButton()
    {
        if (!string.IsNullOrWhiteSpace(ChannelInputField.text) && !string.IsNullOrWhiteSpace(IdInputField.text))
        {
            VoiceManager.Connect(ChannelInputField.text, NicknameInputField.text, NicknameInputField.text);
            ChannelInputField.interactable = false;
            IdInputField.interactable = false;
            NicknameInputField.interactable = false;
            ConnectButton.interactable = false;
            AddLogText("접속중입니다.");
        }
    }

    public void ClickDisconnectButton()
    {
        VoiceManager.Disconnect();
        DisconnectButton.interactable = false;

    }

    public void ConnectSuccess()
    {
        DisconnectButton.interactable = true;
        MicToggle.interactable = true;
        MicToggle.isOn = true;
        SoundToggle.interactable = true;
        SoundToggle.isOn = true;
    }

    public void InitSetting()
    {
        ChannelInputField.interactable = true;
        NicknameInputField.interactable = true;
        IdInputField.interactable = true;

        ChannelInputField.text = "";
        NicknameInputField.text = "";
        IdInputField.text = "";

        ConnectButton.interactable = true;
        DisconnectButton.interactable = false;
        MicToggle.interactable = false;
        MicToggle.isOn = false;
        SoundToggle.interactable = false;
        SoundToggle.isOn = false;
    }

    public void SoundOnOff(bool isOn)
    {
        if (isOn)
            VoiceManager.SoundOnAll();
        else
            VoiceManager.SoundOffAll();
    }

    public void MicOnOff(bool isOn)
    {
        if (isOn)
            VoiceManager.MicOn();
        else
            VoiceManager.MicOff();
    }

    public void UserSoundOn()
    {
        if (string.IsNullOrWhiteSpace(SoundIdInputField.text))
            return;

        VoiceManager.SoundOn(SoundIdInputField.text);
        SoundIdInputField.text = "";
    }

    public void UserSoundOff()
    {
        if (string.IsNullOrWhiteSpace(SoundIdInputField.text))
            return;

        VoiceManager.SoundOff(SoundIdInputField.text);
        SoundIdInputField.text = "";
    }

    public void AddLogText(string inputtext)
    {
        LogTextBox.text += inputtext + "\n";
        VerticalScroll.value = 0;
    }
}
