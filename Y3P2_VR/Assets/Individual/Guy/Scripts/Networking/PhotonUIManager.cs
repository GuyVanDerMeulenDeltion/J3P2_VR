using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonUIManager : MonoBehaviour
{
    public static PhotonUIManager photonUI;

    [Header("Player Settings:")]
    public InputField name;

    [Header("Create Room Settings:")]
    public InputField createRoomField;

    [Header("Create Room Settings:")]
    public InputField joinRoomField;

    [Header("Status UI:")]
    public GameObject[] connectionStatus;

    public void Awake() {
        if (photonUI != null) return;
        photonUI = this;
    }

    public void CreateRoom() {
        PhotonManager.photon.SetupRoom(createRoomField.text, name.text);
    }

    //Loads up the correct UI that represents the current connection status;
    public void SetVisualConnectionStatus(int _Index) {
        foreach (GameObject _Status in connectionStatus)
            _Status.SetActive(false);

            connectionStatus[_Index].SetActive(true);
    }
}
