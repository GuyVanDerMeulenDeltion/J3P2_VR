using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicMovement : NetworkedObject {
    public Text text;
    public GameObject camera;
    public string name = "";

    public float speed;

    private Vector3 selfPos;

    public override void Initialise(bool _Local) {
        SetNameplate();
        base.Initialise(_Local);
        camera.SetActive(true);
    }

    private void Update() {
        if (photonView.IsMine)
            Move();
    }

    public void SetNameplate() {
        text.text = photonView.Owner.NickName;
    }

    private void Move() {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, Input.GetAxis("Vertical") * Time.deltaTime * speed));
    }


}
