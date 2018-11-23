using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager thisPlayer;

    public BasicMovement player;
    public PlayerHead player_head;
    public PlayerCamera player_cam;
    public FlintLock player_Weapon;
    public HealthUI player_health;

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
        player_head = GetComponentInChildren<PlayerHead>();
        player_cam = GetComponentInChildren<PlayerCamera>();
        player_Weapon = GetComponentInChildren<FlintLock>();
        player_health = GetComponentInChildren<HealthUI>();
    }

    private void InitialiseComponents() {
        player.Initialise(PhotonNetwork.IsConnected && photonView.IsMine);
    }

    private void SendMessage() {
        ChatManager.chatManager.SendMessage(photonView.ViewID);
    }
}
