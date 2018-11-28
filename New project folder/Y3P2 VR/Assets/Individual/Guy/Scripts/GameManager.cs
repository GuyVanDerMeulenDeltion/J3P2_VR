using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks {
    public static GameManager gameManager;

    [SerializeField] private Transform[] _SpawnPoint;
    public static float _MAXPLAYERHEALTH = 250;
    public static float _PLAYERHEALTH = _MAXPLAYERHEALTH;

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;
    }

    public void Start() {
        if (PlayerManager.thisPlayer == null && PhotonNetwork.IsConnected) {
                PhotonNetwork.Instantiate("[CameraRig]", _SpawnPoint[0].position, Quaternion.identity);
        } else if(PhotonNetwork.IsConnected == false) {
                Instantiate(Resources.Load("[CameraRig]"), _SpawnPoint[0].position, Quaternion.identity);
        }
    }
}

