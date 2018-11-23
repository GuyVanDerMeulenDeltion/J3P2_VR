using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public void Start() {
        if(PlayerManager.thisPlayer == null) {
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        }
    }
}
