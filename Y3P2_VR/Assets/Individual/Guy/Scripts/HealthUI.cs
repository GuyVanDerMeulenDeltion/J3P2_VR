using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HealthUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image healthbarFill;

    private GameObject _targetPlayer;

    public void SetHealth() {
        float _FillAmount = GameManager._PLAYERHEALTH / GameManager._MAXPLAYERHEALTH;

        photonView.RPC("FindNetworkReference", RpcTarget.AllBuffered, photonView.ViewID);
        photonView.RPC("BroadCastUI", RpcTarget.AllBuffered, _FillAmount);
    }

    [PunRPC]
    public void FindNetworkReference(int _PhotonID) {
        _targetPlayer = PhotonView.Find(_PhotonID).gameObject;
    }

    [PunRPC]
    public void BroadCastUI(float _Fill) {
        _targetPlayer.GetComponentInChildren<HealthUI>().gameObject.GetComponent<Image>().fillAmount = _Fill;
    }
}
