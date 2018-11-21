using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks {

    public static PhotonManager photon;

    [Header("Menu:")]
    public GameObject menu;

    private void Awake() {
        if (photon != null) return;
        photon = this;
    }

    public void Start() {
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(this);
    }

    public override void OnConnectedToMaster() {
        print("Connected to master.");
        menu.SetActive(true);
    }

    public void SetupRoom(string _Identifier) {
        Debug.Log("Created a room with the ID of: " + _Identifier);
        RoomOptions _RoomOptions = new RoomOptions() { IsOpen = true, MaxPlayers = 20, };
        PhotonNetwork.JoinOrCreateRoom(_Identifier, _RoomOptions, TypedLobby.Default);
        PhotonNetwork.LoadLevel(1);
    }
}
