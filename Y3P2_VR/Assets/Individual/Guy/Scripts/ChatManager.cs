using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public static ChatManager chatManager;

     [SerializeField]private GameObject text_prefab;

     public InputField chatbox;

    public void Awake() {
        Initialisation();
    }

    public void Initialisation() {
        if (chatManager != null)
            return;

        chatManager = this;
    }

    public void SendMessage(Transform _Player) {
        photonView.RPC("SetTextForAllToSee", RpcTarget.All, (_Player, chatbox.text));
    }

    [PunRPC]
    public void SetTextForAllToSee(Transform _Player, string _String) {
        GameObject _NewLog = PhotonNetwork.Instantiate(text_prefab.name, Vector3.zero, Quaternion.identity);
        _NewLog.transform.SetParent(_Player);
        _NewLog.transform.localPosition = Vector3.zero + new Vector3(0, 8, 0);
        Text _Text = _NewLog.GetComponentInChildren<Text>();
        _Text.text = _String;
        ChatManager.chatManager.chatbox.text = "";
    }
}
