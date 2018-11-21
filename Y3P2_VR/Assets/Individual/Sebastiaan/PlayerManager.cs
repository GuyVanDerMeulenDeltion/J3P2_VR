using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private static int playerSpawnTimer = 1;

    [Header("Player Settings:")]
    public List<GameObject> currentPlayers = new List<GameObject>();

    public Transform spawnpoint;

    private void Start() {
        InstanceNetworkPlayer();
    }

    private void InstanceNetworkPlayer() {
            GameObject _NewPlayer = PhotonNetwork.Instantiate("Player", spawnpoint.position, Quaternion.identity);
            currentPlayers.Add(_NewPlayer);
    }
}
