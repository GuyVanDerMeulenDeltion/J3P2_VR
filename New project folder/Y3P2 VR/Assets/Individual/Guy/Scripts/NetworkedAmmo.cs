using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedAmmo : MonoBehaviourPunCallbacks {

    public int baseDamage = 25;

    public bool canDestroy = true;
    public bool canHit = false;
    public bool calculateKinetics = true;

    private void OnTriggerEnter(Collider _O) {
        if (_O.transform.tag == "Enemy") {
            Hit(_O.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.tag == "Enemy")
        {
            PhotonNetwork.Destroy(gameObject);
            Hit(collision.transform.root.gameObject);
        }
            if(canDestroy == true)
            PhotonNetwork.Destroy(gameObject);
    }

    public void Hit(GameObject _O) {
        if (CheckKinetics() < 10) return;

        if (canHit == true && gameObject.GetPhotonView().IsMine) {
            canHit = false;
            int _Damage = !calculateKinetics ? baseDamage : (int)(Mathf.Pow(GetComponent<Rigidbody>().velocity.magnitude, 2) * 0.5f);
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID, _Damage, GetComponent<Rigidbody>().velocity, GetComponent<Rigidbody>().angularVelocity);
        }
    }

    private float CheckKinetics() {
        return Mathf.Pow(GetComponent<Rigidbody>().velocity.magnitude, 2) * 0.5f;
    }

    private void Check()
    {
        if(PhotonNetwork.IsMasterClient == false)
        {
            Destroy(GetComponent<Rigidbody>());
        }
    }
} 


