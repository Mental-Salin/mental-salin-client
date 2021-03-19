using System.Collections;
using System.Collections.Generic;
using SalinSDK;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class RoomChat : SalinCallbacks
{
    public Button btnChat;
    public InputField inputChat;
    public ScrollView ScrollViewChat;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void OnReceiveMessage<T>(T data)
    {
        base.OnReceiveMessage(data);
    }
}
