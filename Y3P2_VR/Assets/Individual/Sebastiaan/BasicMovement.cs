using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicMovement : MonoBehaviourPunCallbacks {
    public Image healthbar;

    public Text text;
    public GameObject camera;
    public string name = "";

    public float speed;

    private Vector3 selfPos;

    public void Initialise(bool _Local) {
        SetNameplate();

        if (!_Local) {
            this.enabled = false;
            return;
        }

        PlayerManager.thisPlayer.player_health.enabled = true;
        PlayerManager.thisPlayer.player_cam.enabled = true;
        PlayerManager.thisPlayer.player_Weapon.enabled = true;
    }

    private void Update() {
        if (photonView.IsMine)
            Move();
    }

    public void SetNameplate() {
        text.text = photonView.Owner.NickName;
    }

    public void SetNameDuringSession(string _NewName) {
        photonView.RPC("SetNameOverNetwork", RpcTarget.All, _NewName);
    }

    [PunRPC]
    public void SetNameOverNetwork(string _NewName) {
        GetComponentInChildren<Text>().text = _NewName;
    }

    public void GetDamaged(Vector3 _Pos) {
        photonView.RPC("InstantiateHitParticleOverNetwork", RpcTarget.All, _Pos);
        GameManager._PLAYERHEALTH -= 25;
        GameManager.gameManager.CheckHealth();
        PlayerManager.thisPlayer.player_health.SetHealth();
        photonView.RPC("SpawnHitsplashViaNetwork", RpcTarget.All, transform.position + new Vector3(0, 2, 0));
    }

    public void CreateDamageParticle(Vector3 _Pos) {
        photonView.RPC("InstantiateHitParticleOverNetwork", RpcTarget.All, _Pos);
    }

    [PunRPC]
    public void SpawnHitsplashViaNetwork(Vector3 _Pos) {
        Instantiate(Resources.Load("Hitsplash"), _Pos, Quaternion.identity);
    }

    [PunRPC]
    public void InstantiateHitParticleOverNetwork(Vector3 _PointOfImpact) {
        Instantiate(Resources.Load("ImpactEffect"), _PointOfImpact, Quaternion.identity);
    }

    private void Move() {
        if (Input.GetButton("Fire2"))
            transform.eulerAngles += new Vector3(0, +Input.GetAxis("Mouse X") * PlayerCamera._SENSITIVITY * Time.deltaTime, 0);
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, Input.GetAxis("Vertical") * Time.deltaTime * speed));
    }
}
