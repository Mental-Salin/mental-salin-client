using UnityEngine;

namespace SalinSDK
{
    public interface IObjectManageable
    {
        NetworkObject CreateInstance(string userToken, string prefName);
        NetworkObject GetInstance(string userToken, int netId);
    }
}