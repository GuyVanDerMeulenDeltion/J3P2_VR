using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Shield>())
            transform.root.GetComponent<Animator>().SetTrigger("Recoil");
    }
}
