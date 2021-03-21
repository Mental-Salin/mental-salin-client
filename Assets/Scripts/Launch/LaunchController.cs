using System.Collections;
using System.Collections.Generic;
using SalinSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchController : SalinCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        XRSocialSDK.ConnectToMainServer();   
    }

    public override void OnConnectedMainServer(string appToken)
    {
        MalinLog.Get().ShowLog("Success to connect to the main server.");

        if (appToken.Length == 0)
            MalinLog.Get().ShowLog("AppToken is Empty! XR social SDK will not work... Please check the Api key and user name.");

        SalinTokens.AppToken = appToken;
        //SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
        SceneManager.LoadScene("DemoLoginScene", LoadSceneMode.Single);
        
    }

    public override void OnConnectedMainServerFail(ErrorCode errorCode)
    {
        MalinLog.Get().ShowLog($"Fail to connect to the main server... code : {errorCode}");
    }
}
