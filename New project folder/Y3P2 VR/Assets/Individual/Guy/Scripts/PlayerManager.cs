using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class PlayerManager : MonoBehaviourPunCallbacks {

    public static PlayerManager thisPlayer;

    //Player components;
    public bool died = false;

    public Camera camera;
    public GameObject[] menu;
    public VR_Player playerMain;

    [SerializeField] private GameObject playerbody;

    public InteractionManager interaction_manager;

    internal Hand[] player_hands_steam;
    internal Grayscale_ImageEffect player_grayscale;
    internal Controller[] player_controllers;
    internal PlayerHead player_head;
    internal Menu player_menu;
    internal SteamVR_PlayArea area;
    internal SteamVR_Behaviour_Pose[] hands;
    internal Player_Revivefield[] reviveFields;
    internal PhotonTestMovement testMov;
    internal Hitfield player_hitbox;
    internal Teleport[] teleporters;

    private void Awake()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            thisPlayer = this;
            GetComponentsFromPlayer();
            camera.enabled = true;
            playerbody.SetActive(false);
            return;
        }

        foreach (GameObject _MenuItem in menu)
            Destroy(_MenuItem);
        playerMain.enabled = false;
        Destroy(interaction_manager);
        this.enabled = false;
    }

    public void SetCurrentState() {
        player_head.isWielding = CheckHands();
    }

    private bool CheckHands() {
        foreach (Controller _Cont in player_controllers)
            if (_Cont.item != null)
                return true;

        return false;
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
            GetComponentsFromPlayer();
            playerMain.Initialise();
        }
    }

    private void GetComponentsFromPlayer() {
        player_hitbox = GetComponentInChildren<Hitfield>();
        player_head = GetComponentInChildren<PlayerHead>();
        player_menu = GetComponentInChildren<Menu>();
        camera = GetComponentInChildren<Camera>();
        area = GetComponentInChildren<SteamVR_PlayArea>();
        hands = GetComponentsInChildren<SteamVR_Behaviour_Pose>();
        reviveFields = GetComponentsInChildren<Player_Revivefield>();
        player_grayscale = GetComponentInChildren<Grayscale_ImageEffect>();
        player_controllers = GetComponentsInChildren<Controller>();
        player_hands_steam = GetComponentsInChildren<Hand>();
        teleporters = GetComponentsInChildren<Teleport>();
        //Steam_VR_Manager.steamManager.EnableRender();
    }

    public void SetDeath() {
        if (died == false) {
            playerMain.SetDeath();
            died = true;
            GameManager.gameManager.SetCount(1);
            EnemyManager.enemyManager.SetRemoveTarget(photonView.ViewID);
            foreach(Controller _Cont in PlayerManager.thisPlayer.player_controllers)
            {
                _Cont.DropObject(true);
            }

            foreach (Player_Revivefield _Field in reviveFields) {
                _Field.SetReviveFieldState(false);
            }
            return;
        }
    }

    public void Revive() {
        if(died == true) {
            died = false;
            GameManager.gameManager.SetCount(-1);
            playerMain.SetRevive();
            foreach (Player_Revivefield _Field in reviveFields) {
                _Field.SetReviveFieldState(true);
            }
        }
    }
}
