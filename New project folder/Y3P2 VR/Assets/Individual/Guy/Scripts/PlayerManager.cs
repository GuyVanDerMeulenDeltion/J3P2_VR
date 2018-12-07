using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable {

    public static PlayerManager thisPlayer;

    //Player components;
    public bool died = false;
    public bool test = false;

    [Header("Photon Sync Settings:")]
    public Transform[] childrenToSync;

    public Camera camera;
    internal VR_Player playerMain;
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
            return;
        }

        this.enabled = false;
    }

    private void Start() {
        if (this == thisPlayer)
        {
            EnemyManager.enemyManager.SetNewTarget(photonView.ViewID);
            if(test == false)
            playerMain.Initialise();
        }
    }

    private void GetComponentsFromPlayer() {
        if(test == true) {
            testMov = GetComponent<PhotonTestMovement>();
            testMov.enabled = true;
            camera.enabled = true;
        }

        playerMain = GetComponentInChildren<VR_Player>();
        player_head = GetComponentInChildren<PlayerHead>();
        player_menu = GetComponentInChildren<Menu>();
        camera = GetComponentInChildren<Camera>();
        area = GetComponentInChildren<SteamVR_PlayArea>();
        hands = GetComponentsInChildren<SteamVR_Behaviour_Pose>();
        reviveFields = GetComponentsInChildren<Player_Revivefield>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            foreach (Transform _Child in childrenToSync) {
                stream.SendNext(_Child.position);
                stream.SendNext(_Child.rotation);
            }
        } else if (stream.IsReading) {
            foreach (Transform _Child in childrenToSync) {
                _Child.position = (Vector3)stream.ReceiveNext();
                _Child.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }




    public void SetDeath() {
        if (died == false) {
            died = true;
            foreach (Player_Revivefield _Field in reviveFields) {
                _Field.SetReviveFieldState(false);
            }

            playerMain.SendMessageOnline("Someone has died!");
            playerMain.SendMessageLocally("You has died!");
            return;
        }
    }

    public void Revive() {
        if(died == true) {
            died = false;
            playerMain.SendMessageOnline("Someone has been revived!");
            playerMain.SendMessageLocally("You have been revived!");
            foreach (Player_Revivefield _Field in reviveFields) {
                _Field.SetReviveFieldState(true);
            }
        }
    }
}
