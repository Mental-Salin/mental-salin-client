namespace SalinSDK
{
    public interface IAccountManageable
    {
        void SignUp(string account, string password, string nickname, Gender gender);
        void LogIn(string account, string password);
        void LogOut( );
    }
}