using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shield : MeleeWeapons {

    private void OnTriggerEnter(Collider _O) {
        if (_O.transform.tag == "Enemy") {
            Hit(_O.gameObject);
        }
    }

    public void Hit(GameObject _O) {
        print(CalculateKinetics());

        if(CalculateKinetics() > 3)
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID,(int)CalculateKinetics(), customVelocity * (CalculateKinetics()*8), customAngularVelocity);
    }
}