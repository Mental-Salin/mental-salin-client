using System;
using System.Collections;
using System.Collections.Generic;
using SalinSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoLoginController : SalinCallbacks
{
    private const string DemoPasswd = "demotest";
    
    public Canvas canvas;
    public string currentId;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void BeginDemo(string id)
    {
        currentId = id;
        SignUpDemoAccount();
    }

    private void SignUpDemoAccount()
    {
        AccountManager.SignUp(currentId, DemoPasswd, currentId);
    }

    public override void OnSignUp()
    {
        Debug.Log($"Sign up Success! Id:{currentId}");
        LogIn(currentId);
    }

    private void LogIn(string id)
    {
        AccountManager.Login(id, DemoPasswd);
    }
    
    public override void OnLogIn(UserInfo info)
    {
        Debug.Log($"Log in Success! Id:{info.userID}");
        XRSocialSDK.ConnectToSocialServer();
    }

    public override void OnAccountError(ErrorCode errorCode)
    {
        Debug.Log($"Fail to sign up or log in... code : {errorCode}");
    }
    
    public override void OnConnectedSocialServer()
    {
        Debug.Log("Success to connect to the social server.");
        EnterRoom(currentId);
    }
    
    public override void OnConnectedSocialServerFail(DisconnectCause disconnectCause)
    {
        Debug.Log($"Fail to connect to the social server... code : {disconnectCause}");
    }

    private void EnterRoom(string id)
    {
        switch (id)
        {
            case "interviewee" :
                XRSocialSDK.CreateRoom("Demo");
                SceneManager.LoadScene("RoomScene");
                break;
            case "interviewer" :
                XRSocialSDK.JoinRoom("Demo");
                SceneManager.LoadScene("RoomScene");
                break;
        }
    }
}
