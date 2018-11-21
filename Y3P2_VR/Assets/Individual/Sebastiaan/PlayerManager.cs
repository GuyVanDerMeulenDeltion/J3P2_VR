using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{

    public Transform spawnpoint;

    private void Start() {
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        GameObject _NewPlayer = PhotonNetwork.Instantiate("Player", spawnpoint.position, Quaternion.identity);
    }
}
