using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyArrow : MonoBehaviourPunCallbacks {

    private bool canDamage = true;

    private void OnCollisionEnter(Collision _c) {
        canDamage = false;

        if (_O.transform.tag != "Spawner")
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider _O)
    {
        if(_O.transform.tag == "Player")
        {
            if (_O.transform.root.gameObject.GetComponent<PlayerManager>().died == false)
            {
                _O.transform.root.gameObject.GetComponent<PlayerManager>().SetDeath();
            }
        }

        if(_O.transform.tag != "Spawner")
        Destroy(gameObject);
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
