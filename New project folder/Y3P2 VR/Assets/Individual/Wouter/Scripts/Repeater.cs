using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using Photon.Pun;

public class Repeater : Interactables
{
    private Animator myAnim { get { return GetComponent<Animator>();} }

    [SerializeField] private RepeaterReload repeaterReload;
    [SerializeField] private Transform bulletSpawnpos;

    internal bool firing;
    private bool resetTrigger;

    private float triggerAxis;
    private float rechamberAxis;

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    private void OnEnable()
    {
        //resets the hand upon pickup
        repeaterReload.currentHand = null;
    }

    void Update()
    {
        //applies pickup rotation offset
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;

        ReChamber();
        Trigger();
        ResetTrigger();
    }

   private void ReChamber()
    {
        //recocks flintlock by distance from hand you recock with to the hammer multiplied my 4 to get a  realistic feeling
        if (repeaterReload.currentHand != null)

            if (repeaterReload.currentHand.GetComponent<Controller>().leftHand && leftHandAxis > 0.99f || repeaterReload.currentHand.GetComponent<Controller>().rightHand && rightHandAxis > 0.99f)
                myAnim.SetFloat("RechamberAxis", Mathf.Clamp(Vector3.Distance(transform.position, repeaterReload.currentHand.transform.position) * 3f, 0, 2f));
        else
                myAnim.SetFloat("RechamberAxis", Mathf.Lerp(myAnim.GetFloat("RechamberAxis"), 0.1f, 0.05f));
    }
   private void Trigger()
    {
        //grabs the trigger value of the hand you hold the weapon with and applies it to the animation
        if (transform.parent.GetComponent<Controller>().leftHand && leftHandAxis > 0.05f && resetTrigger)
        {
            firing = true;
            myAnim.SetFloat("TriggerAxis", leftHandAxis);
        }
        else if (transform.parent.GetComponent<Controller>().rightHand && rightHandAxis > 0.05f && resetTrigger)
        {
            firing = true;
            myAnim.SetFloat("TriggerAxis", rightHandAxis);
        }
        else
        {
            firing = false;
            myAnim.SetFloat("TriggerAxis", Mathf.Lerp(myAnim.GetFloat("TriggerAxis"), 0, 0.01f));         
        }
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
        Haptic();
        GameObject _NewBullet;
        if (PhotonNetwork.IsConnected && gameObject.GetPhotonView().IsMine)
        {
            _NewBullet = PhotonNetwork.Instantiate("Bullet_Winchester", bulletSpawnpos.position, Quaternion.Euler(-transform.right));
            _NewBullet.GetComponent<Rigidbody>().AddForce(-transform.right * 80, ForceMode.Impulse);
        }
        else
        {
            _NewBullet = (GameObject)Instantiate(Resources.Load("Bullet_Winchester"), bulletSpawnpos.position, Quaternion.Euler(-transform.right));
            _NewBullet.GetComponent<Rigidbody>().AddForce(-transform.right * 80, ForceMode.Impulse);
        }

    }

    private void ResetTrigger()
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
            repeaterReload.currentHand = null;
        myAnim.SetFloat("TriggerAxis", 0);
        myAnim.SetFloat("RechamberAxis", 0);
    }
}