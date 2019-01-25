using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyCrab : Enemy {

    private static List<EnemyCrab> team = new List<EnemyCrab>();

    internal override void Start() {
        base.Start();
        team.Add(this); //Adds itself to the team;
        EnemyManager.enemyManager.SetEnemyRagdoll(gameObject.GetPhotonView().ViewID, false);
        EnemyManager.enemyManager.SetCrabAnim(gameObject.GetPhotonView().ViewID, "StartDance");
    }

    public override void GetDamaged(int _Hit, Vector3 _Velocity, Vector3 _Angular) {
        base.GetDamaged(_Hit, _Velocity, _Angular);

        foreach (EnemyCrab _CrabMate in team)
        {
            if (_CrabMate != null)
            {
                if (_CrabMate.started == false && _CrabMate != this)
                {
                    EnemyManager.enemyManager.SetCrabAnim(_CrabMate.gameObject.GetPhotonView().ViewID, "Walk");
                    _CrabMate.StartEnemy();
                    _CrabMate.started = true;
                }
            }
        }
    }
}
