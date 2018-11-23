using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedObject : MonoBehaviourPunCallbacks
{

    public virtual void Initialise(bool _IsLocal) {
        if (!_IsLocal) {
            this.enabled = false;
            return;
        }
    }
}
