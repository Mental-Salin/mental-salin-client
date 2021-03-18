using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;
using System.Text;

public class DemoRoomListUI : SalinCallbacks
{
    RoomInfo SelectedRoomInfo;

    [Header("RoomList")]
    public GameObject RoomListPrefab;
    public GameObject RoomListContentRoot;

    [Header("RoomPwPopup")]
    public GameObject PwPopup;
    public DemoEnterPwPopup PwPopupScript;

    public void AddRoomList()
    {
        DemoRoomListInfo[] Rootchildren = RoomListContentRoot.GetComponentsInChildren<DemoRoomListInfo>();

        if (Rootchildren != null)
        {
            foreach (DemoRoomListInfo child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }
        Debug.Log("AddRoomList");

        Dictionary<string, RoomInfo> RoomListDictionary = XRSocialSDK.GetRoomList();

        if (RoomListDictionary == null || RoomListDictionary.Count < 1)
        {
            Debug.Log("RoomListDictionary null");
            return;
        }

        foreach (var items in RoomListDictionary)
        {
            RoomInfo roominfo = items.Value;

            if (!roominfo.RemoveFromList)
            {
                Debug.Log("roominfo: " + roominfo.RoomName);

                GameObject RoomListContent = GameObject.Instantiate(RoomListPrefab, RoomListContentRoot.transform);

                if (RoomListContent == null)
                    return;

                DemoRoomListInfo contentui = RoomListContent.GetComponent<DemoRoomListInfo>();

                if (contentui == null)
                    return;

                contentui.SetRoomInfo(roominfo.RoomName, roominfo.IsOpen, roominfo.PlayerCount, roominfo.MaxPlayerCount);

                if (roominfo.IsOpen)
                {
                    Button entrybutton = RoomListContent.GetComponentInChildren<Button>();
                    entrybutton.onClick.AddListener(() => { CallJoinRoom(roominfo.RoomName); });
                }
            }
        }

    }
    public void CallJoinRoom(string roomname)
    {
        Debug.Log("selected room name: " + roomname);
        SelectedRoomInfo = XRSocialSDK.GetRoomInfoFromLobby(roomname);

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
    public override void OnRoomListUpdate()
    {
        Debug.Log("OnRoomListUpdate");
        AddRoomList();
    }

}
