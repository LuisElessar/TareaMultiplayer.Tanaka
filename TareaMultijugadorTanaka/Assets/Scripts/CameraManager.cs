using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraManager : MonoBehaviour
{
    public GameObject cameraPrefab;

    private void Start()
    {
        if (PhotonNetwork.IsConnected) 
        {
            if(PhotonNetwork.LocalPlayer  != null && cameraPrefab != null)
            {
                Instantiate(cameraPrefab);
            }
        }
    }
}
