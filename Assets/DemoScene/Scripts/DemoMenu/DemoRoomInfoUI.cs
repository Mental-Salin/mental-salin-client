using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;

public class DemoRoomInfoUI : MonoBehaviour
{
    public Text DefaultTextPref;

    public Text RoomNameText;
    public Toggle RoomOpenToggle;
    public InputField RoomPwInput;
    public InputField RoomMaxPlayerInput;

    public InputField AddKeyInput;
    public InputField AddValueInput;
    public InputField DeleteKeyInput;

    public Button AddButton;
    public Button RemoveButton;

    public GameObject PropRoot;

    public Button CloseButton;

    public void SetRoomInfo()
    {
        RoomNameText.text = "ROOM NAME - " + XRSocialSDK.currentRoom.RoomName;
        RoomOpenToggle.isOn = XRSocialSDK.currentRoom.IsOpen;

        if(XRSocialSDK.currentRoom.Password != "")
        {
            RoomPwInput.text = XRSocialSDK.currentRoom.Password;
        }

        if(XRSocialSDK.currentRoom.MaxPlayerCount > 0)
        {
            RoomMaxPlayerInput.text = XRSocialSDK.currentRoom.MaxPlayerCount.ToString();
        }

        SetPropertyContent();

    }
    public void SetPropertyContent()
    {
        Text[] Rootchildren = PropRoot.GetComponentsInChildren<Text>();

        if (Rootchildren != null)
        {
            foreach (Text child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }

        Room RoomProp = XRSocialSDK.currentRoom;

        //Debug.Log("AddRoomPlayerList: " + RoomProp.RoomProperties.Count.ToString());

        foreach (var items in RoomProp.RoomProperties)
        {
            Debug.Log("RoomPropList: " + items.Key.ToString() + " / value:" + items.Value.ToString());

            Text item = GameObject.Instantiate(DefaultTextPref, PropRoot.transform);
            item.text = "Key: " + items.Key.ToString() + " / Value: " + items.Value.ToString();

        }
    }

    public void AddRoomProp()
    {
        if (AddKeyInput.text != null && AddValueInput.text != null)
        {
            var element = new KeyValuePair<object, object>(AddKeyInput.text, AddValueInput.text);
            XRSocialSDK.currentRoom.AddRoomProperties(element);
        }

        AddKeyInput.text = "";
        AddValueInput.text = "";

        SetPropertyContent();
    }

    public void RemoveRoomProp()
    {
        if (DeleteKeyInput.text != null)
        {
            XRSocialSDK.currentRoom.RemoveRoomProperties(DeleteKeyInput.text);
        }

        DeleteKeyInput.text = "";

        SetPropertyContent();
    }

    public void SetRoomOpen(bool ischeck)
    {
        XRSocialSDK.currentRoom.SetIsOpen(ischeck);
    }

    public void SetRoomPassword(string newpassword)
    {
        XRSocialSDK.currentRoom.SetPassword(newpassword);
    }

    public void SetRoomMaxPlayer(string newmaxnum)
    {
        XRSocialSDK.currentRoom.SetMaxPlayerCount(int.Parse(newmaxnum));
    }

}
