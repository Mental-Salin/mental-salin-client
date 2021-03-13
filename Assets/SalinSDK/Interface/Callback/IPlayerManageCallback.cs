namespace SalinSDK
{
    public interface IPlayerManageCallback
    {
        void OnUserBlock();
        void OnUserBlockFail(ErrorCode errorCode);
        void OnUserKick(Player kickedPlayer);
        void OnUserKickFail(ErrorCode errorCode);
    }
}