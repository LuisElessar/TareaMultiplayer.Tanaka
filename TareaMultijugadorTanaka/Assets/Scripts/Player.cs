using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviourPun
{
    private static GameObject localInstance;

    private Rigidbody rb;
    [SerializeField] private float speed;

    public static GameObject LocalInstance { get { return localInstance; } }

    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private Material Jugador1;
    [SerializeField] private Material Jugador2;

    [SerializeField] private GameObject bulletPrefab;

    private MeshRenderer meshRenderer;

    private float fireRate = 0.5f;
    private float nextFireTime = 0f;
    
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if(photonView.IsMine)
        {
            localInstance = gameObject;
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);
            meshRenderer.material = Jugador1;
        }
        else
        {
            meshRenderer.material = Jugador2;
        }

        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
    }
    [PunRPC]
    public void SetName(string playerName)
    {
        playerNameText.text = playerName;
    }

    private void Update()
    {
        if(!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        Move();
        Shoot();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, vertical * speed);

        if (horizontal != 0 || vertical != 0)
        {
            transform.forward = new Vector3(horizontal, 0, vertical);
        }
    }

    private void Shoot()
    {
        if(Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
            obj.GetComponent<Bullet>().SetUp(transform.forward, photonView.ViewID);
        }
    }
}
