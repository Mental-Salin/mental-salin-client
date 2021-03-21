using System.Collections;
using System.Collections.Generic;
using SalinSDK;
using UnityEngine;

public class InterviewController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogoutAndExit()
    {
        XRSocialSDK.LeaveRoom();
        XRSocialSDK.DisconnectMessageServer();
        XRSocialSDK.DisconnectSocialServer();;
        AccountManager.LogOut();
        Application.Quit();
    }
}
