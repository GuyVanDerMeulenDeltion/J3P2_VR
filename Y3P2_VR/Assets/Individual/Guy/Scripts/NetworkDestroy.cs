using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkDestroy : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool destroyParent;

    public void ActivateNetworkDestroy() {
        if(destroyParent == true) {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }

        Destroy(gameObject);
    }
}
