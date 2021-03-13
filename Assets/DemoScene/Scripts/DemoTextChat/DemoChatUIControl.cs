using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoChatUIControl : MonoBehaviour
{
    public Text ChatTextBox;

    public Scrollbar VerticalScroll;

    //public bool OnlyWhisperChat;

    public void UpdateChatBox(string msg)
    {
        ChatTextBox.text += msg;
        VerticalScroll.value = 0;
    }

}
