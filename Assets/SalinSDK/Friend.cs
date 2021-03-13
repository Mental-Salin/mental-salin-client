using UnityEngine;
using LitJson;
using SalinSDK.ExtensionMethod;
using System.Collections.Generic;
namespace SalinSDK
{
    public enum FriendStatus
    {
        Completed,
        Requested,
        Pending,
        None
    }

    public class Friend
    {
        public string userID;
        public string userAccount;
        public string userNickName;
        public string processID;
        public FriendStatus status;

        public Friend()
        {
        }

        public Friend(string _userID, string _userAccount, string _userNickName, string _processID, FriendStatus _status)
        {
            userID = _userID;
            userAccount = _userAccount;
            userNickName = _userNickName;
            processID = _processID;
            status = _status;
        }

        public static Friend Convert(JsonData jsonData)
        {
            string userID = string.Empty;
            if (jsonData.JsonDataContainsKey(JsonKey.friendID))
            {
                userID = jsonData[JsonKey.friendID].ToString();
            }
            string userAccount = string.Empty;
            if (jsonData.JsonDataContainsKey(JsonKey.account))
            {
                userAccount = jsonData[JsonKey.account].ToString();
            }

            string userNickName = string.Empty;
            if (jsonData.JsonDataContainsKey(JsonKey.nickname))
            {
                userNickName = jsonData[JsonKey.nickname].ToString();
            }
            string processID = string.Empty;
            if (jsonData.JsonDataContainsKey(JsonKey.processID))
            {
                processID = jsonData[JsonKey.processID].ToString();
            }

            FriendStatus status;
            if (jsonData.JsonDataContainsKey(JsonKey.friendStatus))
            {
                switch (jsonData[JsonKey.friendStatus].ToString())
                {
                    case "completed": status = FriendStatus.Completed; break;
                    case "requested": status = FriendStatus.Requested; break;
                    case "pending": status = FriendStatus.Pending; break;
                    default:
                        status = FriendStatus.None;
                        Debug.LogError(jsonData[JsonKey.friendStatus].ToString() + "Status를 찾을 수 없음"); break;
                }
            }
            else
            {
                status = FriendStatus.None;
            }

            return new Friend(userID, userAccount, userNickName, processID, status);
        }

        public static List<Friend> ConvertToList(JsonData jsonData)
        {
                List<Friend> friendList = new List<Friend>();
                for (int idx = 0; idx < jsonData.Count; ++idx)
                {
                    JsonData tmp = jsonData[idx];
                    Friend friend = Friend.Convert(tmp);

                    if (friend.userAccount != UserManager.Instance.userInfo.userAccount)
                    {
                        friendList.Add(friend);
                    }
                }
                if (friendList.Count == 0)
                    return null;

                return friendList;
        }
    }
}
