using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkDestroy : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool destroyParent;

    public void ActivateNetworkDestroy() {
        DestroyObjectOverNetwork(destroyParent);
    }

    private void DestroyObjectOverNetwork(bool _DestroyParent) {
        if (_DestroyParent == true) {
            PhotonNetwork.Destroy(gameObject.transform.parent.gameObject);
            return;
        } 
            PhotonNetwork.Destroy(gameObject);
    }
}
