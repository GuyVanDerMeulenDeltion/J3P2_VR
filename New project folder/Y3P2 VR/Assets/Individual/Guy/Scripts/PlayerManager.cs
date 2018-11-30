using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class PlayerManager : MonoBehaviourPunCallbacks {

    public static PlayerManager thisPlayer;

    //Player components;
    public bool died = false;
    public bool test = false;

    internal VR_Player playerMain;
    internal PlayerHead player_head;
    internal Menu player_menu;
    public Camera camera;
    internal SteamVR_PlayArea area;
    internal SteamVR_Behaviour_Pose[] hands;

    private void Awake()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            thisPlayer = this;

            if(test == false)
            GetComponentsFromPlayer();
        }
    }

    private void Start() {
        if (this == thisPlayer)
        {
            if(test == false)
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
    
    public void SetDeath(int _Photonview) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("SetOnNetworkDeath", RpcTarget.All, _Photonview);
        else
            SetOnNetworkDeath(_Photonview);
    }

    [PunRPC]
    public void SetOnNetworkDeath(int _Photonview) {
        if(PhotonNetwork.IsConnected && _Photonview == photonView.ViewID) {
            died = true;
            return;
        }

        died = true;
    }
}
