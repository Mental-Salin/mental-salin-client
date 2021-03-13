namespace SalinSDK
{
    public class SalinConstants
    {

        public const string defaultTemplate = "D_TP00001";

        public const int defaultInt = -9999;
        public const float defaultFloat = -99999.9f;

        public const int loginCheckTime = 60;

        public const float errorTime = 10;
        public const int retryCount = 3;

        public const string workspaceID = "WS214363943629338875880943936577131386443";

        public const string dataSettingsFileName = "SalinData";
    }

    /// <summary>
    /// 테스트 서버 주소
    /// </summary>
    class SalinServerURL
    {
        public const string serverUrl = "https://dev1.epiclive.io/ords/pdb1/sdk/";
    }

    /// <summary>
    /// 테스트 서버 API
    /// </summary>
    class SalinServerAPI
    {
        public const string signUp = "user/10/new/account";
        public const string login = "user/11/login";
        public const string logout = "user/12/logout";
        public const string appToken = "api/key";

        public const string reqFriend =     "user/16/request/friend";
        public const string reqFriendCancel = "user/16/request/friend";
        public const string responseFriend = "user/17/response/friend";
        public const string removeFriend = "user/18/remove/friend";
        public const string searchFriend = "user/19/search/friend";
        public const string updateState = "user/20/update/status";

    }

    class SalinAPIKey
    {
        public const string apikey = "apikey";
        public const string code = "code";
        public const string workspaceId = "workspace_id";
        public const string apptoken = "apptoken";
        public const string account = "account";
        public const string password = "password";
        public const string nickname = "username";
        public const string userID = "mid";
        public const string sex = "sex";
        public const string pkgname = "pkgname";
        public const string unixtime = "unixtime";
        public const string loginKey = "login_key";
        public const string sessionKey = "session_key";

        public const string friendID = "fid";
        public const string processID = "process_id";
        public const string type = "type";
    }

    class JsonKey
    {
        public const string user = "user";
        public const string friend = "friend";
        public const string friendID = "fid";
        public const string FavatarIndex = "ava_index";
        public const string account = "account";
        public const string nickname = "username";
        public const string friendStatus = "status";
        public const string processID = "process_id";
    }

    public enum ErrorCode
    {
        None = 0,
        Success = 2000, //	* 요청정상 처리.
        Error = 9999, //	Result Error

        Null_PkgName = 1000,        //	유니티 패키지 이름을 입력하세요.
        Null_SessionKey = 1001,     //	세션 키를 입력하세요.
        Null_ProcessKey = 1002,     //	프로세스 Id를 입력하세요.
        Null_Unixtime = 1003,       //	Unixtime을 입력하세요.
        Null_CloudKey = 1004,       //	클라우드 키를 입력하세요.
        Null_FileKey = 1005,        //	파일 키를 입력하세요.
        Null_LoginKey = 1006,       //	로그인 키를 입력하세요.

        Null_Account = 2001,        //	계정을 입력하세요.
        Null_Password = 2002,       //	비밀번호를 입력하세요.
        Null_UserId = 2003,         //	유저 ID를 입력하세요.
        Null_FriendId = 2004,       //	친구 ID를 입력하세요.
        Null_UserName = 2005,       //	유저 이름을 입력하세요.
        Null_NewUserName = 2006,    //	새로운 유저 이름을 입력하세요.
        Null_AvatarIdx = 2007,      //	아바타 번호를 입력하세요.
        Null_NewAvaTarIdx = 2008,   //	새로운 아바타 번호를 입력하세요.
        Null_Sex = 2009,            //	성별을 입력하세요.
        Null_Type = 2010,           //	구분을 입력하세요.
        Null_NewPassword = 2011,    //	새로운 비밀번호를 입력하세요.
        Null_CurPassword = 2012,    //	현재 비밀번호를 입력하세요.
        Null_Email = 2013,          //	이메일을 입력하세요.

        Null_ProviderId = 3000,     //	공급자 Id를 입력하세요.
        Null_Workspace = 3001,      //	Workspace Id를 입력하세요.
        Null_Cp = 3002,             //	CP Id를 입력하세요.
        Null_Category = 3003,       //	카테고리 Id를 입력하세요.
        Null_Genre = 3004,          //	장르를 Id 입력하세요.
        Null_ContentsId = 3005,     //	콘텐츠 Id를 입력하세요.

        Null_CategoryName = 3501,   //	카테고리 이름을 입력하세요.
        Null_GenreName = 3502,      //	장르 이름을 입력하세요.
        Null_ContentsName = 3503,   //	콘텐츠 이름을 입력하세요.
        Null_VideoName = 3504,      //	영상 이름을 입력하세요.

        SessionExpired = 4000,      //	세션이 종료 되었습니다.

        NotAllowDeleteMyself = 4300,//	자기 자신을 삭제 할 수 없습니다.
        NotAllowAddMyself = 4301,   //	자기 자신을 추가 할 수 없습니다.
        NotAllowAddAccount = 4302,  //	유저를 생성 할 수 없습니다.

        ExistFriend = 4500, //	이미 친구 입니다.
        ExistPending = 4501, //	이미 친구 요청 상태 입니다.
        ExistRequested = 4502, //	이미 친구 요청을 받았습니다.
        ExistMail = 4503, //	이미 존재하는 이메일 입니다.
        ExistAccount = 4504, //	이미 존재하는 계정 입니다.
        ExistUser = 4505, //	이미 유저가 존재 합니다.

        Invalid_Data = 5000, //	Server API Error
        NoUser = 5001, //	유저가 존재하지 않습니다.
        NoData = 5002, //	데이터가 존재 하지 않습니다.

        WrongPw = 6000, //	비밀번호가 틀렸습니다.
        Null_AppToken = 7000, //AppToken 값이 잘못되어 있습니다. 
        Fail_TempPw = 8000, //	임시 비밀번호 발급을 실패 했습니다. (Apex call fail)
        Fail_TempPw_Email = 8001, //	임시 비밀번호 이메일 전송을 실패 했습니다.
        
        
        
        // photon
        InvalidOperation = -2,
        InternalServerError = -1,
        InvalidAuthentication = 0x7FFF,
        GameIdAlreadyExists = 0x7FFF - 1,
        GameFull = 0x7FFF - 2,
        GameClosed = 0x7FFF - 3,
        AlreadyMatched = 0x7FFF - 4,
        ServerFull = 0x7FFF - 5,
        UserBlocked = 0x7FFF - 6,
        NoRandomMatchFound = 0x7FFF - 7,
        GameDoesNotExist = 0x7FFF - 9,
        MaxCcuReached = 0x7FFF - 10,
        InvalidRegion = 0x7FFF - 11,
        CustomAuthenticationFailed = 0x7FFF - 12,
        AuthenticationTicketExpired = 0x7FF1,
        PluginReportedError = 0x7FFF - 15,
        PluginMismatch = 0x7FFF - 16,
        JoinFailedPeerAlreadyJoined = 32750,
        JoinFailedFoundInactiveJoiner = 32749,
        JoinFailedWithRejoinerNotFound = 32748,
        JoinFailedFoundExcludedUserId = 32747,
        JoinFailedFoundActiveJoiner = 32746,
        HttpLimitReached = 32745,
        ExternalHttpCallFailed = 32744,
        SlotError = 32742,
        InvalidEncryptionParameters = 32741,
        
        // custom photon
        InvalidPassword,
        BlockedFromRoom,
        NotInTheLobby
    }
    
    class RoomOptionKey
    {
        public const string RoomName = "RoomName";
        public const string HostPlayerId = "HostPlayerId";
        public const string HostPlayerName = "HostPlayerName";
        public const string KeyPlayerId = "KeyPlayerId";
        public const string KeyPlayerName = "KeyPlayerName";
        public const string IsVisible = "IsVisible";
        public const string Password = "Password";
        public const string BlockedPlayerIdList = "BlockedPlayerIdList";
        public const string ExpectPlayerIdList = "ExpectPlayerIdList";
    }
    
    class PlayerKey
    {
        public const string UserId = "userId";
        public const string AllowInvite = "allowInvite";
    }
    
    class RelayServerKey
    {
        public const string Connect = "connect";
        public const string Disconnect = "disconnect";
    }

    
    public enum PhotonAction
    {
        Connect,
        Disconnect,
        CreateRoom,
        JoinRoom,
        LeaveRoom,
        JoinLobby,
        LeaveLobby,
        UpdateRoomList,
        ChangePassword,
        BlockPlayer,
        KickPlayer,
        UpdateRoomProperties,
        PlayerEnteredRoom,
        PlayerLeftRoom,
        Message
    }
    
    public enum DisconnectCause
    {
        /// <summary>No error was tracked.</summary>
        None,
        /// <summary>OnStatusChanged: The server is not available or the address is wrong. Make sure the port is provided and the server is up.</summary>
        ExceptionOnConnect,
        /// <summary>OnStatusChanged: Some internal exception caused the socket code to fail. This may happen if you attempt to connect locally but the server is not available. In doubt: Contact Exit Games.</summary>
        Exception,

        /// <summary>OnStatusChanged: The server disconnected this client due to timing out (missing acknowledgement from the client).</summary>
        ServerTimeout,

        /// <summary>OnStatusChanged: This client detected that the server's responses are not received in due time.</summary>
        ClientTimeout,

        /// <summary>OnStatusChanged: The server disconnected this client from within the room's logic (the C# code).</summary>
        DisconnectByServerLogic,
        /// <summary>OnStatusChanged: The server disconnected this client for unknown reasons.</summary>
        DisconnectByServerReasonUnknown,

        /// <summary>OnOperationResponse: Authenticate in the Photon Cloud with invalid AppId. Update your subscription or contact Exit Games.</summary>
        InvalidAuthentication,
        /// <summary>OnOperationResponse: Authenticate in the Photon Cloud with invalid client values or custom authentication setup in Cloud Dashboard.</summary>
        CustomAuthenticationFailed,
        /// <summary>The authentication ticket should provide access to any Photon Cloud server without doing another authentication-service call. However, the ticket expired.</summary>
        AuthenticationTicketExpired,
        /// <summary>OnOperationResponse: Authenticate (temporarily) failed when using a Photon Cloud subscription without CCU Burst. Update your subscription.</summary>
        MaxCcuReached,

        /// <summary>OnOperationResponse: Authenticate when the app's Photon Cloud subscription is locked to some (other) region(s). Update your subscription or master server address.</summary>
        InvalidRegion,

        /// <summary>OnOperationResponse: Operation that's (currently) not available for this client (not authorized usually). Only tracked for op Authenticate.</summary>
        OperationNotAllowedInCurrentState,
        /// <summary>OnStatusChanged: The client disconnected from within the logic (the C# code).</summary>
        DisconnectByClientLogic
    }


    public enum SendTarget
    {
        ToTarget,
        ToAll,
        ToOthers
    }

    public enum Gender
    {
        Female,
        Male,
    }
}
