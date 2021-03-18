using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SalinSDK;
using UnityEngine.UI;

public class DemoEnterPwPopup : SalinCallbacks
{
    [Header("RoomPwPopup")]
    public InputField PwInputfield;
    public Text PwErrorMsg;
    public Button PwJoinRoomButton;

    private RoomInfo SelectedRoomInfo;

    public void EnterPwJoinRoom()
    {

        if (string.IsNullOrWhiteSpace(PwInputfield.text))
        {
            PwErrorMsg.text = "비밀번호를 입력하세요";
            return;

        }

        PwJoinRoomButton.interactable = false;

        XRSocialSDK.JoinRoomWithPassword(SelectedRoomInfo.RoomName, PwInputfield.text);

        PwInputfield.text = null;
    }

    public void ClosePopup()
    {
        PwInputfield.text = null;
        PwJoinRoomButton.interactable = true;
        SelectedRoomInfo = null;
        this.gameObject.SetActive(false);
    }

    public override void OnJoinRoomFail(ErrorCode errorCode)
    {
        PwErrorMsg.text = errorCode.ToString();
        PwJoinRoomButton.interactable = true;
    }

    public void SetRoomInfo(RoomInfo selectroominfo)
    {
        SelectedRoomInfo = selectroominfo;
    }    
}
