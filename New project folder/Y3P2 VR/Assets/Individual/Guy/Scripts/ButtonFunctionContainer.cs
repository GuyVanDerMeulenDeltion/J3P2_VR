using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonFunctionContainer : MonoBehaviourPunCallbacks {

    [Header("Option Menu:")]
    public Animator options;

    private bool openedOptions = false;

    public void OpenOptionsMenu() {
        if (openedOptions == false) {
            openedOptions = true;
            options.SetBool("Open", true);
        } else if (openedOptions == true) {
            openedOptions = false;
            options.SetBool("Open", false);
        }

    }

    public void Quit() {
        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame()
    {
        PlayerManager.thisPlayer.playerMain.SendMessageLocally("Quitting game, please wait...");
        yield return new WaitForSeconds(4);

        if(Application.isEditor == false)
        Application.Quit();
        else
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ReturnPlayersToLobby()
    {
        photonView.RPC("Lobby", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void Lobby()
    {
        StartCoroutine(GoToLobby());
    }

    private IEnumerator GoToLobby()
    {
        PlayerManager.thisPlayer.playerMain.SendMessageLocally("Returning to lobby...");
        yield return new WaitForSeconds(4);
        PhotonNetwork.LoadLevel(1);
    }
}
