using SalinSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoConstants;

public class DemoConnectionManager : SalinCallbacks
{
    private void Start()
    {
        XRSocialSDK.ConnectToMainServer();
    }

    #region Connection Callbacks

    public override void OnConnectedMainServer(string appToken)
    {
        SalinTokens.AppToken = appToken;
        DemoControlManager.Instance.currentState = DemoState.login;
    }

    public override void OnConnectedSocialServer()
    {
        Debug.Log("Photon Connect");
    }

    public override void OnConnectedSocialServerFail(DisconnectCause disconnectCause)
    {
        Debug.Log("OnConnectedSocialServerFail: " + disconnectCause.ToString());
    }

    public override void OnDisconnectedSocialServer(DisconnectCause disconnectCause)
    {
        Debug.Log("OnDisconnectedSocialServer: " + disconnectCause.ToString());
    }

    public override void OnConnectedMessageServer()
    {
        Debug.Log("RTC Connect");
    }

    public override void OnConnectedMessageServerFail(ErrorCode errorCode)
    {
        Debug.LogError("OnConnectedMessageServerFail: + " + errorCode.ToString());
    }

    public override void OnDisconnectedMessageServer()
    {
        Debug.LogError("OnDisconnectedMessageServer");
    }

    #endregion
}
