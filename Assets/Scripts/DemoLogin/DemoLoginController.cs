using System;
using System.Collections;
using System.Collections.Generic;
using SalinSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoLoginController : SalinCallbacks
{
    public Canvas canvas;
    public string currentId;

    private RoomInfo demoRoom;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Disconnect()
    {
        try
        {
            if (XRSocialSDK.IsConnected_SocialServer)
                XRSocialSDK.DisconnectSocialServer();
        }
        catch
        {
            // ignored
        }

        try
        {
            if (XRSocialSDK.myPlayer != null)
                AccountManager.LogOut();
        }
        catch
        {
            // ignored
        }
    }

    public void BeginDemo(string id)
    {
        Disconnect();
        currentId = id;
        SignUpDemoAccount();
    }

    private void SignUpDemoAccount()
    {
        AccountManager.SignUp(currentId, Constants.DemoAccountPw, currentId);
    }

    public override void OnSignUp()
    {
        MalinLog.Get().ShowLog($"Sign up Success! Id:{currentId}");
        LogIn(currentId);
    }

    private void LogIn(string id)
    {
        AccountManager.Login(id, Constants.DemoAccountPw);
    }
    
    public override void OnLogIn(UserInfo info)
    {
        MalinLog.Get().ShowLog($"Log in Success! Id:{info.userAccount}, name:{info.userNickname}");
        XRSocialSDK.ConnectToSocialServer(true);
    }

    public override void OnAccountError(ErrorCode errorCode)
    {
        if (errorCode == ErrorCode.ExistAccount || ((int) errorCode) == 6003)
        {
            MalinLog.Get().ShowLog($"Already signed up, log in...");
            LogIn(currentId);
        }
        else
        {
            MalinLog.Get().ShowLog($"Fail to sign up or log in... code : {errorCode}");
        }
    }
    
    public override void OnConnectedSocialServer()
    {
        MalinLog.Get().ShowLog("Success to connect to the social server.");
        XRSocialSDK.JoinLobby();
        demoRoom = XRSocialSDK.GetRoomInfoFromLobby(Constants.DemoRoomName);
        EnterRoom();
    }

    public override void OnConnectedSocialServerFail(DisconnectCause disconnectCause)
    {
        MalinLog.Get().ShowLog($"Fail to connect to the social server... code : {disconnectCause}");
    }
    
    public override void OnJoinedLobby()
    {
        MalinLog.Get().ShowLog("Success to Join lobby.");
   
    }

    public override void OnLeftLobby()
    {
        MalinLog.Get().ShowLog("Left lobby.");
    }

    private void EnterRoom()
    {
        if (demoRoom is { })
        {
            if (demoRoom.IsOpen && demoRoom.PlayerCount < 2)
            {
                MalinLog.Get().ShowLog("Able to join the room. Try to join the room.");
                XRSocialSDK.JoinRoom(Constants.DemoRoomName);
            }
            else
            {
                MalinLog.Get().ShowLog($"Fail to join the room.. demoRoom IsOpen: {demoRoom.IsOpen}, playerCount: {demoRoom.PlayerCount}");
            }
        }
        else
        {
            MalinLog.Get().ShowLog("No room existed. Try to create the room.");
            XRSocialSDK.CreateRoom(Constants.DemoRoomName, new RoomOption {MaxPlayerCount = 2});
        }
    }

    public override void OnJoinRoom()
    {
        MalinLog.Get().ShowLog("Success to Join the room. Load room scene..");
        SceneManager.LoadScene("RoomScene");
    }

    public override void OnJoinRoomFail(ErrorCode errorCode)
    {
        MalinLog.Get().ShowLog("Fail to Join the room from SDK");
    }
}
