using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;
using System;

public class DemoMenuUIControl : MonoBehaviour
{
    public Toggle FriendToggle;
    public Toggle MyInfoToggle;

    public GameObject FriendPopup;
    public DemoFriendPopUI FriendPopScript;
  
    public GameObject MyInfoPopup;
    public DemoMyInfoPopUI MyInfoPopScript;

    public GameObject InvitePopupRoot;

    string InviteHostID = null;
    string InviteRoomName = null;

    public GameObject RoomSettingPopup;
    public DemoCreateRoomOptionPopup RoomSetScript;

    public GameObject PwPopup;
    public DemoEnterPwPopup PwPopupScript;

    // Start is called before the first frame update
    void Start()
    {
        FriendToggle.isOn = true;
        MyInfoToggle.isOn = true;

        GameObject[] disableobj = GameObject.FindGameObjectsWithTag("PopupRoot");
        if (disableobj == null || disableobj.Length < 1)
            return;

        foreach (GameObject items in disableobj)
        {
            items.SetActive(false);
        }
    }

    #region Toggle Listener
    public void IsOpenFriendPopup(bool IsOn)
    {
        if (!IsOn)
        {
            FriendPopup.SetActive(true);
            MyInfoToggle.interactable = false;
            FriendPopScript.FriendInit(true);
        }
        else
        {
            FriendPopup.SetActive(false);
            FriendToggle.isOn = true;
            MyInfoToggle.interactable = true;
            FriendPopScript.ClosePopUI();
        }
    }

    public void IsOpenMyInfoPopup(bool IsOn)
    {
        if (!IsOn)
        {
            MyInfoPopup.SetActive(true);
            FriendToggle.interactable = false;
            MyInfoPopScript.SetInfoText(); 

        }
        else
        {
            MyInfoPopup.SetActive(false);
            MyInfoToggle.isOn = true;
            FriendToggle.interactable = true;
        }
    }
    #endregion

    #region CreateRoom
    public void CreateRoom()
    {
        string roomname = UserManager.Instance.userInfo.userAccount + "Room";
        XRSocialSDK.CreateRoom(roomname);
        //ChatManager.Connect(roomname, UserManager.Instance.userInfo.userNickname);
    }

    public void OpenRoomSetPopup()
    {
        RoomSettingPopup.SetActive(true);
        RoomSetScript.InitSetting();

    }
    #endregion

    #region Receive Invite Meesage 
    public void ReceiveInviteMessage(string senderId, string roomName, string hostName)
    {
        InviteHostID = senderId;
        InviteRoomName = roomName;

        InvitePopupRoot.SetActive(true);
    }

    public void AcceptInvite()
    {
        RoomInfo inviteroominfo = XRSocialSDK.GetCachedRoomInfo(InviteRoomName);

        if(inviteroominfo.HasPassword)
        {
            XRSocialSDK.JoinRoom(InviteRoomName);
            XRSocialSDK.RespondInviteRoom(InviteHostID, InviteRoomName, true);
            InvitePopupRoot.SetActive(false);
            return;
        }

        XRSocialSDK.RespondInviteRoom(InviteHostID, InviteRoomName, true);
        SetJoinRoom(InviteRoomName);
    }
    public void SetJoinRoom(string roomname)
    {
        Debug.Log("SetJoinRoom: " + roomname);
        RoomInfo SelectedRoomInfo = XRSocialSDK.GetCachedRoomInfo(roomname);

        if (SelectedRoomInfo.HasPassword)
        {
            XRSocialSDK.JoinRoom(roomname);
        }
        else
        {
            PwPopup.SetActive(true);
            PwPopupScript.SetRoomInfo(SelectedRoomInfo);
        }
    }

    public void DeclineInvite()
    {
        XRSocialSDK.RespondInviteRoom(InviteHostID, InviteRoomName, false);
        InvitePopupRoot.SetActive(false);
    }
    #endregion
}
