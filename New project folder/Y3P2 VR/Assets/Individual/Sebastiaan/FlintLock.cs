using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using Photon.Pun;

public class FlintLock : Interactables
{
    private Animator myanim { get { return GetComponent<Animator>();} }

    [SerializeField] private FlintlockHammer flintLockHammer;
    [SerializeField] private Transform bulletSpawnpos;

    internal bool firing;
    private bool resetTrigger;

    internal bool cocking;

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    private void OnEnable()
    {
        //resets the hand upon pickup
        flintLockHammer.currentHand = null;
    }

    void Update()
    {
        //applies pickup rotation offset
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;

        Recock();
        Trigger();
        ResetTrigger();
    }

    void Recock()
    {
        //recocks flintlock by distance from hand you recock with to the hammer multiplied my 4 to get a  realistic feeling
        if (flintLockHammer.currentHand != null)
        {
            cocking = true;
            if (flintLockHammer.currentHand.GetComponent<Controller>().leftHand && leftHandAxis > 0.99f || flintLockHammer.currentHand.GetComponent<Controller>().rightHand && rightHandAxis > 0.99f)
            {
                myanim.SetBool("Firing", false);
                myanim.SetFloat("RecockAxis", Mathf.Clamp(Vector3.Distance(transform.position, flintLockHammer.currentHand.transform.position) * 3f, 0, 2f));
                if (myanim.GetFloat("RecockAxis") > 0.95f)
                    myanim.SetBool("Cocked", true);
            }

        }
        else       
            cocking = false;       
    }
    void Trigger()
    {
        //grabs the trigger value of the hand you hold the weapon with and applies it to the animation
        if (transform.parent.GetComponent<Controller>().leftHand && leftHandAxis > 0.05f && myanim.GetBool("Cocked") && resetTrigger)
        {
            firing = true;
            myanim.SetFloat("TriggerAxis", leftHandAxis);
            if (leftHandAxis > 0.99f)
                Shoot();
        }
        else if (transform.parent.GetComponent<Controller>().rightHand && rightHandAxis > 0.05f && myanim.GetBool("Cocked") && resetTrigger)
        {
            firing = true;
            myanim.SetFloat("TriggerAxis", rightHandAxis);
            if (rightHandAxis > 0.99f && myanim.GetBool("Cocked"))
                Shoot();
        }
        else
        {
            firing = false;
            myanim.SetFloat("TriggerAxis", Mathf.Lerp(myanim.GetFloat("TriggerAxis"), 0, 0.01f));
            myanim.SetFloat("RecockAxis", Mathf.Lerp(myanim.GetFloat("RecockAxis"), 0, 0.05f));
        }

    }

    void Shoot()
    {
        //resetting variables
        firing = false;
        myanim.SetBool("Cocked", false);
        myanim.SetBool("Firing",true);
        myanim.SetFloat("RecockAxis", 0);
        resetTrigger = false;


        //spawn particles
        //instantiate bullet
    }

    public void ShootBullet()
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("ShootBulletNetworked", RpcTarget.All);
        else
            ShootBulletNetworked();
    }
    
    [PunRPC]
    private void ShootBulletNetworked()
    {
        GameObject _NewBullet;
        if (PhotonNetwork.IsConnected && gameObject.GetPhotonView().IsMine)
        {
            _NewBullet = PhotonNetwork.Instantiate("Bullet", bulletSpawnpos.position, Quaternion.Euler(-transform.right));
            _NewBullet.GetComponent<Rigidbody>().AddForce(-transform.right * 80, ForceMode.Impulse);
        }
        else
        {
            _NewBullet = (GameObject)Instantiate(Resources.Load("Bullet"), bulletSpawnpos.position, Quaternion.Euler(-transform.right));
            _NewBullet.GetComponent<Rigidbody>().AddForce(-transform.right * 80, ForceMode.Impulse);
        }

    }

    void ResetTrigger()
    {
        //prevents instant firing on pickup
        if (transform.parent.GetComponent<Controller>().leftHand && leftHandAxis == 0f)
            resetTrigger = true;

        if (transform.parent.GetComponent<Controller>().rightHand && rightHandAxis == 0f)
            resetTrigger = true;
    }

    private void OnDisable()
    {
        //resets variables on disable
        if (flintLockHammer.currentHand != null)
            flintLockHammer.currentHand = null;
        myanim.SetFloat("TriggerAxis", 0);
        myanim.SetFloat("RecockAxis", 0);
    }
}
/*void Update()
{
    triggerAxis = Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f);
    rechamberAxis = Mathf.Clamp(Input.GetAxis("Vertical"), 0.1f, 0.95f);

    myAnim.SetFloat("TriggerAxis", triggerAxis);
    myAnim.SetFloat("RechamberAxis", rechamberAxis);
}
*/