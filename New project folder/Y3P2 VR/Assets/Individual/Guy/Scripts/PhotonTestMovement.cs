using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonTestMovement : MonoBehaviourPunCallbacks {

    public static float speed = 10;

	// Use this for initialization
	private void Start () {
	    if(!photonView.IsMine) {
            this.enabled = false;
        }
	}

    private void Update() {
        Move();
    }

    private void Move() {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
    }
}
