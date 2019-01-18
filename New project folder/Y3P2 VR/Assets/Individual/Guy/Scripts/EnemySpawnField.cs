using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawnField : MonoBehaviourPunCallbacks {

    [SerializeField] private int enemyChunkID;

    private bool hasSpawned = false;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider _O)
    {
        if(_O.transform.tag == "Player" && hasSpawned == false)
        {
            print(_O.gameObject);
            hasSpawned = true;
            SpawnManager.spawnManager.SpawnEntites(enemyChunkID);
        }
    }
}
