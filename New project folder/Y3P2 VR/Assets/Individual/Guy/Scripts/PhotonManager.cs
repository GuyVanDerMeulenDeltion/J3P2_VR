using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks {

    public static PhotonManager photonManager;

    [SerializeField]private bool joinTest = false;
    
    // Use this for initialization
    private void Awake() {
        DontDestroyOnLoad(this);

        if (photonManager != null)
            Destroy(this);
        photonManager = this;
    }

    private void Start() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        if (joinTest == false)
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.JoinOrCreateRoom("Test", new RoomOptions(), TypedLobby.Default);

        print(PhotonNetwork.CloudRegion+ " is the region.");
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 }, null);
    }
}

