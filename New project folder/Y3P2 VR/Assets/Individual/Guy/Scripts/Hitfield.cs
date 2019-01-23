using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitfield : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("initiate tag test");
        if(other.transform.tag == "EnemyWeapon")
        {
            Debug.LogWarning("Passed tag test");
            if(PlayerManager.thisPlayer.died == false) 
            PlayerManager.thisPlayer.SetDeath();
        }
    }
}
