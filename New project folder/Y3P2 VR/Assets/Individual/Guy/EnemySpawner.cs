using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPunCallbacks {
    //ONLY LOCAL!
    public float spawnInterval = 6;
    public bool shouldSpawnOnStart = false;

    public Transform spawnPoint;

    private void Start()
    {
        if (shouldSpawnOnStart)
            StartCoroutine(SpawnEnemyTimer());
    }

    private void SpawnEnemy() {
     
            GameObject _Enemy = PhotonNetwork.Instantiate("Crusader", spawnPoint.position, Quaternion.identity);
            _Enemy.GetComponent<Enemy>().StartEnemy();
    }

     private IEnumerator SpawnEnemyTimer( )
     {
        yield return new WaitForSeconds(spawnInterval);
        SpawnEnemy();
        StartCoroutine(SpawnEnemyTimer());
    }
}
