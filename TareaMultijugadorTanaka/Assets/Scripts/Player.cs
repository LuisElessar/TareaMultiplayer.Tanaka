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
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material lightblueMaterial;

    private MeshRenderer meshRenderer;
    
    private void Awake()
    {
        if(photonView.IsMine)
        {
            localInstance = gameObject;
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);
            meshRenderer.material = blueMaterial;
        }
        else
        {
            meshRenderer.material = lightblueMaterial;
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, vertical * speed);
       
        if(horizontal!=0 || vertical != 0)
        {
            transform.forward = new Vector3(horizontal, 0, vertical);
        }
    }
}
