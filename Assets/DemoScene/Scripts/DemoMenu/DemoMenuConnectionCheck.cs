using SalinSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoMenuConnectionCheck : SalinCallbacks
{
    public Toggle PhotonToggle;
    public Toggle RtcToggle;

    public DemoRoomListUI roomlistui;

    private void Start()
    {
        MenuInit();
        RtcToggle.isOn = XRSocialSDK.IsConnected_MessageServer;
    }

    public void MenuInit()
    {

        FriendManager.UpdateState();

        if (!XRSocialSDK.IsConnected_SocialServer)
            XRSocialSDK.ConnectToSocialServer(true);

        if (!XRSocialSDK.IsConnected_MessageServer)
            XRSocialSDK.ConnectToMessageServer();

    }

    public override void OnConnectedSocialServer()
    {
        PhotonToggle.isOn = true;

        //roomlistui.AddRoomList();
    }

    public override void OnConnectedSocialServerFail(DisconnectCause disconnectCause)
    {
        PhotonToggle.isOn = false;
    }

    public override void OnDisconnectedSocialServer(DisconnectCause disconnectCause)
    {
        PhotonToggle.isOn = false;
    }

    public override void OnConnectedMessageServer()
    {
        RtcToggle.isOn = true;
    }

    public override void OnConnectedMessageServerFail(ErrorCode errorCode)
    {
        RtcToggle.isOn = false;
    }

    public override void OnDisconnectedMessageServer()
    {
        RtcToggle.isOn = false;
    }
}
