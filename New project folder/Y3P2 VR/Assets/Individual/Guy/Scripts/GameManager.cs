using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks {
    public static GameManager gameManager;

    public bool test = false;
    [SerializeField] private Transform[] _SpawnPoint;
    public static float _MAXPLAYERHEALTH = 250;
    public static float _PLAYERHEALTH = _MAXPLAYERHEALTH;

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;
    }

    public void Start() {
        if (test == true) {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Instantiate("TestPlayer", _SpawnPoint[0].position, Quaternion.identity);
                return;
        }

        if (PlayerManager.thisPlayer == null && PhotonNetwork.IsConnected) {
                PhotonNetwork.Instantiate("[CameraRig]", _SpawnPoint[0].position, Quaternion.identity);
            print(_SpawnPoint[0]);
                photonView.RPC("SetSpawn", RpcTarget.All, _SpawnPoint[0].position);
                photonView.RPC("SendOnJoinedMessage", RpcTarget.All, "All welcome the new player!");
                SendOnJoinedMessage("Welcome to the game");
        } else if(PhotonNetwork.IsConnected == false) {
                Instantiate(Resources.Load("[CameraRig]"), _SpawnPoint[0].position, Quaternion.identity);
                SetSpawn(_SpawnPoint[0].position);
                SendOnJoinedMessage("Welcome to the game");
        }
    }

    [PunRPC]
    private void SetSpawn(Vector3 _Spawn) {
        Instantiate(Resources.Load("Spawn_Effect"), _Spawn, Quaternion.identity);
    }

    [PunRPC]
    private void SendOnJoinedMessage(string _Message) {
        PlayerManager.thisPlayer.playerMain.SendMessageLocally(_Message);
    }
}

