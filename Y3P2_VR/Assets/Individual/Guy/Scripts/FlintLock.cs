using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlintLock : MonoBehaviourPunCallbacks
{
    public Transform bulletspawn;

    private Animator myanim;
    private float triggerTimeline;
    private bool readyForRecock;
    public float cooldown;
    private float currentCooldown;

    private GameObject currentBullet;

    //For testing purposes;
    public static float _MAXCOOLDOWN = 1;
    public static float _COOLDOWN = _MAXCOOLDOWN;

    //public float triggerAxis;
    //public float recockAxis;
    void Start()
    {
        myanim = GetComponent<Animator>();
    }
   
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            ShootBullet(false);

        if (Input.GetButton("Fire1"))
            ShootBullet(true);

        //Notes for implementer:
        //Replace the following "Input.GetAxis("Horizontal")" with the HTC Vive Trigger on the current hand (min 0, max 1).
        myanim.SetFloat("TriggerAxis", Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f));

        
        if (currentCooldown < 0)
        {
            //Notes for implementer:
            //Replace the following "Input.GetAxis("Vertical")" with recocking action progress (min 0, max 1).
            myanim.SetFloat("RecockAxis", Mathf.Clamp(Input.GetAxis("Vertical"), 0, 0.95f));          
        }
        
        if(currentCooldown >= 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void ChangeStateReadyToFire()
    {
        //myanim.SetBool("Ready", true); temp
    }

    public void ApplyCooldown()
    {
        currentCooldown = cooldown;
    }

    public void ShootBullet(bool _ApplyCooldown) {
        if (_ApplyCooldown == true) {
            if (_COOLDOWN > 0) {
                _COOLDOWN -= Time.deltaTime;
                return;
            }
        }

        _COOLDOWN = _MAXCOOLDOWN;
        float[] _Pos = {bulletspawn.position.x, bulletspawn.position.y, bulletspawn.position.z};
        float[] _Rot = { bulletspawn.eulerAngles.x, bulletspawn.eulerAngles.y, bulletspawn.eulerAngles.z };
        photonView.RPC("NetworkShootBullet", RpcTarget.All, (_Pos));
        photonView.RPC("CorrectBulletRotation", RpcTarget.All, (_Rot));

    }

    [PunRPC]
    public void NetworkShootBullet(float[] _Pos) {
        Vector3 _NewPos = new Vector3(_Pos[0], _Pos[1], _Pos[2]);
        GameObject _BulletInstance = _BulletInstance = Instantiate(Resources.Load("Bullet") as GameObject, _NewPos, Quaternion.identity);
        currentBullet = _BulletInstance;
    }

    [PunRPC]
    public void CorrectBulletRotation(float[] _Eulers) {
        if(currentBullet != null)
        currentBullet.transform.eulerAngles = new Vector3(_Eulers[0], _Eulers[1], _Eulers[2]);
    }
}
