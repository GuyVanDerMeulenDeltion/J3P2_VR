using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks {

    public static PhotonManager photon;

    public string playername;
    public List<string> players = new List<string>();

    [Header("Menu:")]
    public GameObject menu;

    private void Awake() {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;

        if (photon != null) return;
        photon = this;
    }

    private void Start() {
        SetupPhoton();
    }
    
    private void SetupPhoton() {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to photon...");
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        print("Connected to master.");
        PhotonNetwork.NickName = playername;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby() {
        base.OnJoinedLobby();
        Debug.Log("Succesfully joined lobby.");
        menu.SetActive(true);
    }

    public void SetupRoom(string _Identifier, string name) {
        playername = name;
        Debug.Log("Created a room with the ID of: " + _Identifier);
        RoomOptions _RoomOptions = new RoomOptions() { IsOpen = true, MaxPlayers = 20, };
        PhotonNetwork.JoinOrCreateRoom(_Identifier, _RoomOptions, TypedLobby.Default);
        PhotonNetwork.LoadLevel("Game");
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Game") {
            print("Sceneload");
            InstantiatePlayer(PhotonNetwork.PlayerList.Length);
        }
    }

   private void InstantiatePlayer(int i) {
        GameObject _Player = PhotonNetwork.Instantiate(players[i], Vector3.zero, Quaternion.identity);
        print("instantiated");
   }
}
