using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SalinSDK
{
    public interface IVoiceManageable
    {
        void Connect(string _channelName, string _nickName, string _userID);

        void Disconnect();

        void SoundOnAll();
        void SoundOffAll();

        void SoundOn(string _userID);
        void SoundOff(string _userID);

        void MicOn();
        void MicOff();

        bool IsConnect();
    }
}
