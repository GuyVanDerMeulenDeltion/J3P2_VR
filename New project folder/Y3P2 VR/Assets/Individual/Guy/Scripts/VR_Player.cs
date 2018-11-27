using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class VR_Player : MonoBehaviourPunCallbacks {

    public static VR_Player localPlayer;

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

    public void SendMessageOnline(string _Message) {
        photonView.RPC("BroadcastMessage", RpcTarget.All, _Message);

    }

    public void SendMessageLocally(string _Message) {
        broadcastAnim.SetTrigger("Send");
        broadcastText.text = _Message;
    }

    [PunRPC]
    private new void BroadcastMessage(string _Message) {
        broadcastAnim.SetTrigger("Send");
        broadcastText.text = _Message;
    }
}
