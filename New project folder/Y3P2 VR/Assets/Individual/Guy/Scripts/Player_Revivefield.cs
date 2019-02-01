using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Revivefield : MonoBehaviourPunCallbacks {

    private void OnTriggerEnter(Collider _O) {
        if(_O.transform.tag == "Hand") {
            if(_O.gameObject.GetPhotonView().IsMine == false)
            {
                photonView.RPC("Revive", RpcTarget.All, _O.transform.root.gameObject.GetPhotonView().ViewID);
            }
        }
    }

    [PunRPC]
    public void Revive(int i) {
        PlayerManager _Player = null;

        foreach(PhotonView _View in PhotonNetwork.PhotonViews)
        {
            if(_View.ViewID == i)
            {
                _Player = _View.transform.GetComponent<PlayerManager>();
                break;
            }
        }

        if(_Player.died == true)
        _Player.Revive();
    }
}
