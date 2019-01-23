using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : MeleeWeapons {

    public int baseDamageMutliplier = 20;

    public override void OnTriggerEnter(Collider _O)
    {
        base.OnTriggerEnter(_O);

        if (_O.transform.tag == "Enemy")
        {
            Hit(_O.gameObject);
        }
    }

    private new void Update()
    {
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;
    }

    public void Hit(GameObject _O)
    {
        print(CalculateKinetics());

        if (CalculateKinetics() > 2)
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID, baseDamageMutliplier * (int)CalculateKinetics(), customVelocity, customAngularVelocity);
    }

}
