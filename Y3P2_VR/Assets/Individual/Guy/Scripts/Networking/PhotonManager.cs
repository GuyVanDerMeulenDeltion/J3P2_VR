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
    public bool _DEVMODE = false;
    public bool _ENTERDEVROOM = false;

    private void Awake() {
        DontDestroyOnLoad(this);

        if (photon != null) return;
        photon = this;
    }

    private void Start() {
        PhotonNetwork.SerializationRate = 40;
        PhotonNetwork.SendRate = 80;
        SetupPhoton();
    }
    
    private void SetupPhoton() {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to photon...");
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        print("Connected to master.");


        if(_ENTERDEVROOM == false)
        SetupRoom("Standard", "Tyler" + Random.Range(0, 100));
        else
        SetupRoom("Devs", "Dev_" + Random.Range(0, 100));
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
