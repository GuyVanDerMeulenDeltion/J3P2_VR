using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks {

    public static PhotonManager photonManager;
    public bool test = false;
    private bool loadedroom = false;

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
     /*   if (VR_Player.localPlayer != null && test == false) {
            VR_Player.localPlayer.SendMessageLocally("You have connected to the network.");
            return;
        }*/

        CreateRoom("Test");
    }

    public void CreateRoom(string _RoomID) {
        RoomOptions _Options = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(_RoomID, _Options, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel(1);
    }
}
