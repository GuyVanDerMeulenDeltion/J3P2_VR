using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LevelTrigger : MonoBehaviourPunCallbacks {

    private static bool selectedLevel = false;

    [SerializeField] private int levelIndex;

    #region References
    private Animator thisAnim;
    #endregion

    private void Awake () {
        thisAnim = GetComponent<Animator>();
	}

    private void Update() {
        levelIndex = (int)Mathf.Clamp(levelIndex, 0, 2);
    }

    private void OnTriggerEnter(Collider _O) {
        if(_O.transform.tag == "Hand") {
            thisAnim.SetTrigger("Clap");
        }
    }

    //Main function to load a level over the network;
    internal void LoadLevel() {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("LoadLevelForPlayers", RpcTarget.AllBuffered, levelIndex);
        else
            LoadLevelForPlayers(levelIndex);
    }


    [PunRPC]
    private void LoadLevelForPlayers(int _LevelIndex) {
        selectedLevel = true;
        PhotonNetwork.LoadLevel(_LevelIndex); //Loads the level for the player with given overload;
    }
}
