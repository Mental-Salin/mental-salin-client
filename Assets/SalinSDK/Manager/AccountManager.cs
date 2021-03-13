using UnityEngine;

namespace SalinSDK
{
    /// <summary>
    /// RequestData를 생성하고 Transmitter를 통해 
    /// SignUp, LogIn, LogOut을 요청합니다.
    /// </summary>
    public static class  AccountManager
    {
        static IAccountManageable _accountManager;
        static IAccountManageable accountManager
        {
            get
            {
                if(_accountManager == null)
                {
                    _accountManager = new BaseAccountManager();

                }
                return _accountManager;
            }
        }

        /// <summary>
        /// 회원가입을 위한 함수
        /// </summary>
        /// <param name="account">아이디</param>
        /// <param name="password">비밀번호</param>
        /// <param name="nickname">닉네임</param>
        /// <param name="gender">성별</param>
        static public void SignUp(string account, string password, string nickname, Gender gender)
        {
            accountManager.SignUp(account, password, nickname, gender);
        }

        static public void SignUp(string account, string password, string nickname)
        {
            accountManager.SignUp(account, password, nickname, Gender.Female);
        }

        /// <summary>
        /// 로그인을 위한 함수
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        static public void Login(string account, string password)
        {
            accountManager.LogIn( account, password);
        }
        
        /// <summary>
        /// 로그아웃을 위한 함수
        /// </summary>
        static public void LogOut()
        {
            accountManager.LogOut();
        }
    }
}
