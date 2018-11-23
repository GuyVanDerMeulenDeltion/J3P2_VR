using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager gameManager;

    public static float _MAXPLAYERHEALTH = 100;
    public static float _PLAYERHEALTH = 100;

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;
    }

    public void Start() {
        if(PlayerManager.thisPlayer == null) {
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        }
    }

    public void CheckHealth() {
    /*    if(_PLAYERHEALTH <= 0) {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.LoadLevel(0);
        } */
    } 
}
