using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SalinSDK;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DemoConstants;

public class DemoLogin : SalinCallbacks
{
    public bool OnlyUseLoginScene = false;

    #region Popup Object
    [Header("Popup Object")]
    public GameObject LoginPopup;
    public GameObject SignupPopup;
    #endregion

    #region Notify Text
    [Header("Notify Text")]
    public Text LoginNotifyText;
    public Text SignupNotifyText;
    #endregion

    #region Login InputField
    [Header("Login InputField")]
    public InputField IdInput;
    public InputField PwInput;
    #endregion

    #region SignUp InputField
    [Header("SignUp InputField")]
    public InputField SingupIdInput;
    public InputField SingupNicknameInput;
    public InputField SingupPwInput;
    public InputField SingupConfirmPwInput;
    #endregion

    public Dropdown GenderDropdown;

    #region Login Button
    [Header("Login Button")]
    public Button LoginButton;
    public Button SingUpButton;
    public Button LogoutButton;
    #endregion

    #region SignUp Button
    [Header("SignUp Button")]
    public Button CancelButton;
    public Button CreateButton;
    #endregion

    private string SaveId;
    private string SavePw;

    public GameObject MenuPrefab;
    private void Start()
    {
        LoginPopup.SetActive(true);
        SignupPopup.SetActive(false);

    }

    #region Login Popup
    //Login Button Listener
    public void StartLogin()
    {
        if (!XRSocialSDK.IsConnected_MainServer)
        {
            LoginNotifyText.text = "Cannot find Token";
            XRSocialSDK.ConnectToMainServer();
            return;
        }

        ControlInputButton(false, "Login");
        AccountManager.Login(IdInput.text, PwInput.text);

    }

    //SignUp Button Listener
    public void OpenSignupPopup()
    {
        LoginPopup.SetActive(false);
        SignupPopup.SetActive(true);
    }

    #endregion

    #region SignUp Popup
    //Create Button Listener
    public void SignUp()
    {
        if (!XRSocialSDK.IsConnected_MainServer)
            return;

        if(!SingupPwInput.text.Equals(SingupConfirmPwInput.text))
        {
            SignupNotifyText.text = "Passwords are not identical. Try again.";
            return;
        }

        ControlInputButton(false, "Signup");

        SaveId = SingupIdInput.text;
        SavePw = SingupPwInput.text;

        Gender selectedGender = GenderDropdown.value == 0 ? Gender.Female : Gender.Male;
        AccountManager.SignUp(SingupIdInput.text, SingupPwInput.text, SingupNicknameInput.text, selectedGender);
    }

    //Cancel Button Listener
    public void CloseSignupPopup()
    {
        SignupNotifyText.text = "";
        SignupPopup.SetActive(false);
        LoginPopup.SetActive(true);
    }

    //SignUp Confirm Password InputField OnEndEdit Listener
    public void CompareConfirmPassword(string confirmpw)
    {
        //NotifyText 로 디버그 메세지 출력 
        if (!SingupPwInput.text.Equals(confirmpw))
        {
            SignupNotifyText.text = "Passwords are not identical. Try again.";
        }
        else
        {
            SignupNotifyText.text = "";
        }
    }

    #endregion

    #region LogOut
    public void ClickLogOut()
    {
        LogoutButton.interactable = false;
        AccountManager.LogOut();
    }
    #endregion
    #region Button Disable
    //로그인이나 회원가입이 서버를 통해 진행중일때 중복으로 불리지 않게 버튼 비활성화 처리
    private void ControlInputButton(bool OnIntract, string States)
    {
        if (States.Equals("Login"))
        {
            LoginButton.interactable = OnIntract;
            SingUpButton.interactable = OnIntract;
        }

        if (States.Equals("Signup"))
        {
            CancelButton.interactable = OnIntract;
            CreateButton.interactable = OnIntract;
        }
    }
    #endregion

    #region callback override

    public override void OnLogIn(UserInfo info)
    {
        Debug.Log("Login callback: " + info.userAccount);

        if (OnlyUseLoginScene)
        {
            LoginNotifyText.text = "로그인 성공";
            SignupNotifyText.text = "로그인 성공";
            LogoutButton.interactable = true;
        }
        else
        {
            if (DemoControlManager.Instance.currentState != DemoState.login)
                return;

            Instantiate(MenuPrefab);
            DemoControlManager.Instance.currentState = DemoState.menu;
            GameObject thisPrefab = this.transform.parent.gameObject;
            if (thisPrefab != null)
                Destroy(thisPrefab);

            //SceneManager.LoadScene("DemoMenuScene");
        }
    }

    public override void OnLogInFail(ErrorCode errorCode)
    {
        base.OnLogInFail(errorCode);
        ControlInputButton(true, "Login");
        Debug.Log("OnLogInFail + " + errorCode.ToString());
        LoginNotifyText.text = errorCode.ToString();

    }

    public override void OnSignUp()
    {
        if (OnlyUseLoginScene)
            SignupNotifyText.text = "회원가입 성공";

        AccountManager.Login(SaveId, SavePw);
    }

    public override void OnSignUpFail(ErrorCode errorCode)
    {
        ControlInputButton(true, "Signup");
        Debug.Log("OnSignUpFail + " + errorCode.ToString());
        SignupNotifyText.text = errorCode.ToString();

    }

    public override void OnLogOut()
    {
        if (OnlyUseLoginScene)
        {
            LoginNotifyText.text = "";

            Destroy(DemoControlManager.Instance.gameObject);
            Destroy(UserManager.Instance.gameObject);
            SceneManager.LoadScene("DemoOnlyLoginScene");
        }
    }

    public override void OnLogOutFail(ErrorCode errorCode)
    {

    }

    #endregion

}
