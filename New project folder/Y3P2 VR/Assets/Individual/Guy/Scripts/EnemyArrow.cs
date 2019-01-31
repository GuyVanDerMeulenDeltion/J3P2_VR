using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyArrow : MonoBehaviourPunCallbacks {

    public bool canDamage = true;
    private bool shouldDestroy = true;

    private void OnCollisionEnter(Collision _c) {
        canDamage = false;

        if (_c.transform.tag != "Spawner" && shouldDestroy == true)
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

        if (_O.transform.tag != "Spawner" && shouldDestroy == true)
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
