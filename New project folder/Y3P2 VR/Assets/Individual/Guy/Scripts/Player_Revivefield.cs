using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Revivefield : MonoBehaviourPunCallbacks {

    public bool isMine = false;
    public bool canRevive = true;

    private void OnTriggerEnter(Collider _O) {
        if(_O.transform.tag == "Hand") {
            if(_O.GetComponent<Player_Revivefield>().isMine == false && isMine == true && canRevive == true && PlayerManager.thisPlayer.died == true) {
                Revive();
            }
        }
    }
    
    public void SetReviveFieldState(bool _State) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetReviveFieldState", RpcTarget.All);
    }

    [PunRPC]
    private void GetReviveFieldState(bool _State) {
            canRevive = _State;
    }

    public void Revive() {
        PlayerManager.thisPlayer.Revive();
        
    }
}
