using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class PlayerManager : MonoBehaviourPunCallbacks {

    public static PlayerManager thisPlayer;

    internal VR_Player playerMain;

    //Player components;
    internal PlayerHead player_head;
    internal Menu player_menu;
    internal Camera camera;
    internal SteamVR_PlayArea area;
    internal SteamVR_Behaviour_Pose[] hands;

    private void Awake()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            thisPlayer = this;
            GetComponentsFromPlayer();
        }
    }

    private void Start() {
        if (this == thisPlayer)
        {
            playerMain.Initialise();
        }
    }

    private void GetComponentsFromPlayer() {
        playerMain = GetComponentInChildren<VR_Player>();
        player_head = GetComponentInChildren<PlayerHead>();
        player_menu = GetComponentInChildren<Menu>();
        camera = GetComponentInChildren<Camera>();
        area = GetComponentInChildren<SteamVR_PlayArea>();
        hands = GetComponentsInChildren<SteamVR_Behaviour_Pose>();
    }
}
