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

    public void SetTextForAllToSee() {
        GameObject _NewLog = PhotonNetwork.Instantiate(text_prefab.name, Vector3.zero, Quaternion.identity);
        _NewLog.transform.SetParent(PlayerManager.thisPlayer.transform);
        _NewLog.transform.localPosition = Vector3.zero + new Vector3(0, 8, 0);
        Text _Text = _NewLog.GetComponentInChildren<Text>();
        _Text.text = chatbox.text;
        PlayerManager.thisPlayer.SetTextboxText(_NewLog.GetPhotonView().ViewID, _Text.text);
        ChatManager.chatManager.chatbox.text = "";
    }
}
