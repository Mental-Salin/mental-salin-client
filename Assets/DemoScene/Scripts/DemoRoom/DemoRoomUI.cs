using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;

public class DemoRoomUI : MonoBehaviour
{
    #region prefabs
    [Header("Prefabs")]
    public Button FriendPref;
    public Text DefaultTextPref;
    #endregion
    [Space(10f)]

    #region Room Info UI
    [Header("Room Info UI")]
    public Text RoomNameText;
    public GameObject RoomInfoPopup;
    public DemoRoomInfoUI RoomInfoScript;
    #endregion
    [Space(10f)]

    #region Toggle Buttons
    [Header("Toggel Buttons")]
    public Toggle FriendToggle;
    //public Toggle MyInfoToggle;
    public Toggle RoomInfoToggle;
    public Toggle BlockListToggle;
    #endregion
    [Space(10f)]

    #region Friend Popup
    [Header("Friend Popup")]
    public GameObject FriendPopup;
    public DemoFriendPopUI FriendPopScript;
    #endregion
    [Space(10f)]

    #region MyInfo Popup
    [Header("MyInfo Popup")]
    public GameObject MyInfoPopup;
    public DemoMyInfoPopUI MyInfoScript;
    #endregion
    [Space(10f)]

    #region Current Room Player UI
    [Header("Current Room Player UI")]
    public DemoRoomPlayerInfoUI PlayerInfoScript;
    public GameObject RoomJoinListRoot;
    public GameObject PlayerInfoPopup;
    #endregion
    [Space(10f)]

    #region Voice Chat UI
    [Header("Voice Chat UI")]
    public GameObject SoundSetting;
    public GameObject SoundReconnetButton;
    #endregion
    [Space(10f)]

    #region Text Chat UI
    [Header("Text Chat UI")]
    public InputField ChatTextInputField;
    public InputField FriendInputField;
    public InputField WhisperInputField;
    public DemoChatUIControl ChatBoxScript;
    #endregion
    [Space(10f)]

    #region Player Block UI
    [Header("Player Block UI")]
    public GameObject BlockList;
    public GameObject BlockListRoot;
    public InputField BlockUserIdInput;
    #endregion
    [Space(10f)]

    #region Invite Message
    [Header("Invite Message Popup")]
    public Text InviteText;
    public GameObject InvitePopup;
    #endregion

    string selectedFriendName;

    // Start is called before the first frame update
    void Start()
    {
        //room info init
        RoomInfoScript.SetRoomInfo();
        MyInfoScript.SetInfoText();

        //RoomSettingToogleOn();

        RoomInfoToggle.isOn = true;
        FriendToggle.isOn = true;
        //MyInfoToggle.isOn = true;

        RoomNameText.text = "ROOM NAME: " + XRSocialSDK.currentRoom.RoomName;

        AddRoomPlayerList();

        GameObject[] disableobj = GameObject.FindGameObjectsWithTag("PopupRoot");

        if (disableobj == null || disableobj.Length < 1)
            return;

        foreach (GameObject items in disableobj)
        {
            items.SetActive(false);
        }
    }

    public void AddRoomProperty()
    {
        RoomInfoScript.SetPropertyContent();
    }

    public void LeaveRoomListener()
    {
        XRSocialSDK.LeaveRoom();
    }

    private void RoomSettingToogleOn()
    {
        if (XRSocialSDK.myPlayer.isKeyPlayer)
            RoomInfoToggle.gameObject.SetActive(true);

        else
            RoomInfoToggle.gameObject.SetActive(false);
    }

    #region Invite Message Popup
    public void ReceiveInviteMessage(string senderId, string roomName, string hostName)
    {
        InviteText.text = "<" + roomName + "> 방에서 초대되었습니다.\n" + "현재 다른방으로 이동할 수 없습니다.";
        InvitePopup.SetActive(true);
    }
   
    public void CloseInvitePopup()
    {
        InvitePopup.SetActive(false);
    }
    #endregion

    #region Toggle Listener
    public void IsOpenFriendPopup(bool IsOn)
    {
        if (!IsOn)
        {
            FriendPopup.SetActive(true);
            //MyInfoToggle.interactable = false;
            RoomInfoToggle.interactable = false;
            FriendPopScript.FriendInit(true);
        }
        else
        {
            FriendPopup.SetActive(false);
            FriendToggle.isOn = true;
            //MyInfoToggle.interactable = true;
            RoomInfoToggle.interactable = true;
            FriendPopScript.ClosePopUI();
        }
    }

    public void IsOpenMyInfoPopup(bool IsOn)
    {
        if (!IsOn)
        {
            MyInfoPopup.SetActive(true);
            FriendToggle.interactable = false;
            RoomInfoToggle.interactable = false;
            MyInfoScript.SetInfoText();

        }
        else
        {
            MyInfoPopup.SetActive(false);
            //MyInfoToggle.isOn = true;
            FriendToggle.interactable = true;
            RoomInfoToggle.interactable = true;
        }
    }

    public void IsOpenRoomInfoPopup(bool IsOn)
    {
        if (!IsOn)
        {
            RoomInfoPopup.SetActive(true);
            FriendToggle.interactable = false;
            //MyInfoToggle.interactable = false;

        }
        else
        {
            RoomInfoPopup.SetActive(false);
            RoomInfoToggle.isOn = true;
            FriendToggle.interactable = true;
            //MyInfoToggle.interactable = true;
        }
    }

    public void IsOpenBlockListPopup(bool IsOn)
    {
        if (IsOn)
        {
            BlockList.SetActive(true);
        }
        else
        {
            BlockList.SetActive(false);
            BlockListToggle.isOn = false;
        }
    }
    #endregion

    #region Current Room Player List
    public void AddRoomPlayerList()
    {
        RoomSettingToogleOn();

        Button[] Rootchildren = RoomJoinListRoot.GetComponentsInChildren<Button>();

        if (Rootchildren != null)
        {
            foreach (Button child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }

        Dictionary<string, Player> _playerList = new Dictionary<string, Player>();
        _playerList = XRSocialSDK.currentRoom.PlayerList;

        if (_playerList == null || _playerList.Count < 1)
            return;

        Debug.Log("AddRoomPlayerList: " + _playerList.Count.ToString());

        foreach (var items in _playerList)
        {
            Debug.Log("AddRoomPlayerList: " + items.Key);

            Button item = GameObject.Instantiate(FriendPref, RoomJoinListRoot.transform);

            GameObject crownicon = item.transform.GetChild(1).gameObject;
            GameObject meicon = item.transform.GetChild(2).gameObject;

            Text ChildText = item.GetComponentInChildren<Text>();

            string richtext = "<color=#0000ffff>" + items.Value.userNickname + "</color>";

            if (items.Value.isKeyPlayer)
            {
                crownicon.SetActive(true);
            }
 
            if (UserManager.Instance.userID.Equals(items.Value.userId))
            {
                ChildText.text = richtext;
                item.interactable = false;
                if(!items.Value.isKeyPlayer)
                    meicon.SetActive(true);
            }
            else
            {
                ChildText.text = items.Value.userNickname;
                item.onClick.AddListener(() => { RoomPlayerClickListener(items.Key); });
            }
        }
    }

    public void RoomPlayerClickListener(string useracc)
    {
        PlayerInfoPopup.SetActive(true);

        FriendToggle.interactable = false;
        //MyInfoToggle.interactable = false;
        RoomInfoToggle.interactable = false;

        Player ClickedPlayer = XRSocialSDK.GetUser(useracc);
        if (ClickedPlayer == null)
            return;

        PlayerInfoScript.SetInfoText(ClickedPlayer);

        if (XRSocialSDK.myPlayer.isKeyPlayer)
        {
            PlayerInfoScript.SetKickButtonListener(PlayerKick);
        }

        selectedFriendName = useracc;
    }
 
    public void PlayerKick()
    {
        Player selectedPlayer = XRSocialSDK.GetUser(selectedFriendName);
        XRSocialSDK.UserKick(selectedPlayer);
        CloseFriendPopup();
    }

    public void CloseFriendPopup()
    {
        selectedFriendName = "";
        PlayerInfoPopup.SetActive(false);
        FriendToggle.interactable = true;
        //MyInfoToggle.interactable = true;
        RoomInfoToggle.interactable = true;

    }
    #endregion

    #region Chat box 
    public void SendChat()
    {
        if (ChatTextInputField.text == "")
            return;

        ChatManager.SendMessageInRoom(ChatTextInputField.text);
        ChatTextInputField.text = "";
    }

    public void SendWhisperChat()
    {
        if (FriendInputField.text == "" || WhisperInputField.text == "")
            return;

        ChatManager.SendWhisperMessageTarget(FriendInputField.text, WhisperInputField.text);
        WhisperInputField.text = "";
        ChatTextInputField.text = "";
    }

    public void LeaveRoomChatMessage(Player LeftPlayer)
    {
        if (LeftPlayer == null) 
            return;

        string msg = "<b><color=#0000ffff>" + LeftPlayer.userNickname + "님이 퇴장했습니다.</color></b>" + "\n";
        ChatBoxScript.UpdateChatBox(msg);
    }

    public void VoiceConnectChatMessage(bool isconnect)
    {
        string msg = null;

        if (isconnect)
            msg = "<b><color=#008000ff>" + "Voice Chat이 연결되었습니다.</color></b>" + "\n";
        else
            msg = "<b><color=#ff0000ff>" + "Voice Chat의 연결이 끊겼습니다.</color></b>" + "\n";

        ChatBoxScript.UpdateChatBox(msg);
    }
    #endregion

    #region Player Block

    public void AddRoomBlockList()
    {
        Text[] Rootchildren = BlockListRoot.GetComponentsInChildren<Text>();

        if (Rootchildren != null)
        {
            foreach (Text child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }

        Dictionary<string, string> _blockedPlayerIdList = new Dictionary<string, string>();

        _blockedPlayerIdList = XRSocialSDK.currentRoom.BlockedPlayerIdList;

        if (_blockedPlayerIdList == null || _blockedPlayerIdList.Count < 1)
            return;

        Debug.Log("BlockedPlayerIdList: " + _blockedPlayerIdList.Count.ToString());

        foreach (var items in _blockedPlayerIdList)
        {
            Debug.Log("BlockedPlayerIdList Key:" + items.Key + " / Value:" + items.Value);

            Text item = GameObject.Instantiate(DefaultTextPref, BlockListRoot.transform);

            List<Friend> friendlist = UserManager.Instance.GetFriendList();

            if (friendlist == null)
                return;

            foreach (Friend findfriend in friendlist)
            {
                if (findfriend.userID == items.Value)
                    item.text = findfriend.userAccount;
            }
            
        }
    }

    public void AddBlockUser()
    {
        if (BlockUserIdInput.text == "")
            return;

        string friendid = DemoControlManager.Instance.GetFriendId(BlockUserIdInput.text);

        if (friendid != null)
            XRSocialSDK.UserBlocking(friendid);

        BlockUserIdInput.text = "";
    }

    public void RemoveBlockUser()
    {
        if (BlockUserIdInput.text == "")
            return;

        string friendid = DemoControlManager.Instance.GetFriendId(BlockUserIdInput.text);

        if (friendid != null)
            XRSocialSDK.UserUnblock(friendid);
       
        BlockUserIdInput.text = "";
        AddRoomBlockList();
    }

    #endregion

    #region Voice Chat

    public void SoundOnOff(bool isOn)
    {
        if (isOn)
            VoiceManager.SoundOnAll();
        else
            VoiceManager.SoundOffAll();
    }

    public void MicOnOff(bool isOn)
    {
        if (isOn)
            VoiceManager.MicOn();
        else
            VoiceManager.MicOff();
    }

    public void SetVoiceReconnectButton()
    {
        SoundSetting.SetActive(false);
        SoundReconnetButton.SetActive(true);
    }
    public void VoiceReconnect()
    {
        SoundReconnetButton.SetActive(false);
        VoiceManager.ReConnect();
    }

    public void ConnectedVoice()
    {
        SoundSetting.SetActive(true);
        VoiceConnectChatMessage(true);
    }
    #endregion

}
