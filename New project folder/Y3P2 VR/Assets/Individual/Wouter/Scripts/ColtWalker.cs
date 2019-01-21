using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Photon.Pun;
using UnityEngine;

public class ColtWalker : Interactables {

    [SerializeField] private Transform bulletSpawnPos;

    private Animator myAnim { get { return GetComponent<Animator>(); } }
    private float triggerAxis;
    internal int bulletsLoaded;
    private bool canFire;

    internal float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    internal float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    private void Update()
    {
        Trigger();

        triggerAxis = Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f);

        myAnim.SetFloat("TriggerAxis", triggerAxis);
        myAnim.SetInteger("Bullets", bulletsLoaded);
    }

    private void EditBulletcount(int amount)
    {
        bulletsLoaded +=amount;
    }

    private void Trigger()
    {
        if (transform.parent.GetComponent<Controller>().leftHand && leftHandAxis > 0.05f && bulletsLoaded > 0)
        {
            myAnim.SetFloat("TriggerAxis", leftHandAxis);
            if (myAnim.GetFloat("TriggerAxis") > 0.99f && canFire)
                Shoot();
        }
        else
            canFire = true;
    }

    private void Shoot()
    {
        bulletsLoaded--;
        canFire = false;

        if (PhotonNetwork.IsConnected)
            photonView.RPC("SpawnBullet", RpcTarget.All);
        else
            SpawnBullet();
    }

    [PunRPC]
    private void SpawnBullet()
    {
        GameObject iNewBullet;
        if (gameObject.GetPhotonView().IsMine)
        {
            iNewBullet = PhotonNetwork.Instantiate("Colt-walker_Bullet", bulletSpawnPos.position, Quaternion.Euler(-transform.right));
            iNewBullet.GetComponent<Rigidbody>().AddForce(-transform.right * 80, ForceMode.Impulse);
        }
        else
        {
            iNewBullet = (GameObject)Instantiate(Resources.Load("Colt-walker_Bullet"), bulletSpawnPos.position, Quaternion.Euler(-transform.right));
            iNewBullet.GetComponent<Rigidbody>().AddForce(-transform.right * 80, ForceMode.Impulse);
        }
    }
}
