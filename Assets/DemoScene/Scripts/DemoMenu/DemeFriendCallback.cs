using SalinSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemeFriendCallback : SalinCallbacks
{
    public DemoFriendPopUI FrUICon;

    #region UpdateCallbacks

    public override void OnUpdateState(List<Friend> friendList)
    {
        if (friendList == null)
            return;
        UserManager.Instance.SetFriendList(friendList);
        DemoControlManager.Instance.SetMyFriend(friendList);

        FrUICon.AddFriendList();

    }
    public override void OnUpdateStateFail(ErrorCode errorCode)
    {
        //FrUICon.SettingLoadingText(true, errorCode.ToString());
        Debug.LogError("OnUpdateStateFail: " + errorCode);
    }
    #endregion

    #region SearchCallbacks
    public override void OnSearchFriendList(List<Friend> friendList)
    {
        UserManager.Instance.MakeSearchList(friendList);
        DemoControlManager.Instance.SetSearchList(friendList);
        FrUICon.AddFriendSearchList(friendList);

    }

    public override void OnSearchFriendListFail(ErrorCode errorCode)
    {
        Debug.LogError("OnUpdateStateFail: " + errorCode);
    }

    #endregion

    #region RequestCallbacks

    public override void OnRequestFriend(UserInfo info)
    {
        Debug.Log("성공");
        FriendManager.UpdateState();
    }

    public override void OnRequestFriendFail(ErrorCode errorCode)
    {
        Debug.LogError(errorCode.ToString());
    }

    #endregion

    #region ResponseCallbacks
    //친구 요청 수락 
    public override void OnResponseFriend(UserInfo info)
    {
        Debug.Log("성공");
        FriendManager.UpdateState();
    }

    public override void OnResponseFriendFail(ErrorCode errorCode)
    {
        Debug.LogError(errorCode.ToString());
    }

    #endregion

    #region DeleteCallbacks
    public override void OnRemoveFriend(UserInfo info)
    {
        Debug.Log("성공");
        FriendManager.UpdateState();
    }
    public override void OnRemoveFriendFail(ErrorCode errorCode)
    {
        Debug.LogError(errorCode.ToString());
    }
    #endregion
}
