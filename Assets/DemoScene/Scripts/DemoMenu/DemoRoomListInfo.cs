using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class DemoRoomListInfo : MonoBehaviour
{
    public Text RoomName;
    public Text RoomPlayerCount;

    public Button EntryButton;
    public Text EntryButtonText;

    string saveroomname;

    public void SetRoomInfo(string name, bool isopen, int currentcnt = 0, int maxcnt = 0)
    {
        RoomName.text = SplitStringByte(name, 18);
        saveroomname = name;
        if (!isopen)
        {
            EntryButtonText.text = "참가불가";
            EntryButton.interactable = false;
            return;
        }

        RoomPlayerCount.text = currentcnt.ToString() + "/" + maxcnt.ToString();
    }

    public static string SplitStringByte(string content, int ByteLength)

    {
        string temp = "";
        string resetstring = "";
        int currentlength = 0;

        for (int i = 0; i < content.Length; i++)
        {
            string splitcurrentindex = content.Substring(i, 1);
            temp += splitcurrentindex;

            //currentlength += Encoding.Default.GetByteCount(splitcurrentindex);
            currentlength += Mathf.Min(Encoding.Default.GetByteCount(splitcurrentindex), 2);
            //Debug.Log("encoding: " + (Encoding.Default.GetByteCount(splitcurrentindex).ToString()));
            if (currentlength > ByteLength)
            {
                resetstring = temp.Substring(0, temp.Length - 1) + "...";

                break;

            }
            else 
                resetstring = temp;

        }

        return resetstring;
;
    }
}
