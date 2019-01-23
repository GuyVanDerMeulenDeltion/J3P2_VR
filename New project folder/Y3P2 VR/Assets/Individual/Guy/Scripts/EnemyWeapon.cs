using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {

    private bool canHitAgain = true;
    private GameObject shield;


    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Shield>() && canHitAgain == true)
        {
            shield = other.gameObject;
            transform.root.GetComponent<Animator>().SetTrigger("Recoil");
        }
    }
}
