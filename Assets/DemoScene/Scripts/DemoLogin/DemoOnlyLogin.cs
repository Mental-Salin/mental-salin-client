using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SalinSDK;
using UnityEngine.UI;

public class DemoOnlyLogin : SalinCallbacks
{
    #region Login 
    [Header("Login")]
    public InputField IdInput;
    public InputField PwInput;
    public Button LoginButton;
    #endregion

    #region Notify Text
    [Header("Notify Text")]
    public Text LoginNotifyText;
    #endregion

    public void StartLogin()
    {
        if (!XRSocialSDK.IsConnected_MainServer)
        {
            LoginNotifyText.text = "Cannot find Token";
            XRSocialSDK.ConnectToMainServer();
            return;
        }

        LoginButton.interactable = false;
        AccountManager.Login(IdInput.text, PwInput.text);

    }

    public override void OnLogInFail(ErrorCode errorCode)
    {
        LoginButton.interactable = true;
        Debug.Log("OnLogInFail + " + errorCode.ToString());
        LoginNotifyText.text = errorCode.ToString();

    }

}
