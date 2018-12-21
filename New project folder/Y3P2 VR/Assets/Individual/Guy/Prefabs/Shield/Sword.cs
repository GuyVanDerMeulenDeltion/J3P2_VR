using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : MeleeWeapons {

    public int baseDamageMutliplier = 20;

    private void OnTriggerEnter(Collider _O)
    {
        if (_O.transform.tag == "Enemy")
        {
            Hit(_O.gameObject);
        }
    }

    public void Hit(GameObject _O)
    {
        print(CalculateKinetics());

        if (CalculateKinetics() > 2)
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID, baseDamageMutliplier * (int)CalculateKinetics(), customVelocity, customAngularVelocity);
    }

}
