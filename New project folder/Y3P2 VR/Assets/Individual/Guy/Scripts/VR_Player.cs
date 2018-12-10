using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Valve.VR;

public class VR_Player : MonoBehaviourPunCallbacks {

    public static VR_Player localPlayer;

    public static string death_Message = "O-oh, someone has been removed from the cast!";

    [SerializeField] private bool isLocal = false;
    [SerializeField] private Animator deathscreenAnim;
    [SerializeField] private Animator broadcastAnim;
    [SerializeField] private TextMeshProUGUI broadcastText;

    private bool ignoreBroadcastedMessage = false;


    private void Awake() {
        SetLocalPlayer();
    }

    private void SetLocalPlayer() {
        if (isLocal == true)
            localPlayer = this;
    }

    public void Initialise() {
        PlayerManager.thisPlayer.playerMain.enabled = true;
        PlayerManager.thisPlayer.player_head.enabled = true;
        PlayerManager.thisPlayer.player_menu.enabled = true;
        PlayerManager.thisPlayer.camera.enabled = true;
        PlayerManager.thisPlayer.area.enabled = true;

        for(int i = 0; i < PlayerManager.thisPlayer.hands.Length; i++) {
            PlayerManager.thisPlayer.hands[i].enabled = true;
        }
}

    public void SendMessageOnline(string _Message) {
        photonView.RPC("BroadcastMessage", RpcTarget.All, _Message);

    }

    public void SendMessageLocally(string _Message) {
        BroadcastMessage(_Message);
    }

    [PunRPC]
    private new void BroadcastMessage(string _Message) {
        if (ignoreBroadcastedMessage == false) {
            broadcastAnim.SetTrigger("Send");
            broadcastText.text = _Message;
        }

        ignoreBroadcastedMessage = false;
    }

    public void SetDeath() {
        ignoreBroadcastedMessage = true;
        deathscreenAnim.SetTrigger("Open");
        PlayerManager.thisPlayer.died = true;
        PlayerManager.thisPlayer.player_grayscale.enabled = true;

        if (PhotonNetwork.IsConnected)
            SendMessageOnline(death_Message);
        else
            SendMessageLocally(death_Message);
    }

    public void SetRevive() {
        SendMessageOnline("Someone has been revived!");
        SendMessageLocally("You have been revived!");
        PlayerManager.thisPlayer.player_grayscale.enabled = false;
        PlayerManager.thisPlayer.died = false;
    }
}
