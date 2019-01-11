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
        //Implementer notes
        //This controlls the mitten grabbing animation.
        //Replace " Input.GetAxis("Horizontal") " with Controller trigger axis.
        SetGloveState();
    }

    public void SetGloveState() {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetGloveState", RpcTarget.All, photonView.ViewID, triggerAxis);
        else
            GetGloveState(photonView.ViewID, triggerAxis);

    }

    [PunRPC]
    private void GetGloveState(int _view, float _axis) {
        Animator _Glove = null;
        foreach(PhotonView _View in PhotonNetwork.PhotonViews)
        {
            if(_View.ViewID == _view)
            {
                _Glove = _View.transform.GetComponent<Animator>();
            }
        }

        if(_Glove != null)
          _Glove.SetFloat("Timeline", Mathf.Clamp(_axis, 0, 0.95f));
    }
}
