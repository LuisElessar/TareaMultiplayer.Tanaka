using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int currentHealth;

    private Transform targetPlayer;

    public float speed = 10f;
    public float damage = 10f;
    public float attackRate = 1f;
    public float nextAttack = 0f;
    public float attackRange = 1.5f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FindPlayer();
            if (targetPlayer != null)
            {
                MovetoPlayer();
                TryAttackPlayer();
            }
        }
    }

    private void FindPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        Transform nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player.transform;
            }
        }
        targetPlayer = nearestPlayer;
        Debug.Log($"Atacar  a:{targetPlayer.name}");

    }
    private void MovetoPlayer()
    {
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void TryAttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);
        if (distanceToPlayer <= attackRange && Time.time >= nextAttack)
        {
            nextAttack = Time.time + attackRate; 
            photonView.RPC("AttackPlayer", RpcTarget.All, damage);
        }
    }

    [PunRPC] 
    private void AttackPlayer(float damage)
    {
        if (targetPlayer != null)
        {
            Player playerScript = targetPlayer.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage((int)damage);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
    }

    [PunRPC]
    private void Die()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
