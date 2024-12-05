using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int currentHealth;

    private Transform player;

    public float speed = 10f;
    public float damage = 10f;
    public float attackRate = 1f;
    public float nextAttack = 0f;
    public float attackRange = 1.5f;

    private void Start()
    {
        currentHealth = maxHealth;


        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null)
        {
            player = Player.LocalInstance.transform;
        }
    }

    private void Update()
    {
        if (player != null)
        {

            MovetoPlayer();
            TryAttackPlayer();
        }
    }

    private void MovetoPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void TryAttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && Time.time >= nextAttack)
        {
            nextAttack = Time.time + attackRate; 
            photonView.RPC("AttackPlayer", RpcTarget.All, damage);
        }
    }

    [PunRPC] 
    private void AttackPlayer(float damage)
    {
        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage((int)damage);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"Enemigo recibió {damage} de daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("El enemigo ha muerto.");

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
