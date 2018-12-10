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

    public Camera camera;
    public VR_Player playerMain;
    internal Grayscale_ImageEffect player_grayscale;
    internal PlayerHead player_head;
    internal Menu player_menu;
    internal SteamVR_PlayArea area;
    internal SteamVR_Behaviour_Pose[] hands;
    internal Player_Revivefield[] reviveFields;
    internal PhotonTestMovement testMov;

    private void Awake()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            thisPlayer = this;
            GetComponentsFromPlayer();
            camera.enabled = true;
            return;
        }

        playerMain.enabled = false;
        this.enabled = false;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.E))
            SetDeath();
        else if (Input.GetKeyDown(KeyCode.R))
            Revive();
    }

    private void Start() {
        if (this == thisPlayer)
        {
            EnemyManager.enemyManager.SetNewTarget(photonView.ViewID);
            if (test == false)
            GetComponentsFromPlayer();
            playerMain.Initialise();
        }
    }

    private void GetComponentsFromPlayer() {
        if(test == true) {
            testMov = GetComponent<PhotonTestMovement>();
            testMov.enabled = true;
            camera.enabled = true;
        }

        player_head = GetComponentInChildren<PlayerHead>();
        player_menu = GetComponentInChildren<Menu>();
        camera = GetComponentInChildren<Camera>();
        area = GetComponentInChildren<SteamVR_PlayArea>();
        hands = GetComponentsInChildren<SteamVR_Behaviour_Pose>();
        reviveFields = GetComponentsInChildren<Player_Revivefield>();
        player_grayscale = GetComponentInChildren<Grayscale_ImageEffect>();
        Steam_VR_Manager.steamManager.EnableRender();
    }

    public void SetDeath() {
        if (died == false) {
            playerMain.SetDeath();
            foreach (Player_Revivefield _Field in reviveFields) {
                _Field.SetReviveFieldState(false);
            }
            return;
        }
    }

    public void Revive() {
        if(died == true) {
            playerMain.SetRevive();
            foreach (Player_Revivefield _Field in reviveFields) {
                _Field.SetReviveFieldState(true);
            }
        }
    }
}
