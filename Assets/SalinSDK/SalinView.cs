using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;

public class SalinView : PhotonView
{

}
#else
public class SalinView : MonoBehaviour
{
    public int ViewID;
}
#endif
