using UnityEngine;

namespace SalinSDK
{
    public class NetworkObject : MonoBehaviour
    {
        private int _netId;
        public int NetId { get; private set; }
        private string _ownerId;
        public string OwnerId { get; private set; }
        private GameObject _obj;
        public GameObject Obj { get; private set; }

        private bool _isMine = false;
        public bool IsMine { get; private set; }

        public void Init(int netId, string ownerId, GameObject obj)
        {
            this.NetId = netId;
            this.OwnerId = ownerId;
            this.Obj = obj;

            IsMine = OwnerId.Equals(UserManager.Instance.userID);
        }
    }
}