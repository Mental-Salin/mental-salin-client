using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SalinSDK
{
    public class DemoObejctSyncData : MessageData
    {
        public DemoObejctSyncData() : base("DemoObejctSyncData")
        {

        }

        public Vector3 position;
        public int objViewID;
    }
}

