using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyRanger : Enemy {

    public bool isCowboy = false;

    [Header("Archer Settings:")]
    [SerializeField] private Transform _ArrowSpawnPos;
    [SerializeField] private GameObject _EnemyArrow;

    private Enemy root;

    internal override void Start()
    {
        base.Start();
        root = transform.root.GetComponent<Enemy>();
    }

    internal void ShootArrow()
    {
        photonView.RPC("ShootEnemyArrow", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void ShootEnemyArrow()
    {
        Quaternion _Rot = Quaternion.LookRotation(_ArrowSpawnPos.position - currentTarget.position);
        Rigidbody _NewArrow = PhotonNetwork.InstantiateSceneObject(_EnemyArrow.name, _ArrowSpawnPos.position, _Rot).GetComponent<Rigidbody>();
        _NewArrow.GetComponent<Rigidbody>().AddForce(-_NewArrow.transform.forward * 3, ForceMode.Impulse);
        _NewArrow.useGravity = false;

    }
}
