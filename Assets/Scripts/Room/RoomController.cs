using System;
using DefaultNamespace;
using SalinSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class RoomController : SalinCallbacks
{
    private RoomInfo demoRoom;

    public RoomChat roomChat;
    public RoomPlayersState roomPlayersState;
    
    private UserMode _userMode = UserMode.Interviewer;

    public Button btnReady;
    public Button btnStart;

    // Start is called before the first frame update
    void Start()
    {
        _userMode = XRSocialSDK.myPlayer.userNickname == Constants.InterviewerId ? UserMode.Interviewer : UserMode.Interviewee;
        InitRoom();
    }

    private void InitRoom()
    {
        var userStateMessage = new RoomUserStateMessage {isConnected = true};

        // Send room enter message
        if (_userMode == UserMode.Interviewee)
        {
            userStateMessage.isReady = false;
            userStateMessage.userMode = UserMode.Interviewee;
        }
        else
        {
            userStateMessage.isReady = true;
            userStateMessage.userMode = UserMode.Interviewer;
        }
        XRSocialSDK.SendBroadcastMessage(userStateMessage);

        // set button
        btnStart.gameObject.SetActive(_userMode == UserMode.Interviewer);
        btnReady.gameObject.SetActive(_userMode == UserMode.Interviewee);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleIntervieweeReady()
    {
        var readyMessage = new RoomUserStateMessage()
        {
            isConnected = true,
            isReady = !roomPlayersState.isInterviewerReady,
            userMode = UserMode.Interviewee
        };
        XRSocialSDK.SendBroadcastMessage(readyMessage);
    }

    public void StartInterview()
    {
        if (roomPlayersState.isIntervieweeWait && roomPlayersState.isInterviewerReady && roomPlayersState.isInterviewerWait)
        {
            XRSocialSDK.SendBroadcastMessage(new RoomStartInterviewMessage());
        }
        else
        {
            Debug.Log("Cannot start the interview: Interviewee is not ready or one of both is not connected in the room.");
        }
    }

    public void LeaveRoom()
    {
        var readyMessage = new RoomUserStateMessage()
        {
            isConnected = false,
            isReady = false,
            userMode = _userMode
        };
        XRSocialSDK.SendBroadcastMessage(readyMessage);
        XRSocialSDK.LeaveRoom();
    }

    public override void OnLeaveRoom()
    {
        SceneManager.LoadScene("DemoLoginScene");
    }
    
    public override void OnReceiveMessage<T>(T data)
    {
        switch (data)
        {
            case RoomUserStateMessage userConnectedMessageData:
                if (userConnectedMessageData.userMode == UserMode.Interviewee)
                {
                    btnReady.GetComponentInChildren<Text>().text = userConnectedMessageData.isReady switch
                    {
                        true => "Unready",
                        false => "Ready"
                    };
                }
                break;
            
            case RoomStartInterviewMessage startInterviewMessage:
                SceneManager.LoadScene("InterviewScene");
                break;
        }
    }
}
