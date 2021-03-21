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

    private void OnDestroy()
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
        UpdateDemoRoom();
        EnterRoom();
    }
    
    public override void OnConnectedSocialServerFail(DisconnectCause disconnectCause)
    {
        MalinLog.Get().ShowLog($"Fail to connect to the social server... code : {disconnectCause}");
    }

    private void UpdateDemoRoom()
    {
        demoRoom = XRSocialSDK.GetRoomInfoFromLobby(Constants.DemoRoomName);
    }

    private void EnterRoom()
    {
        if (demoRoom is { })
        {
            if (demoRoom.IsOpen && demoRoom.PlayerCount < 2)
            {
                XRSocialSDK.JoinRoom(Constants.DemoRoomName);
                SceneManager.LoadScene("RoomScene");
            }
            else
            {
                MalinLog.Get().ShowLog($"Fail to enter the room.. demoRoom IsOpen: {demoRoom.IsOpen}, playerCount: {demoRoom.PlayerCount}");
            }
        }
        else
        {
            XRSocialSDK.CreateRoom(Constants.DemoRoomName, new RoomOption() {MaxPlayerCount = 2});
            SceneManager.LoadScene("RoomScene");
        }
    }
}
