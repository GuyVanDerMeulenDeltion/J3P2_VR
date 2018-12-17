using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks {

    public static PhotonManager photonManager;

	// Use this for initialization
	private void Awake () {
        DontDestroyOnLoad(this);

        if (photonManager != null)
        Destroy(this);
        photonManager = this;
	}

    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        CreateRoom("test");
    }

    public void CreateRoom(string _RoomID) {
        print("Joined room");
        RoomOptions _Options = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(_RoomID, _Options, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel(1);
    }
}
