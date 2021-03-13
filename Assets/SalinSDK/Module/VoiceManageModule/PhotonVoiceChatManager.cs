//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using SalinSDK;


#if PHOTON_UNITY_NETWORKING
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Voice.Unity;
using Photon.Pun;
#endif

#if PHOTON_UNITY_NETWORKING
public class PhotonVoiceChatManager : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks,IVoiceManageable
{
    private VoiceConnection voiceConnection;
    private RoomOptions roomOption = new RoomOptions();
    private TypedLobby typedLobby = TypedLobby.Default;
    private Dictionary<string , Speaker> speakerDic = new Dictionary<string, Speaker>();
 
    [SerializeField]
    private bool autoTransmit = true;

    private string channelName;
    private string nickname;
    private string id;

    private bool isMute = false;

    private void Awake()
    {
        voiceConnection = this.GetComponent<VoiceConnection>();
        voiceConnection.SpeakerLinked += SpeakerLinkOn;
    }
    private void OnEnable()
    {
        this.voiceConnection.Client.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        this.voiceConnection.Client.RemoveCallbackTarget(this);
    }

    /// <summary>
    /// 유저 접속시 들어오는 구문 
    /// 스피커 프리팹이 생성될때 불립니다. 
    /// 입장한 유저의 닉네임과 아이디를 저장합니다.
    /// </summary>
    /// <param name="speaker"></param>
    void SpeakerLinkOn(Speaker speaker)
    {
        speaker.enabled = !isMute;
        speaker.OnRemoteVoiceRemoveAction += SpeakerLinkOff;

        object speakerNickName;
        object speakerID;
        speaker.Actor.CustomProperties.TryGetValue(SalinAPIKey.nickname, out speakerNickName);
        speaker.Actor.CustomProperties.TryGetValue(SalinAPIKey.userID, out speakerID);

        if(speakerDic.ContainsKey((string)speakerID) || (string)speakerID == id )
        {
            Debug.LogError("동일한 키(id)값이 이미 들어있습니다.");
            SalinCallbacks.OnUserConnectVoiceFail(SalinSDK.ErrorCode.Error);
            Destroy(speaker.gameObject);
        }
        else
        {
            speakerDic.Add((string)speakerID, speaker);
            SalinCallbacks.OnUserConnectVoice((string)speakerNickName, (string)speakerID);
        }
    }

    void SpeakerLinkOff(Speaker speaker)
    {
        string key = speakerDic.FirstOrDefault(x => x.Value == speaker).Key;
        speakerDic.Remove(key);
        Destroy(speaker.gameObject);
        SalinCallbacks.OnUserDisconnectVoice(key);
    }

#region VoiceManageable

    public void Connect(string _channelName, string _nickname, string _userID)
    {
        channelName = _channelName;
        voiceConnection.Client.LocalPlayer.SetCustomProperties(new Hashtable { { SalinAPIKey.nickname, _nickname } });
        voiceConnection.Client.LocalPlayer.SetCustomProperties(new Hashtable { { SalinAPIKey.userID, _userID } });

        nickname = _nickname;
        id = _userID;

        voiceConnection.ConnectUsingSettings(PhotonNetwork.PhotonServerSettings.AppSettings);
    }

    public void Disconnect()
    {
        this.voiceConnection.Client.Disconnect(Photon.Realtime.DisconnectCause.DisconnectByClientLogic);
    }

    //음소거
    public void SoundOffAll()
    {
        isMute = true;
        foreach (var item in speakerDic)
        {
            item.Value.GetComponent<AudioSource>().enabled = false;
        }
    }

    //음소거 취소
    public void SoundOnAll()
    {
        isMute = false;        
        foreach (var item in speakerDic)
        {
            item.Value.GetComponent<AudioSource>().enabled = true;
        }
    }

    public void SoundOff(string _userID)
    {
        if (speakerDic.ContainsKey(_userID))
            speakerDic[_userID].GetComponent<AudioSource>().enabled = false;
    }

    public void SoundOn(string _userID)
    {
        if(speakerDic.ContainsKey(_userID))
            speakerDic[_userID].GetComponent<AudioSource>().enabled = true;
    }

    public void MicOn()
    {
        voiceConnection.PrimaryRecorder.TransmitEnabled = true;
    }
    public void MicOff()
    {
        voiceConnection.PrimaryRecorder.TransmitEnabled = false;
    }

    public bool IsConnect()
    {
        if (voiceConnection == null) return false;

        return voiceConnection.Client.IsConnected;
    }

#endregion

#region MatchmakingCallbacks

    public void OnCreatedRoom()
    {
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnJoinedRoom()
    {
        if (this.autoTransmit)
        {
            if (voiceConnection.PrimaryRecorder == null)
            {
                voiceConnection.PrimaryRecorder = this.GetComponent<Recorder>();
            }
            voiceConnection.PrimaryRecorder.TransmitEnabled = autoTransmit;
            voiceConnection.PrimaryRecorder.Init(voiceConnection.VoiceClient);
        }
        SalinCallbacks.OnConnectVoice();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        SalinCallbacks.OnConnectVoiceFail(SalinSDK.ErrorCode.Error);
    }

    public void OnLeftRoom()
    {

    }

#endregion

#region ConnectionCallbacks

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        this.voiceConnection.Client.OpJoinOrCreateRoom(new EnterRoomParams { RoomName = channelName, RoomOptions = roomOption, Lobby = typedLobby });
    }


    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {

    }

    public void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        SalinCallbacks.OnDisconnectVoice();
        Destroy(this.gameObject);
        //Debug.LogErrorFormat("OnDisconnected cause={0}", cause);
        //if (cause == Photon.Realtime.DisconnectCause.None || cause == Photon.Realtime.DisconnectCause.DisconnectByClientLogic)
        //{
        //    return;
        //}
    }
#endregion
}
#else
#region FAKE CLASS PhotonVoiceChatManager
public class PhotonVoiceChatManager : MonoBehaviour, IVoiceManageable
{
    void LogError()
    {
        Debug.LogError("Not import Photon Voice Server, Can't use PhotonVoiceChatManager");
    }

    public void Connect(string _channelName, string _nickName, string _userID) { LogError(); }
    public void Disconnect()                { LogError(); }
    public void MicOff()                    { LogError(); }
    public void MicOn()                     { LogError(); }
    public void SoundOnAll()                { LogError(); }
    public void SoundOffAll()               { LogError(); }
    public void SoundOn(string nickName)    { LogError(); }
    public void SoundOff(string nickName)   { LogError(); }
    public bool IsConnect()                 { LogError(); return false;}
}
#endregion
#endif