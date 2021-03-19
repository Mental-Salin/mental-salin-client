using System;
using DefaultNamespace;
using SalinSDK;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayersState : SalinCallbacks
{
    public Text txtInterviewer;
    public Text txtInterviewee;
    
    public bool isInterviewerWait = false;
    public bool isIntervieweeWait = false;
    public bool isInterviewerReady = false;

    void Start()
    {
        txtInterviewee.color = Color.gray;
        txtInterviewer.color = Color.gray;
    }

    public override void OnReceiveMessage<T>(T data)
    {
        switch (data)
        {
            case RoomUserStateMessage userConnectedMessageData:
                UpdateUserState(userConnectedMessageData);
                break;
        }
    }

    private void UpdateUserState(RoomUserStateMessage data)
    {
        switch (data.userMode)
        {
            case UserMode.Interviewee:
                isIntervieweeWait = data.isConnected;
                isInterviewerReady = data.isReady;
                txtInterviewee.color = data.isConnected switch
                {
                    true => data.isReady ? Color.blue : Color.black,
                    false => txtInterviewee.color = Color.gray
                };
                break;
            
            case UserMode.Interviewer:
                isInterviewerWait = data.isConnected;
                txtInterviewer.color = data.isConnected switch
                {
                    true => data.isReady ? Color.blue : Color.black,
                    false => txtInterviewer.color = Color.gray
                };          
                break;
        }
    }
}
