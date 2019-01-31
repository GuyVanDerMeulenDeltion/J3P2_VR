using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class Mitten : MonoBehaviourPunCallbacks {

    internal float triggerAxis;

    private Animator myanim;

    private SteamVR_TrackedObject trackedObject;
    private Valve.VR.SteamVR_TrackedObject device;

    private void Start()
    {
        myanim = GetComponent<Animator>();
    }

    private void Update() {
        SetGloveState();
    }

    public void SetGloveState() {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetGloveState", RpcTarget.All, triggerAxis);
    }

    [PunRPC]
    private void GetGloveState(float _axis) {
            GetComponent<Animator>().SetFloat("Timeline", Mathf.Clamp(_axis, 0, 0.95f));
    }
}
