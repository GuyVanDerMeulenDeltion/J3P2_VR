using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks {
    public static GameManager gameManager;

    [SerializeField] private Transform[] _SpawnPoint;

    private int spawnIndex = 0;

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;
    }

    public void Start() {
        print(PhotonNetwork.IsConnected +" is the current state of photon network;");
        print(PhotonNetwork.CurrentRoom.Name+ " is the current room name");
        print(PhotonNetwork.PlayerList.Length+ " is the amount of players");

        if (PlayerManager.thisPlayer == null && PhotonNetwork.IsConnected) {
            PhotonNetwork.Instantiate("Cube", _SpawnPoint[spawnIndex].position, Quaternion.identity);
            photonView.RPC("SetSpawn", RpcTarget.All, _SpawnPoint[spawnIndex].position);
            photonView.RPC("SendOnJoinedMessage", RpcTarget.All, "All welcome the new player!");
            SendOnJoinedMessage("Welcome to the game");

        } else
            print("Could not spawn joined player");

        if (PhotonNetwork.IsConnected)
            photonView.RPC("SetNewSpawnIndex", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SetNewSpawnIndex() {
        spawnIndex++;
        print(PhotonNetwork.PlayerList.Length);
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

