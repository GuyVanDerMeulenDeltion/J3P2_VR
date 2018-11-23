using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager gameManager;

    public static float _MAXPLAYERHEALTH = 250;
    public static float _PLAYERHEALTH = _MAXPLAYERHEALTH;

    public void Awake() {
        if (gameManager != null) return;
        gameManager = this;
    }

    public void Start() {
        if(PlayerManager.thisPlayer == null) {
            if (PhotonManager.photon._DEVMODE == false)
                PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            else
                PhotonNetwork.Instantiate("Player_Alpha Variant", Vector3.zero, Quaternion.identity);
        }
    }

    public void CheckHealth() {
        if(_PLAYERHEALTH <= 0) {
            _PLAYERHEALTH = _MAXPLAYERHEALTH;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            Application.Quit();
        } 
    } 
}
