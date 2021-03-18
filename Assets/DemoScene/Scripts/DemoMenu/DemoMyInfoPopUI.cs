using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;

public class DemoMyInfoPopUI : MonoBehaviour
{
    public Text DefaultTextPref;

    public Text IdText;
    public Text NicknameText;

    public Toggle InviteToggle;

    public InputField AddKeyInput;
    public InputField AddValueInput;
    public InputField DeleteKeyInput;

    public Button AddButton;
    public Button RemoveButton;
   
    public GameObject PropRoot;

    public Button CloseButton;

    bool IsSet = false;
    public void SetInfoText()
    {
        if (IsSet)
            return;

        IsSet = true;

        Player myplayer = XRSocialSDK.myPlayer;
        IdText.text = UserManager.Instance.userInfo.userAccount;
        NicknameText.text = myplayer.userNickname;
        InviteToggle.isOn = myplayer.allowInvite;

        SetMyPropertyContent();
    }

    public void SetMyPropertyContent()
    {
        Text[] Rootchildren = PropRoot.GetComponentsInChildren<Text>();

        if (Rootchildren != null)
        {
            foreach (Text child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }

        Player selectedPlayer = XRSocialSDK.myPlayer;

        foreach (var items in selectedPlayer.userProperties)
        {
            Debug.Log("AddRoomPlayerList: " + items.Key);

            Text item = GameObject.Instantiate(DefaultTextPref, PropRoot.transform);
            item.text = "Key: " + items.Key.ToString() + " / Value: " + items.Value.ToString();

        }
    }

    public void AddPlayerProp()
    {
        if (AddKeyInput.text != null && AddValueInput.text != null)
        {
            var element = new KeyValuePair<object, object>(AddKeyInput.text, AddValueInput.text);
            XRSocialSDK.myPlayer.AddUserProperties(element);
        }

        AddKeyInput.text = "";
        AddValueInput.text = "";

        SetMyPropertyContent();
    }

    public void RemovePlayerProp()
    {
        if (DeleteKeyInput.text != null)
        {
            XRSocialSDK.myPlayer.RemoveUserProperties(DeleteKeyInput.text);
        }

        DeleteKeyInput.text = "";

        SetMyPropertyContent();
    }

    public void ChangeAllowInvite(bool IsCheck)
    {
        XRSocialSDK.myPlayer.SetAllowInvite(IsCheck);
    }
}
