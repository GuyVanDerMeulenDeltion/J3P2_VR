using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicMovement : MonoBehaviourPunCallbacks, IPunObservable {
    public PhotonView myView;
    public Text text;
    public GameObject camera;
    public string name = "yes";

    public float speed;

    private Vector3 selfPos;

    private void Awake() {
        text.text = PhotonManager.photon.playername;
    }

    void Update() {
        if (myView.IsMine) {
            Move();
        } else
            SmoothedNetMovement();
    }

    private void SmoothedNetMovement() {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * speed);
    }

    private void Move() {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, Input.GetAxis("Vertical") * Time.deltaTime * speed));
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.IsWriting) {
            stream.SendNext(transform.position);
        } else {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }
}
