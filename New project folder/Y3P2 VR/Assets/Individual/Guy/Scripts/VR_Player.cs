using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Valve.VR;

public class VR_Player : MonoBehaviourPunCallbacks {

    public static VR_Player localPlayer;

    public bool isDead = false;

    [SerializeField] private bool isLocal = false;
    [SerializeField] private Animator broadcastAnim;
    [SerializeField] private TextMeshProUGUI broadcastText;

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
        broadcastAnim.SetTrigger("Broadcast");
        broadcastText.text = _Message;
    }

    [PunRPC]
    private new void BroadcastMessage(string _Message) {
        broadcastAnim.SetTrigger("Broadcast");
        broadcastText.text = _Message;
    }
}
