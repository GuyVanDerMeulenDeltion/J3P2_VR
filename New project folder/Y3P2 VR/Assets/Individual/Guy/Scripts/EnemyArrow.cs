using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyArrow : MonoBehaviourPunCallbacks {

    private void OnTriggerEnter(Collider _O)
    {
        if(_O.transform.tag == "Player")
        {
            photonView.RPC("HitPlayer", RpcTarget.AllBuffered, _O.gameObject.GetPhotonView().ViewID);
        }
    }

    [PunRPC]
    private void HitPlayer(int _I)
    {
        foreach(PhotonView _View in PhotonNetwork.PhotonViews)
        {
            if(_View.ViewID == _I)
            {
                if(_View.IsMine && PlayerManager.thisPlayer.died == false)
                {
                    PlayerManager.thisPlayer.SetDeath();
                    Destroy(this);
                }
            }
        }
    }
}
