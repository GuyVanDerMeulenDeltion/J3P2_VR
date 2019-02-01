using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Photon.Pun;
using UnityEngine;

public class ColtWalker : Interactables {

    [SerializeField] private Transform bulletSpawnPos;

    private Animator myAnim { get { return GetComponent<Animator>(); } }
    private float triggerAxis;
    internal int bulletsLoaded = 6;
    private bool canFire;

    internal float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    internal float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    private void Update()
    {
        bulletsLoaded = (int)Mathf.Clamp(bulletsLoaded, 0, 6);
        Trigger();
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;

        myAnim.SetInteger("Bullets", bulletsLoaded);
    }

    private void EditBulletcount(int amount)
    {
        bulletsLoaded += amount;
    }

    private void Trigger()
    {
        if (transform.parent.GetComponent<Controller>().leftHand && leftHandAxis > 0.05f)
        {
            myAnim.SetFloat("TriggerAxis", leftHandAxis);

        }

        if (transform.parent.GetComponent<Controller>().rightHand && rightHandAxis > 0.05f)
        {
            myAnim.SetFloat("TriggerAxis", rightHandAxis);
        }
    }

    internal void Shoot()
    {
        canFire = false;
        SpawnBullet();
    }

    private void SpawnBullet()
    {
        GameObject iNewBullet;
        if (gameObject.GetPhotonView().IsMine)
        {
            iNewBullet = PhotonNetwork.InstantiateSceneObject("Colt-walker_Bullet", bulletSpawnPos.position, Quaternion.Euler(-transform.right));
            iNewBullet.GetComponent<Rigidbody>().AddForce(-transform.right * 80, ForceMode.Impulse);
        }
    }
}
