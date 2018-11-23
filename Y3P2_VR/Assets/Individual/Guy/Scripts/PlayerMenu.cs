using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PlayerMenu : MonoBehaviourPunCallbacks
{
    private Animator thisAnimator;

    private void Awake() {
        thisAnimator = GetComponent<Animator>();
    }

    public void LoadUI(bool _DetectedRay) {
        photonView.RPC("SyncMenu", RpcTarget.All, _DetectedRay);
    }

    [PunRPC]
    private void SyncMenu(bool _State) {
        thisAnimator.SetBool("Opened", _State);
    }
}
