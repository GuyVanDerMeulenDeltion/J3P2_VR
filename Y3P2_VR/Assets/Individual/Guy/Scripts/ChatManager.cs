using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public static ChatManager chatManager;

     [SerializeField]private GameObject text_prefab;
                     private GameObject currentChatMessage;
        
     public InputField chatbox;

    public void Awake() {
        Initialisation();
    }

    public void Initialisation() {
        if (chatManager != null)
            return;

        chatManager = this;
    }

    public void SendMessage(int _PhotonID) {
        photonView.RPC("SetTextForAllToSee", RpcTarget.All, (string)chatbox.text);
        photonView.RPC("SetChatTransform", RpcTarget.All, _PhotonID);

        ChatManager.chatManager.chatbox.text = "";
    }

    [PunRPC]
    public void SetTextForAllToSee(string _String) {
        currentChatMessage = Instantiate(text_prefab, Vector3.zero, Quaternion.identity);
        Text _Text = currentChatMessage.GetComponentInChildren<Text>();
        _Text.text = _String;
        if (_Text.text == "") Destroy(currentChatMessage);
    }

    [PunRPC]
    public void SetChatTransform(int _PhotonID) {
        currentChatMessage.transform.SetParent(PhotonView.Find(_PhotonID).transform);
        currentChatMessage.transform.localPosition = Vector3.zero + new Vector3(0, 7, 0);
    }
}
