using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks {
    public static GameManager gameManager;

    public bool test = false;

    public GameObject teleportManagerInstance;

    [SerializeField] private Transform[] _SpawnPoint;
    public static float _MAXPLAYERHEALTH = 250;
    public static float _PLAYERHEALTH = _MAXPLAYERHEALTH;

    private int spawnIndex = 0;

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;
    }

    public void Start() {
        print(PhotonNetwork.IsConnected +" is the current state of photon network;");
        print(PhotonNetwork.CurrentRoom.Name+ " is the current room name");

        if (PlayerManager.thisPlayer == null && PhotonNetwork.IsConnected) {
                PhotonNetwork.Instantiate("[CameraRig]", _SpawnPoint[spawnIndex].position, Quaternion.identity);
                photonView.RPC("SetSpawn", RpcTarget.All, _SpawnPoint[spawnIndex].position);
                photonView.RPC("SendOnJoinedMessage", RpcTarget.All, "All welcome the new player!");
                SendOnJoinedMessage("Welcome to the game");

        } /*else if(PhotonNetwork.IsConnected == false) {
                Instantiate(Resources.Load("[CameraRig]"), _SpawnPoint[spawnIndex].position, Quaternion.identity);
                SetSpawn(_SpawnPoint[0].position);
                SendOnJoinedMessage("Welcome to the game");
        }*/

        if (PhotonNetwork.IsConnected)
            photonView.RPC("SetNewSpawnIndex", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SetNewSpawnIndex() {
        spawnIndex++;
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

