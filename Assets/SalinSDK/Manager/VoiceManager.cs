using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if PHOTON_UNITY_NETWORKING
using Photon.Voice.Unity;
using Photon.Pun;
#endif

namespace SalinSDK
{
    public static class VoiceManager
    {
        static string channel;
        static string nickName;
        static string userID;
        static IVoiceManageable _voiceManageable;
        static IVoiceManageable voiceManageable
        {
            get
            {
                if(_voiceManageable == null)
                {
                   return null;
                }
                return _voiceManageable;
            }
        }

        static public void Connect(string channelName)
        {
            Connect(channelName, UserManager.Instance.userInfo.userNickname, UserManager.Instance.userID);
        }
        static public void Connect(string channelName, string _nickName)
        {
            Connect(channelName, _nickName, UserManager.Instance.userID);
        }
        /// <summary>
        /// 음성대화 서버에 접속하기 위한 함수입니다. 
        /// VoiceManager의 다른 기능들은 Connect이후 사용이 가능합니다.
        /// 포톤Voice 사용시 VoiceController 프리팹을 생성하고 포톤 서버와 연결합니다.
        /// </summary>
        /// <param name="channelName">채널명</param>
        /// <param name="_nickName">닉네임</param>
        /// <param name="_userID">고유 유저 아이디</param>
        static public void Connect(string channelName, string _nickName, string _userID)
        {
            if(_voiceManageable == null)
            {
                GameObject voiceManagePrefab = Resources.Load("VoiceController") as GameObject;
                GameObject manage = Object.Instantiate(voiceManagePrefab);
                manage.name = "VoiceController";
                Object.DontDestroyOnLoad(manage);
                _voiceManageable = manage.GetComponent<IVoiceManageable>();
            }
            channel = channelName;
            nickName = _nickName;
            userID = _userID;
            voiceManageable.Connect(channelName, _nickName,_userID);
        }
        /// <summary>
        /// 재 연결을 위한 함수입니다.
        /// 이전 Connect때 사용했던 정보를 기억하고 접속 요청을 합니다. 
        /// </summary>
        static public void ReConnect()
        {
            Connect(channel, nickName, userID);
        }
        /// <summary>
        /// 서버와의 연결을 끊습니다. 
        /// </summary>
        static public void Disconnect()
        {
          voiceManageable.Disconnect();
          _voiceManageable = null;
        }

        /// <summary>
        /// 마이크를 켭니다.
        /// </summary>
        static public void MicOn()
        {
           voiceManageable.MicOn();
        }

        /// <summary>
        /// 마이크를 끕니다.
        /// </summary>
        static public void MicOff()
        {
           voiceManageable.MicOff();
        }

        /// <summary>
        /// 모든 소리를 켭니다.
        /// </summary>
        static public void SoundOnAll()
        {
           voiceManageable.SoundOnAll();
        }

        /// <summary>
        /// 모든 소리를 끕니다.
        /// </summary>
        static public void SoundOffAll()
        {
           voiceManageable.SoundOffAll();
        }

        /// <summary>
        /// 특정 사용자의 소리를 켭니다.
        /// </summary>
        /// <param name="_userID"></param>
        static public void SoundOn(string _userID)
        {
            voiceManageable.SoundOn(_userID);
        }

        /// <summary>
        /// 특정 사용자의 소리를 끕니다.
        /// </summary>
        /// <param name="_userID"></param>
        static public void SoundOff(string _userID)
        {
            voiceManageable.SoundOff(_userID);
        }

        /// <summary>
        /// 서버와 연결되어있는지 확인하는 함수입니다. 
        /// </summary>
        /// <returns>연결되어 있으면 True 아니면 False를 반환합니다.</returns>
        static public bool IsConnect()
        {
            if (voiceManageable != null)
            {
                return voiceManageable.IsConnect();
            }
            else
            {
                return false;
            };
        }
    }

#if PHOTON_UNITY_NETWORKING && UNITY_EDITOR
    /// <summary>
    /// 포톤 스피커 프리팹 생성 코드 입니다. 
    /// 스피커 프리팹이 존재하지 않을 경우 자동으로 생성합니다 
    /// 추후 옵션을 통해 자동생성을 원하지 생성안되게 수정 할 생각입니다. 
    /// </summary>
    [InitializeOnLoad]
    public class VoicePrefabCreator : Editor
    {
        static GameObject voiceController;
        static GameObject speaker;
        const string ResourcesFolderPath = "Assets/Resources/";

        static VoicePrefabCreator()
        {
            EditorApplication.projectChanged += OnProjectChanged;
        }

        private static void OnProjectChanged()
        {
            if (speaker == null)
            {
                speaker = (GameObject)Resources.Load("Speaker");
            }

            if (speaker == null)
            {
                GameObject speaker = new GameObject();
                speaker.AddComponent<AudioSource>();
                speaker.AddComponent<Speaker>();
                PrefabUtility.SaveAsPrefabAsset(speaker, ResourcesFolderPath + "Speaker.prefab");
                Object.DestroyImmediate(speaker);
            }

            if (voiceController == null)
            {
                voiceController = (GameObject)Resources.Load("VoiceController");
            }

            if(voiceController == null)
            {
                GameObject voiceController = new GameObject();
                VoiceConnection voiceConnection =  voiceController.AddComponent<VoiceConnection>();
                Recorder recorder = voiceController.AddComponent<Recorder>();
                PhotonVoiceChatManager photonVoiceChatManager = voiceController.AddComponent<PhotonVoiceChatManager>();
                recorder.TransmitEnabled = true;

                voiceConnection.PrimaryRecorder = recorder;
                voiceConnection.SpeakerPrefab = speaker;

                string AppIdVoice = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;
                PrefabUtility.SaveAsPrefabAsset(voiceController, ResourcesFolderPath + "VoiceController.prefab");
                Object.DestroyImmediate(voiceController);
            }
        }
    }
#endif
}