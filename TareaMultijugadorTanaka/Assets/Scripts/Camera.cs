using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviourPun
{
    public Transform player; 
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;

    private void Start()
    {

        if (photonView.IsMine)
        {
            player = transform.parent;
            transform.parent = null;
        }
        else
        {
            gameObject.SetActive(true);
        }
  
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            
            transform.position = smoothedPosition;

          
            transform.LookAt(player);
        }
    }
}
