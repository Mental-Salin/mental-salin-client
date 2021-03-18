using SalinSDK.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SalinSDK;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using DemoConstants;

public class DemoControlManager : Singleton<DemoControlManager>
{
    //[NonSerialized]
    public DemoState currentState = DemoState.none;

    private Dictionary<string, Friend> MyFriend = new Dictionary<string, Friend>();
    private Dictionary<string, Friend> SearchFriend = new Dictionary<string, Friend>();

    public override void Awake()
    {
        base.Awake();
    }

    #region Friend List

    public void SetSearchList(List<Friend> friendList)
    {
        Dictionary<string, Friend> templist = new Dictionary<string, Friend>();

        if (friendList != null)
        {
            foreach (Friend items in friendList)
            {
                templist.Add(items.userAccount, items);
            }
        }

        DemoControlManager.Instance.SearchFriend = templist;
    }

    public void SetMyFriend(List<Friend> friendList)
    {
        Dictionary<string, Friend> tempfriendlist = new Dictionary<string, Friend>();

        foreach (Friend items in friendList)
        {
            tempfriendlist.Add(items.userAccount, items);
        }

        MyFriend = tempfriendlist;

    }

    public Friend SelectedFriend(string useracc)
    {
        Friend FriendValue = null;

        if (MyFriend.ContainsKey(useracc))
        {
            MyFriend.TryGetValue(useracc, out FriendValue);
        }

        return FriendValue;
    }

    public Friend FindSearchFriend(string useracc)
    {
        Friend FriendValue = null;

        if (SearchFriend.ContainsKey(useracc))
        {
            SearchFriend.TryGetValue(useracc, out FriendValue);
        }

        return FriendValue;
    }

    public string GetFriendId(string useracc)
    {
        List<Friend> friendlist = UserManager.Instance.GetFriendList();

        foreach (Friend findfriend in friendlist)
        {
            if (findfriend.userAccount == useracc)
            {
                return findfriend.userID;
            }
        }

        return null;
    }
    #endregion

}
