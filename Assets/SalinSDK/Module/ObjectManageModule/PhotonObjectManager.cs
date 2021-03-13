namespace SalinSDK
{
#if PHOTON_UNITY_NETWORKING
    using Photon.Pun;
    using UnityEngine;
    
    public class PhotonObjectManager : IObjectManageable
    {
        public NetworkObject CreateInstance(string userToken, string prefName)
        {
            NetworkObject netObj = null;
            GameObject obj = PhotonNetwork.Instantiate(prefName, Vector3.zero, Quaternion.identity);
            
            if (obj != null)
            {
                PhotonView pv = obj.GetComponent<PhotonView>();
                netObj = obj.AddComponent<NetworkObject>();

                string userId;
                if (pv.Owner.CustomProperties.ContainsKey(PlayerKey.UserId) == true) 
                    userId = pv.Owner.CustomProperties[PlayerKey.UserId].ToString();
                else
                    userId = UserManager.Instance.userID;
                
                netObj.Init(pv.ViewID, userId, obj);
            }

            return netObj;
        }

        public NetworkObject GetInstance(string userToken, int netId)
        {
            PhotonView pv = PhotonView.Find(netId);

            NetworkObject netObj = pv.GetComponent<NetworkObject>();

            if (netObj == null)
            {
                netObj = pv.gameObject.AddComponent<NetworkObject>();
                
                string userId;
                if (pv.Owner.CustomProperties.ContainsKey(PlayerKey.UserId) == true) 
                    userId = pv.Owner.CustomProperties[PlayerKey.UserId].ToString();
                else
                    userId = UserManager.Instance.userID;
                
                netObj.Init(pv.ViewID, userId, pv.gameObject);
            }

            return netObj;
        }
    }    
#else
    #region Fake class
    public class PhotonObjectManager : IObjectManageable
    {
        public NetworkObject CreateInstance(string userToken, string prefName) { return null; }
        public NetworkObject GetInstance(string userToken, int netId) { return null; }
    }
    #endregion
#endif
}

