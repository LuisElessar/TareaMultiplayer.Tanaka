using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    private int ownerId;
    private Rigidbody rb;
    private Vector3 direction;

    [SerializeField] private float speed;
    [SerializeField] private float lifeTime = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        DestroyBulletAfterTime();
    }

    private void DestroyBulletAfterTime()
    {
        if(photonView.IsMine)
        {
            Invoke(nameof(DestroySelf), lifeTime);
        }
    }

    private void DestroySelf()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    public void SetUp(Vector3 direction, int ownerId)
    {
        this.direction = direction;
        this.ownerId = ownerId;
    }

     void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        rb.velocity = direction.normalized * speed;
    }
}
