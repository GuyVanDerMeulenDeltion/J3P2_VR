using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviour {

    public static PhotonManager photon;

    [Header("Menu:")]
    public GameObject menu;

    private void Awake() {
        if (photon != null) return;
        photon = this;
    }

    public void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConnectedToMaster() {
        print("Connected to master.");
    }

    public void OnJoinedLobby() {
        Debug.Log("Joined Lobby.");
    }
}
