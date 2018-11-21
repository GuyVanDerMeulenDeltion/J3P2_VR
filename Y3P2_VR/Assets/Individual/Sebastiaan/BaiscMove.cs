using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaiscMove : MonoBehaviourPunCallbacks
{
    public GameObject camera;
    public float speed;

    private void Start() {
        InitializeNetworkSync();
    }

    void InitializeNetworkSync() {
        if (!photonView.IsMine)
            camera.SetActive(false);
    }

    void Update()
    {
        if(photonView.IsMine)
        transform.Translate(new Vector3(Input.GetAxis("Horizontal")*Time.deltaTime*speed, 0, Input.GetAxis("Vertical")*Time.deltaTime*speed));
    }
}
