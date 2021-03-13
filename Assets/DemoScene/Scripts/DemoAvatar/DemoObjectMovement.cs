using SalinSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System;

public class DemoObjectMovement : SalinCallbacks
{
    float speed = 3f;

    private float sendFrequencePerSec = 2f;
    private float sendFrequence;
    private float currentTime;

    private Transform ObjecTransfrom;

    SalinView slview;

    [NonSerialized]
    public bool IsMine = false;

    #region 보간 변수
    public float Lerpspeed = 50F;
    private float startTime;
    private float journeyLength;
    private Vector3 startMarker = new Vector3();
    private Vector3 endMarker = new Vector3();

    private Vector3 Curpos = new Vector3();

    #endregion

    private void Awake()
    {
        currentTime = 0;
        sendFrequence = 1 / sendFrequencePerSec;

        ObjecTransfrom = gameObject.GetComponent<Transform>();
        slview = gameObject.GetComponent<SalinView>();

        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (IsMine)
        {
#if UNITY_EDITOR
            Curpos = transform.position + (new Vector3(Input.GetAxisRaw("Horizontal") * speed, 0, Input.GetAxisRaw("Vertical") * speed) * Time.deltaTime);
            transform.position = IsXposExceedWall(Curpos);

#endif

#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    //터치 처리

                    Curpos = transform.position + new Vector3(Input.GetTouch(0).deltaPosition.x * Time.deltaTime * speed, 0, Input.GetTouch(0).deltaPosition.y * Time.deltaTime * speed);
                    transform.position = IsXposExceedWall(Curpos);
                }
            }
#endif

            currentTime += Time.deltaTime;

            if (sendFrequence < currentTime)
            {
                SendMessage();
                currentTime = 0;
            }
        }
        else
        {
            float distCovered = (Time.time - startTime) * Lerpspeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
        }
    }


    public void SendMessage()
    {
        DemoObejctSyncData avatarTransfrom = new DemoObejctSyncData
        {
            position = transform.position,
            objViewID = slview.ViewID
        };

        XRSocialSDK.SendBroadcastMessage(avatarTransfrom);
    }


    //메세지 받는 부분 
    public override void OnReceiveMessage<T>(T message)
    {
        if (message is DemoObejctSyncData)
        {
            DemoObejctSyncData at = message as DemoObejctSyncData;

            if (at.objViewID != slview.ViewID)
                return;

            startMarker = transform.position;
            endMarker = at.position;

            startTime = Time.time;
            journeyLength = Vector3.Distance(transform.position, at.position);
        }
    }

    private Vector3 IsXposExceedWall(Vector3 currentpos)
    {
        Vector3 changepos = currentpos;

        if (currentpos.x > 45 || currentpos.x < -45)
            changepos.x = currentpos.x > 45 ? 45 : -45;

        if (currentpos.z > 45 || currentpos.z < -45)
            changepos.z = currentpos.z > 45 ? 45 : -45;

        return changepos;

    }

}