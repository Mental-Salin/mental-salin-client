using SalinSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoConstants;

public class DemoFriendInfoUI : MonoBehaviour
{
    public Button LeftButton;
    public Button RightButton;
    public Text LeftButtonText;
    public Text RightButtonText;

    public Text FirstInText;
    public Text SecondInText;
    public Text ThirdInText;

    Friend friendInfo;


    public void SetLeftButtonListener(Action action)
    {
        LeftButton.onClick.RemoveAllListeners();
        LeftButton.onClick.AddListener(() => { action(); });
    }
    public void SetRightButtonListener(Action action)
    {
        RightButton.onClick.RemoveAllListeners();
        RightButton.onClick.AddListener(() => { action(); });
    }

    void SetPopupContent(string state)
    {
        switch (state)
        {
            case "FriendCompleteMenu":
                SetTextContent(false, true, "초대불가", "삭제");
                RightButton.onClick.AddListener(() => { DeleteFriend(); });
                break;
            case "FriendRequestMenu":
                SetTextContent(false, false, "초대불가", "요청중");
                break;
            case "FriendPendingMenu":
                SetTextContent(true, true, "거절", "수락");
                LeftButton.onClick.AddListener(() => { DeclineFriend(); });
                RightButton.onClick.AddListener(() => { AcceptFriend(); });
                break;
            case "NotFriend":
                SetTextContent(false, true, "초대불가", "요청");
                RightButton.onClick.AddListener(() => { RequestFriend(); });
                break;
            case "FriendRoom":
                SetTextContent(false, true, "", "초대");
                //LeftButton.onClick.AddListener(() => { BlockFriend(); });
                RightButton.onClick.AddListener(() => { InviteFriend(); });
                break;
            case "NotFriendRoom":
                SetTextContent(false, true, "", "초대");
                //LeftButton.onClick.AddListener(() => { BlockFriend(); });
                RightButton.onClick.AddListener(() => { InviteFriend(); });
                break;
        }
    }

    private void SetTextContent(bool Lin, bool Rin, string lefttext, string righttext)
    {
        LeftButton.interactable = Lin;
        RightButton.interactable = Rin;
        LeftButtonText.text = lefttext;
        RightButtonText.text = righttext;
    }

    public void SetContentText(Friend myfriendinfo)
    {
        friendInfo = myfriendinfo;

        FirstInText.text = myfriendinfo.userAccount;
        SecondInText.text = myfriendinfo.userNickName;
        ThirdInText.text = myfriendinfo.status.ToString();
        FriendStatus friendstatus = myfriendinfo.status;

        if (DemoControlManager.Instance.currentState == DemoState.menu)
        {
            switch (friendstatus)
            {
                case FriendStatus.Completed:
                    SetPopupContent("FriendCompleteMenu");
                    break;
                case FriendStatus.Requested:
                    SetPopupContent("FriendRequestMenu");
                    break;
                case FriendStatus.Pending:
                    SetPopupContent("FriendPendingMenu");
                    break;
                case FriendStatus.None:
                    SetPopupContent("NotFriend");
                    break;
            }
        }
        else
        {
            switch (friendstatus)
            {
                case FriendStatus.None:
                    SetPopupContent("NotFriendRoom");
                    break;
                default:
                    SetPopupContent("FriendRoom");
                    break;
            }
        }
    }

    #region Common ButtonListner

    public void DeleteFriend()
    {
        RightButton.interactable = false;
        FriendManager.RemoveFriend(friendInfo.userID);
    }

    public void InviteFriend()
    {
        XRSocialSDK.InviteFriend(friendInfo.userID);
        Debug.Log("InviteFriend: " + friendInfo.userAccount + " / " + friendInfo.userID);
    }

    public void BlockFriend()
    {
        XRSocialSDK.UserBlocking(friendInfo.userID);
        Debug.Log("PlayerBlock: " + friendInfo.userID);
    }

    public void AcceptFriend()
    {
        LeftButton.interactable = false;
        RightButton.interactable = false;
        FriendManager.ResponeFriend(friendInfo.processID, true);
    }

    public void DeclineFriend()
    {
        LeftButton.interactable = false;
        RightButton.interactable = false;

        FriendManager.ResponeFriend(friendInfo.processID, false);

    }

    public void RequestFriend()
    {
        RightButton.interactable = false;
        FriendManager.RequestFriend(friendInfo.userID);
    }

    #endregion
}
