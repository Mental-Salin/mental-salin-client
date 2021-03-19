using System;
using System.Runtime.CompilerServices;
using SalinSDK;

namespace SalinSDK
{
    [Serializable]
    public class RoomStartInterviewMessage : MessageData
    {
        public RoomStartInterviewMessage() : base("RoomStartInterviewMessage")
        {
        }
    }
}