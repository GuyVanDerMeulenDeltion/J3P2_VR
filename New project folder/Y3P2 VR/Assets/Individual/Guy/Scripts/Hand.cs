using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hand : MonoBehaviourPunCallbacks {

    public void OnTriggerEnter(Collider _O) {
        if(_O.transform.tag == "Enemy") {
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID, 25, GetComponent<Rigidbody>().velocity * 10);
        }
    }
}
