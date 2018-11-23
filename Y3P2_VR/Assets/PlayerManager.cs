using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager thisPlayer;

    public BasicMovement player;

    private void Awake() {
        if(photonView.IsMine || !PhotonNetwork.IsConnected) {
            thisPlayer = this;
        }

        GetComponentsFromPlayer();
        InitialiseComponents();
        ChatManager.chatManager.chatbox.onEndEdit.AddListener(delegate { SendMessage(); });
    }

    private void GetComponentsFromPlayer() {
        player = GetComponentInChildren<BasicMovement>();
    }

    private void InitialiseComponents() {
        player.Initialise(PhotonNetwork.IsConnected && photonView.IsMine);
    }

    private void SendMessage() {
        ChatManager.chatManager.SendMessage(transform);
    }
}
