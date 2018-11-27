using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks {

    public static PhotonManager photonManager;

	// Use this for initialization
	private void Awake () {
        if (photonManager != null)
        Destroy(this);
        photonManager = this;
	}

    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        if(VR_Player.localPlayer != null)
        VR_Player.localPlayer.SendMessageLocally("You have connected to the network.");
    }
}
