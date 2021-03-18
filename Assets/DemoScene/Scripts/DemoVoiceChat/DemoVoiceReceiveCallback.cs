using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SalinSDK;

public class DemoVoiceReceiveCallback : SalinCallbacks
{
    public DemoVoiceUI VoiceUI;

    public override void OnVoiceConnect()
    {
        Debug.Log("VoiceConnect");

        string logtext = "<b>접속했습니다.</b>";
        VoiceUI.ConnectSuccess();
        VoiceUI.AddLogText(logtext);
    }
    public override void OnVoiceConnectFail(ErrorCode errorCode)
    {
        Debug.Log("Voice Fail");
        string logtext = "<color = green>" + errorCode.ToString() + "</color>";
        VoiceUI.AddLogText(logtext);
    }
    public override void OnVoiceDisconnect()
    {
        Debug.Log("Voice Disconnect");
        string logtext = "<b>연결이 해제되었습니다.</b>";
        VoiceUI.AddLogText(logtext);
        VoiceUI.InitSetting();
    }
    public override void OnVoiceUserConnect(string _nickName, string _userID)
    {
        string logtext = "<b>" + _userID + " 접속</b>";
        VoiceUI.AddLogText(logtext);
    }

    public override void OnVoiceUserDisconnect(string _userID)
    {
        string logtext = "<b>" + _userID + " 퇴장 </b>";
        VoiceUI.AddLogText(logtext);
    }

}
