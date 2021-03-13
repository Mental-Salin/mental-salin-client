using SalinSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DemoConstants;

public class DemoSocialManager : SalinCallbacks
{
    public DemoRoomUI RoomUIControl;
    public GameObject MenuPrefab;

    #region ManageRoom Callbacks

    public override void OnLeaveRoom()
    {
        Debug.Log("OnLeaveRoom");

        if (ChatManager.IsConnect())
            ChatManager.Disconnect();

        if (VoiceManager.IsConnect())
            VoiceManager.Disconnect();

        if (DemoControlManager.Instance.currentState != DemoState.room)
            return;


        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Instantiate(MenuPrefab);
        DemoControlManager.Instance.currentState = DemoState.menu;
        GameObject thisPrefab = this.transform.parent.gameObject;
        if (thisPrefab != null)
            Destroy(thisPrefab);


       // SceneManager.LoadScene("DemoMenuScene");


    }

    public override void OnPlayerEnteredRoom(Player enterPlayer)
    {
        if (enterPlayer == null)
        {
            Debug.LogError("OnPlayerEnteredRoom null");
            return;
        }

        Debug.Log("OnPlayerEnteredRoom: " + enterPlayer.userId);

        RoomUIControl.AddRoomPlayerList();
    }
    public override void OnPlayerLeftRoom(Player leftPlayer)
    {
        if (leftPlayer == null)
            Debug.LogError("null");

        Debug.Log("OnPlayerLeftRoom : " + leftPlayer.userNickname);

        RoomUIControl.AddRoomPlayerList();
    }

    public override void OnUpdateRoomProperties(Dictionary<object, object> changeProp)
    {
        Debug.Log("OnUpdateRoomProperties");

        RoomUIControl.AddRoomProperty();
    }


    #endregion

    #region Manage Player Callbacks

    public override void OnUserBlock()
    {
        Debug.Log("Block Done");

        //RoomUIControl.AddRoomBlockList();

    }
    public override void OnUserBlockFail(ErrorCode errorCode)
    {
        Debug.LogError("OnUserBlockFail" + errorCode.ToString());
    }
    public override void OnUserKick(Player kickedPlayer)
    {
        Debug.Log("Kick Done");

        RoomUIControl.AddRoomPlayerList();
        //RoomUIControl.AddRoomBlockList();

    }
    public override void OnUserKickFail(ErrorCode errorCode)
    {
        Debug.LogError("OnUserKickFail" + errorCode.ToString());
    }
    #endregion

    #region Message Callbacks

    public override void OnReceiveInvitePlayerToRoom(string senderId, string roomName, string hostName)
    {
        Debug.Log("OnReceiveInvitePlayerToRoom: " + senderId + "/" + roomName + "/" + hostName);

        if(DemoControlManager.Instance.currentState == DemoState.menu)
        {
            GameObject LobbyCon = GameObject.Find("MenuUIController");
            DemoMenuUIControl FriendUISource = LobbyCon.GetComponent<DemoMenuUIControl>();

            if (FriendUISource != null)
                FriendUISource.ReceiveInviteMessage(senderId, roomName, hostName);
        }
        else if(DemoControlManager.Instance.currentState == DemoState.room)
        {
            GameObject RoomUICon = GameObject.Find("RoomUIController");
            DemoRoomUI RoomUISource = RoomUICon.GetComponent<DemoRoomUI>();

            if (RoomUISource != null)
                RoomUISource.ReceiveInviteMessage(senderId, roomName, hostName);
        }
    }

    public override void OnReceiveRespondInviteRoom(string senderId, string roomName, bool acceptInvite)
    {
        Debug.Log("OnReceiveRespondInviteRoom: " + senderId + "/" + roomName + "/" + acceptInvite.ToString());
    }

    public override void OnUserNotFound(string action, string error)
    {
        Debug.Log("OnUserNotFound: " + error.ToString());
    }

    #endregion

    #region voice Callbacks
    public override void OnVoiceConnect()
    {
        Debug.Log("VoiceConnect");

        RoomUIControl.ConnectedVoice();
    }
    public override void OnVoiceConnectFail(ErrorCode errorCode)
    {
        RoomUIControl.SetVoiceReconnectButton();
    }
    public override void OnVoiceDisconnect()
    {
        Debug.Log("Voice Disconnect");

        RoomUIControl.SetVoiceReconnectButton();
    }

    #endregion
}
