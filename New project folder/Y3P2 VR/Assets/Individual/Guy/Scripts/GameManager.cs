using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks {
    public static GameManager gameManager;

    public static bool test = false;
    [SerializeField] private Transform[] _SpawnPoint;
    public static float _MAXPLAYERHEALTH = 250;
    public static float _PLAYERHEALTH = _MAXPLAYERHEALTH;

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;
    }

    public void Start() {
        if (test == true) return;

        if (PlayerManager.thisPlayer == null && PhotonNetwork.IsConnected) {
                PhotonNetwork.Instantiate("[CameraRig]", _SpawnPoint[0].position, Quaternion.identity);
                photonView.RPC("SetSpawnParticles", RpcTarget.All, _SpawnPoint[0]);
        } else if(PhotonNetwork.IsConnected == false) {
                Instantiate(Resources.Load("[CameraRig]"), _SpawnPoint[0].position, Quaternion.identity);
                SetSpawnParticles(_SpawnPoint[0]);
        }
    }

    [PunRPC]
    private void SetSpawnParticles(Transform _Spawn) {
        Instantiate(Resources.Load("Spawn_Effect"), _Spawn.position, Quaternion.identity);
    }
}

