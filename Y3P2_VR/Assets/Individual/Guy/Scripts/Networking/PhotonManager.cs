using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks {

    public static PhotonManager photon;

    [Header("Menu:")]
    public GameObject menu;

    private void Awake() {
        DontDestroyOnLoad(this);
        PhotonNetwork.AutomaticallySyncScene = true;

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
        SetupRoom("Standard", "Test" + Random.Range(0, 100));
    }

    public void SetupRoom(string _Identifier, string name) {
        Debug.Log("Created a room with the ID of: " + _Identifier);
        RoomOptions _RoomOptions = new RoomOptions() { IsOpen = true, MaxPlayers = 20};
        PhotonNetwork.JoinOrCreateRoom(_Identifier, _RoomOptions, TypedLobby.Default);
        SetNetworkName(name);
    }

    public void SetNetworkName(string _Newname) {
        PhotonNetwork.NickName = _Newname;
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("Game");
    }
}
