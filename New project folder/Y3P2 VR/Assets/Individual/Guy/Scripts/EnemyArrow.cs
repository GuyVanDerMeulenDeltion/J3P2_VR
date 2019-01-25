using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyArrow : MonoBehaviourPunCallbacks {

    private bool canDamage = true;

    private void OnCollisionEnter(Collision _c) {
        GetComponent<Rigidbody>().useGravity = true;
        canDamage = false;
    }

    private void OnTriggerEnter(Collider _O)
    {
        if(_O.transform.tag == "Player")
        {
            if(_O.gameObject.GetPhotonView() != null)
            photonView.RPC("HitPlayer", RpcTarget.AllBuffered, _O.gameObject.GetPhotonView().ViewID);
            Destroy(gameObject);
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
                    if (canDamage == true) {
                        PlayerManager.thisPlayer.SetDeath();
                        Destroy(this);
                    }
                }
            }
        }
    }
}
