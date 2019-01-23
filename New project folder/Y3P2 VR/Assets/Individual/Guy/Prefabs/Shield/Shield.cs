using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shield : MeleeWeapons {

    public override void OnTriggerEnter(Collider _O) {
        if (_O.transform.tag == "Enemy") {
            Hit(_O.gameObject);
        }
    }
    private new void Update()
    {
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;
    }

    public void Hit(GameObject _O) {

        if(CalculateKinetics() > 3)
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID,(int)CalculateKinetics(), customVelocity * (CalculateKinetics()*8), customAngularVelocity);
    }
}