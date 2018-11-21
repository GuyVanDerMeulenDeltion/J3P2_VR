using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaiscMove : MonoBehaviourPunCallbacks
{
    public float speed;

    void Update()
    {
        if(photonView.IsMine)
        transform.Translate(new Vector3(Input.GetAxis("Horizontal")*Time.deltaTime*speed, 0, Input.GetAxis("Vertical")*Time.deltaTime*speed));
    }
}
