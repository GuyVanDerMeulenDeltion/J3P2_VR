using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {

    private bool canHitAgain = true;
    private GameObject shield;

    private void OnEnable()
    {
        canHitAgain = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Shield>() && canHitAgain == true)
        {
            canHitAgain = false;
            shield = other.gameObject;
            transform.root.GetComponent<Animator>().SetTrigger("Recoil");
        }
    }
}
