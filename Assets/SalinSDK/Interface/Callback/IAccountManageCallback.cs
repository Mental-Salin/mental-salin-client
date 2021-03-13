namespace SalinSDK
{
    public interface IAccountManageCallback
    {
        void OnSignUp();
        void OnSignUpFail(ErrorCode errorCode);
        void OnLogIn(UserInfo info);
        void OnLogInFail(ErrorCode errorCode);
        void OnLogOut();
        void OnLogOutFail(ErrorCode errorCode);
    }
}