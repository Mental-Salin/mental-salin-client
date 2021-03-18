using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SalinSDK;

public class DemoRoomManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkObject myobj = XRSocialSDK.CreateInstance("DemoTestCube");
        myobj.Obj.transform.position = new Vector3(Random.Range(-30.0f, 30.0f), 2.1f, Random.Range(-30.0f, 30.0f));
        myobj.Obj.GetComponent<DemoObjectMovement>().IsMine = true;
        //int num = SalinSocialSDK.myPlayer.dynamicCodeInRoom;
        //Debug.Log("dynamicCodeInRoom: " + num.ToString());
        Renderer Myrend = myobj.Obj.GetComponent<Renderer>();
        Myrend.material.color = new Color(255, 0, 0);

        ChatManager.Connect(XRSocialSDK.currentRoom.RoomName, UserManager.Instance.userInfo.userNickname);
        VoiceManager.Connect(XRSocialSDK.currentRoom.RoomName);
    }
}
