using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : MeleeWeapons
{

    public int baseDamageMutliplier = 20;
    public int divide = 1;

    public override void OnTriggerEnter(Collider _O)
    {
        base.OnTriggerEnter(_O);

        if (_O.transform.tag == "Enemy")
        {
            Hit(_O.gameObject);
        }
    }

    protected override void Update()
    {
        base.Update();
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;
    }

    public void Hit(GameObject _O)
    {
        print(CalculateKinetics());

        if (CalculateKinetics() > 1.5f)
        {
            Haptic();
            EnemyManager.enemyManager.SetEnemyTotalHit(_O.GetComponent<PhotonView>().ViewID, Mathf.Clamp(baseDamageMutliplier * (int)CalculateKinetics() / divide, 0, 400), customVelocity / 2, customAngularVelocity / 2);
        }
    }
}
