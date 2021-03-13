using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SalinSDK;
using System;

public class DemoRoomPlayerInfoUI : MonoBehaviour
{
    public Text DefaultTextPref;

    public Text IdText;
    public Text NicknameText;

    public Toggle InviteToggle;

    public GameObject PropRoot;

    public Button KickButton;

    private string playerid;

    public void SetInfoText(Player Friendplayer)
    {
        if (Friendplayer == null)
            return;

        playerid = Friendplayer.userId;
        IdText.text = "ID - " + Friendplayer.userId;
        NicknameText.text = "NickName - " + Friendplayer.userNickname;
        InviteToggle.isOn = Friendplayer.allowInvite;

        SetMyPropertyContent(Friendplayer);
    }

    public void SetMyPropertyContent(Player Friendplayer)
    {
        Text[] Rootchildren = PropRoot.GetComponentsInChildren<Text>();

        if (Rootchildren != null)
        {
            foreach (Text child in Rootchildren)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var items in Friendplayer.userProperties)
        {
            Text item = GameObject.Instantiate(DefaultTextPref, PropRoot.transform);
            item.text = "Key: " + items.Key.ToString() + " / Value: " + items.Value.ToString();

        }
    }
    public void SetKickButtonListener(Action action)
    {
        KickButton.interactable = true;
        KickButton.onClick.RemoveAllListeners();
        KickButton.onClick.AddListener(() => { action(); });
    }

    public void SetSoundOnOff(bool ison)
    {
        if (ison)
            VoiceManager.SoundOn(playerid);
        else
            VoiceManager.SoundOff(playerid);
    }
}
