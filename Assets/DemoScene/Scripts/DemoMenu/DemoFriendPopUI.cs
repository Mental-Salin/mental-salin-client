using SalinSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoFriendPopUI : MonoBehaviour
{
    public Toggle FriendToggle;
    public Toggle SearchToggle;

    public Button RefreshButton;


    public GameObject FriendListRoot;

    public GameObject SearchWindow;
    public Button SearchButton;
    public InputField SearchInputfield;

    public GameObject FriendInfoPref;

    public void FriendInit(bool ison)
    {
        if (ison)
        {
            FriendToggle.isOn = true;
            FriendToggle.interactable = false;
            SearchToggle.interactable = true;
            SearchToggle.isOn = false;
            SearchWindow.SetActive(false);
            RefreshButton.interactable = true;
            InitFriendList();
            AddFriendList();
        }
    }

    public void SearchInit(bool ison)
    {
        if (ison)
        {
            FriendToggle.interactable = true;
            SearchToggle.interactable = false;
            RefreshButton.interactable = false;
            FriendToggle.isOn = false;
            SearchWindow.SetActive(true);
            InitFriendList();
        }
    }

    public void AddFriendList()
    {
        RefreshButton.interactable = true;
        SearchToggle.interactable = true;

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
        FriendToggle.interactable = false;
        SearchToggle.interactable = false;
        InitFriendList();
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
                GameObject FriendContent = GameObject.Instantiate(FriendInfoPref, FriendListRoot.transform);

                if (FriendContent == null)
                    return;

                DemoFriendInfoUI contentui = FriendContent.GetComponent<DemoFriendInfoUI>();

                if (contentui == null)
                    return;

                contentui.SetContentText(items);
            }
        }

        SearchButton.interactable = true;
        FriendToggle.interactable = true;
        SearchToggle.interactable = true;
    }

    public void ClickRefresh()
    {
        RefreshButton.interactable = false;
        SearchToggle.interactable = false;
        FriendManager.UpdateState();

        InitFriendList();
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

    public void ClosePopUI()
    {
        InitFriendList();
    }
}
