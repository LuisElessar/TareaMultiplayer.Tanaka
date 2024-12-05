using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    [SerializeField] private GameObject enemyPrefab; 
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private float timeBetweenWaves = 10f; 

    private int waveNumber = 1; 

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        while (true)
        {
            for (int i = 0; i < waveNumber; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f); 
            }
            waveNumber++;
            yield return new WaitForSeconds(timeBetweenWaves); 
        }
    }

    private void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPosition = spawnPoints[spawnIndex].position;

        PhotonNetwork.Instantiate(enemyPrefab.name, spawnPosition, Quaternion.identity);
    }
}


