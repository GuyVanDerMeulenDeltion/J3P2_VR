using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks {
    public static GameManager gameManager;

    private static float loadTime = 5;

    internal static int deathCount = 0;

    [SerializeField] private Transform[] _SpawnPoint;

    private int spawnIndex = 0;

    public void OnApplicationQuit() {
        spawnedPlayer = false;
    }

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;

    }

    public void UpdateEnemyKill()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (SpawnManager.spawnManager != null)
            {
                SpawnManager.enemyCount -= 1;

                if (SpawnManager.enemyCount <= 0)
                {
                    photonView.RPC("EndMatch", RpcTarget.AllBuffered, "Completed level, returning to main lobby...");
                }
            }
        }
    }

    public void Start() {
        print(PhotonNetwork.IsConnected +" is the current state of photon network;");
        print(PhotonNetwork.CurrentRoom.Name+ " is the current room name");
        print(PhotonNetwork.PlayerList.Length+ " is the amount of players");

        if (PlayerManager.thisPlayer == null && PhotonNetwork.IsConnected) {
            PhotonNetwork.Instantiate("[CameraRig]", _SpawnPoint[spawnIndex].position, Quaternion.identity);
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

    internal void SetCount(int _Add)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetCount", RpcTarget.MasterClient, _Add);
        else
            GetCount(_Add);
    }


    [PunRPC]
    private void GetCount(int _Add)
    {
        deathCount += _Add;

        if (deathCount >= PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("EndMatch", RpcTarget.MasterClient, "Everyone died, loading lobby...");
        }
    }

    [PunRPC]
    private void EndMatch(string _Message)
    {
        StartCoroutine(LoadMenu(_Message));
    }

    private IEnumerator LoadMenu(string _Message)
    {
        yield return new WaitForSeconds(2);
        PlayerManager.thisPlayer.playerMain.SendMessageLocally(_Message);
        yield return new WaitForSeconds(loadTime);
        PhotonNetwork.LoadLevel(1);
    }
}

