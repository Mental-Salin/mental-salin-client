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

    public void BeginDemo(string id)
    {
        try
        {
            XRSocialSDK.DisconnectSocialServer();
            AccountManager.LogOut();
        }
        catch
        {
            // ignored
        }

        currentId = id;
        SignUpDemoAccount();
    }

    private void SignUpDemoAccount()
    {
        AccountManager.SignUp(currentId, Constants.DemoAccountPw, currentId);
    }

    public override void OnSignUp()
    {
        Debug.Log($"Sign up Success! Id:{currentId}");
        LogIn(currentId);
    }

    private void LogIn(string id)
    {
        AccountManager.Login(id, Constants.DemoAccountPw);
    }
    
    public override void OnLogIn(UserInfo info)
    {
        Debug.Log($"Log in Success! Id:{info.userAccount}, name:{info.userNickname}");
        XRSocialSDK.ConnectToSocialServer(true);
    }

    public override void OnAccountError(ErrorCode errorCode)
    {
        if (errorCode == ErrorCode.ExistAccount || ((int) errorCode) == 6003)
        {
            Debug.Log($"Already signed up, log in...");
            LogIn(currentId);
        }
        else
        {
            Debug.Log($"Fail to sign up or log in... code : {errorCode}");
        }
    }
    
    public override void OnConnectedSocialServer()
    {
        Debug.Log("Success to connect to the social server.");
        UpdateDemoRoom();
        EnterRoom();
    }
    
    public override void OnConnectedSocialServerFail(DisconnectCause disconnectCause)
    {
        Debug.Log($"Fail to connect to the social server... code : {disconnectCause}");
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
                Debug.Log($"Fail to enter the room.. demoRoom IsOpen: {demoRoom.IsOpen}, playerCount: {demoRoom.PlayerCount}");
            }
        }
        else
        {
            XRSocialSDK.CreateRoom(Constants.DemoRoomName, new RoomOption() {MaxPlayerCount = 2});
            SceneManager.LoadScene("RoomScene");
        }
    }
}
