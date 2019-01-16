using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyRanger : Enemy {

    [Header("Archer Settings:")]
    [SerializeField] private Transform _ArrowSpawnPos;
    [SerializeField] private GameObject _EnemyArrow;

    internal void ShootArrow()
    {
        photonView.RPC("ShootEnemyArrow", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void ShootEnemyArrow()
    {
        Quaternion _Rot = Quaternion.LookRotation(_ArrowSpawnPos.position - currentTarget.position);
        Rigidbody _NewArrow = PhotonNetwork.InstantiateSceneObject(_EnemyArrow.name, _ArrowSpawnPos.position, _Rot).GetComponent<Rigidbody>();
        _NewArrow.AddForce(Vector3.forward * 20, ForceMode.Impulse);
        _NewArrow.useGravity = true;

    }
}
