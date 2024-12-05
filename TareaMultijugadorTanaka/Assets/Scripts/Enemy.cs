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
            
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
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
