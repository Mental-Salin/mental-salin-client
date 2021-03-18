using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DemoConstants;
using SalinSDK;

public class DemoMatchMakingManager : SalinCallbacks
{
    public GameObject RoomPrefab;

    public override void OnCreateRoom()
    {
        Debug.Log("OnCreateRoom");


    }
    public override void OnCreateRoomFail(ErrorCode errorCode)
    {
        Debug.LogError("OnCreateRoomFail");
    }
    public override void OnJoinRoom()
    {
        Debug.Log("OnJoinRoom");


        if (DemoControlManager.Instance.currentState != DemoState.menu)
            return;

        Camera.main.clearFlags = CameraClearFlags.Skybox;
        GameObject thisPrefab = this.transform.parent.gameObject;
        Instantiate(RoomPrefab);
        DemoControlManager.Instance.currentState = DemoState.room;
        if (thisPrefab != null)
            Destroy(thisPrefab);

        //DemoControlManager.Instance.currentState = DemoState.room;
        //SceneManager.LoadScene("DemoRoom");
    }
    public override void OnJoinRoomFail(ErrorCode errorCode)
    {
        Debug.LogError("OnJoinRoomFail: " + errorCode.ToString());
    }

    #region Message Callbacks

    public override void OnReceiveInvitePlayerToRoom(string senderId, string roomName, string hostName)
    {
        Debug.Log("OnReceiveInvitePlayerToRoom: " + senderId + "/" + roomName + "/" + hostName);

        if (DemoControlManager.Instance.currentState == DemoState.menu)
        {
            GameObject LobbyCon = GameObject.Find("MenuUIController");
            DemoMenuUIControl FriendUISource = LobbyCon.GetComponent<DemoMenuUIControl>();

            if (FriendUISource != null)
                FriendUISource.ReceiveInviteMessage(senderId, roomName, hostName);
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
}
