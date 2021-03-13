using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SalinSDK
{
    public abstract class MessageData
    {
        protected MessageData(string _action)
        {
            this.action = _action;
        }

        public string action;
        public string senderId;
        public string targetrId;
    }
}

