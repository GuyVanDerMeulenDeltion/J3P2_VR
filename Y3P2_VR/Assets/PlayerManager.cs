using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager thisPlayer;

    public BasicMovement player;

    private void Awake() {
        if(photonView.IsMine || !PhotonNetwork.IsConnected) {
            thisPlayer = this;
        }

        GetComponentsFromPlayer();
        InitialiseComponents();
    }

    private void GetComponentsFromPlayer() {
        player = GetComponentInChildren<BasicMovement>();
    }

    private void InitialiseComponents() {
        player.Initialise(PhotonNetwork.IsConnected && photonView.IsMine);
    }

    public void CallNewText(int _PhotonID, string _Text) {
        photonView.RPC("SetTextboxText", RpcTarget.All, (_PhotonID, _Text));
    }

    [PunRPC]
    public void SetTextboxText(int _PhotonViewID, string _Text) {
        foreach (PhotonView _View in PhotonNetwork.PhotonViews) {
            if (_View.ViewID == _PhotonViewID) {
                _View.gameObject.GetComponentInChildren<Text>().text = _Text;
            }
        }
    }
}
