using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LevelTrigger : MonoBehaviourPunCallbacks {

    private static bool selectedLevel = false;
    private static float outlineThickness = 1.82f;
    private static int levelLoadTimer = 4;

    [SerializeField] private bool loadScene = false;
    [SerializeField] private int levelIndex;

    #region References
    private MeshRenderer thisRender;
    private Animator thisAnim;
    #endregion

    private void Awake () {
        thisAnim = GetComponentInChildren<Animator>();
        thisRender = GetComponentInChildren<MeshRenderer>();
        thisRender.materials[1].SetFloat("_Thickness", 0);
        selectedLevel = false;
    }

    private void Start() {
        if (loadScene == true)
            LoadLevel();
    }

    private void OnTriggerEnter(Collider _O) {
        if(_O.transform.tag == "Hand") {
            thisAnim.SetTrigger("Clap");
            thisRender.materials[1].SetFloat("_Thickness", outlineThickness);
        }
    }

    private void OnTriggerExit(Collider _O)
    {
        if (_O.transform.tag == "Hand")
        {
            thisRender.materials[1].SetFloat("_Thickness", 0);
        }
    }

    //Main function to load a level over the network;
    internal void LoadLevel() {
        if (selectedLevel == false)
        {
            if (PhotonNetwork.IsConnected)
                photonView.RPC("LoadLevelForPlayers", RpcTarget.MasterClient, levelIndex);
            else
                LoadLevelForPlayers(levelIndex);
        }
    }


    [PunRPC]
    private void LoadLevelForPlayers(int _LevelIndex) {
        selectedLevel = true;
        PlayerManager.thisPlayer.playerMain.SendMessageLocally("Level has been selected!");
        StartCoroutine(LoadingLevel(_LevelIndex));
    }

    private IEnumerator LoadingLevel(int _LevelIndex)
    {
        yield return new WaitForSeconds(levelLoadTimer);
        PhotonNetwork.LoadLevel(_LevelIndex); //Loads the level for the player with given overload;
    }
}
