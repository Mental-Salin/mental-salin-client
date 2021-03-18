using SalinSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoCreateRoomOptionPopup : SalinCallbacks
{
    public InputField RoomNameInputField;
    public Toggle RoomOpenToggle;
    public Toggle RoomVisibleToggle;
    public Dropdown MaxplayerDropdown;
    public Toggle PasswordToggle;
    public InputField PwInputfield;

    public Text errorcode;

    public void ClickCreateButton()
    {
        if (string.IsNullOrWhiteSpace(RoomNameInputField.text))
        {
            errorcode.text = "Room Name 을 적어주세요";
            return;
        }

        RoomOption myroomoption = new RoomOption();
        myroomoption.IsOpen = RoomOpenToggle.isOn;
        myroomoption.IsVisible = RoomVisibleToggle.isOn;
        myroomoption.MaxPlayerCount = MaxplayerDropdown.value + 1;
        if(PasswordToggle.isOn)
        {
            if (string.IsNullOrWhiteSpace(PwInputfield.text))
            {
                errorcode.text = "비밀번호 입력";
                return;
            }
            Debug.Log("password: " + PwInputfield.text);
            myroomoption.Password = PwInputfield.text;
        }

        XRSocialSDK.CreateRoom(RoomNameInputField.text, myroomoption);
    }

    public void InitSetting()
    {
        RoomOpenToggle.isOn = true;
        RoomVisibleToggle.isOn = true;
        MaxplayerDropdown.value = 4;
        RoomNameInputField.text = UserManager.Instance.userInfo.userNickname + "Room";
        PasswordToggle.isOn = false;
        PwInputfield.text = null;
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnCreateRoomFail(ErrorCode errorCode)
    {
        Debug.Log(errorCode.ToString());
        errorcode.text = errorCode.ToString();
    }

}
