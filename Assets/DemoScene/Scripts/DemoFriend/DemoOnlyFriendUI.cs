using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;

public class DemoOnlyFriendUI : MonoBehaviour
{
    public Button RefreshButton;

    public Button SearchButton;
    public InputField SearchInputfield;

    public GameObject FriendInfoPref;

    public GameObject FriendListRoot;
    public GameObject SearchListRoot;

    public void AddFriendList()
    {
        RefreshButton.interactable = true;
        InitFriendList();

        List<Friend> friendList = UserManager.Instance.GetFriendList();

        if (friendList == null || friendList.Count < 1)
            return;

        foreach (Friend items in friendList)
        {
            GameObject FriendContent = GameObject.Instantiate(FriendInfoPref, FriendListRoot.transform);

            if (FriendContent == null)
                return;

            DemoFriendInfoUI contentui = FriendContent.GetComponent<DemoFriendInfoUI>();

            if (contentui == null)
                return;

            contentui.SetContentText(items);
        }

    }

    public void StartToSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchInputfield.text))
            return;

        SearchButton.interactable = false;
        InitSearchList();
        FriendManager.SearchFriendList(SearchInputfield.text);
    }

    public void AddFriendSearchList(List<Friend> SearchFriend)
    {
        SearchInputfield.text = "";

        if (SearchFriend == null || SearchFriend.Count < 1)
        {
            Debug.Log("친구없음");

        }
        else
        {
            foreach (Friend items in SearchFriend)
            {
                GameObject FriendContent = GameObject.Instantiate(FriendInfoPref, SearchListRoot.transform);

                if (FriendContent == null)
                    return;

                DemoFriendInfoUI contentui = FriendContent.GetComponent<DemoFriendInfoUI>();

                if (contentui == null)
                    return;

                contentui.SetContentText(items);
            }
        }

        SearchButton.interactable = true;
    }

    public void ClickRefresh()
    {
        RefreshButton.interactable = false;
        FriendManager.UpdateState();
    }


    private void InitFriendList()
    {
        DemoFriendInfoUI[] Rootchildren = FriendListRoot.GetComponentsInChildren<DemoFriendInfoUI>();

        if (Rootchildren != null)
        {
            foreach (DemoFriendInfoUI child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void InitSearchList()
    {
        DemoFriendInfoUI[] Rootchildren = SearchListRoot.GetComponentsInChildren<DemoFriendInfoUI>();

        if (Rootchildren != null)
        {
            foreach (DemoFriendInfoUI child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
