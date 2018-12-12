using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shield : MonoBehaviourPunCallbacks {

    private Vector3 oldPos;
    private Vector3 newPos;

    private Vector3 customVelocity;

    private void OnTriggerEnter(Collider _O) {
        if (_O.transform.tag == "Enemy") {
            Hit(_O.gameObject);
        }
    }

    private void Update() {
        newPos = transform.position;
        customVelocity = (oldPos - newPos) / Time.deltaTime;
        oldPos = newPos;
    }

    public void Hit(GameObject _O) {
        print(CalculateKinetics());

        if(CalculateKinetics() > 3)
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID, (int)CalculateKinetics(), -transform.up *( CalculateKinetics() * 8));
    }

    private float CalculateKinetics() {
        return Mathf.Pow(customVelocity.magnitude, 2) * 0.5f;
    }
}