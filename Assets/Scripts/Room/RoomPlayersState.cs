using System;
using System.Linq;
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
        
        XRSocialSDK.currentRoom.PlayerList
            .Values
            .Where(player => !player.isMyPlayer)
            .ToList()
            .ForEach(player => {
                switch (player.userNickname)
                {
                    case Constants.InterviewerId:
                        isInterviewerWait = true;
                        txtInterviewer.color = Color.black;
                        break;
                    case Constants.IntervieweeId:
                        isIntervieweeWait = true;
                        txtInterviewee.color = Color.blue;
                        break;
                }
            });
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

    public override void OnPlayerEnteredRoom(Player enterPlayer)
    {
        var userMode = enterPlayer.userNickname == Constants.InterviewerId
            ? UserMode.Interviewer
            : UserMode.Interviewee;
        
        var readyMessage = new RoomUserStateMessage()
        {
            isConnected = true,
            isReady = userMode == UserMode.Interviewer,
            userMode = userMode
        };
        XRSocialSDK.SendBroadcastMessage(readyMessage);
    }

    public override void OnPlayerLeftRoom(Player leftPlayer)
    {
        var userMode = leftPlayer.userNickname == Constants.InterviewerId
            ? UserMode.Interviewer
            : UserMode.Interviewee;
        
        var readyMessage = new RoomUserStateMessage()
        {
            isConnected = false,
            isReady = false,
            userMode = userMode
        };
        XRSocialSDK.SendBroadcastMessage(readyMessage);
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
