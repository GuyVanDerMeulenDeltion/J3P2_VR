using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class FlintLock : Interactables
{
    private Animator myanim;

    private FlintlockHammer flintLockHammer { get { return GameObject.Find("Hammer").GetComponent<FlintlockHammer>(); } }

    public bool firing;
    private bool canFire = true;
    private bool cocked = true;

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    void Start()
    {
        myanim = GetComponent<Animator>();
    }

    void Update()
    {
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;

        Recock();
        Trigger();



        //Notes for implementer:
        //Replace the following "Input.GetAxis("Horizontal")" with the HTC Vive Trigger on the current hand (min 0, max 1).
        myanim.SetFloat("TriggerAxis", Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f));


        //Notes for implementer:
        //Replace the following "Input.GetAxis("Vertical")" with recocking action progress (min 0, max 1).
        //  myanim.SetFloat("RecockAxis", Mathf.Clamp(Input.GetAxis("Vertical"), 0, 0.95f));

    }

    void Recock()
    {
        if (!cocked &&flintLockHammer.currentHand !=null)
        {
            transform.GetComponent<Animator>().SetFloat("RecockAxis", Vector3.Distance(transform.position, flintLockHammer.currentHand.transform.position));
            if (transform.GetComponent<Animator>().GetFloat("RecockAxis") >= 1f)
            {
                cocked = true;
                print("Cocked");
            }
        }
    }

    void Trigger()
    {
        if (flintLockHammer.currentHand != null)
        {
            if (flintLockHammer.currentHand.GetComponent<Controller>().leftHand && leftHandAxis > 0.05f && canFire)
            {
                firing = true;
                transform.GetComponent<Animator>().SetFloat("TriggerAxis", leftHandAxis);
                print(leftHandAxis);
                if (leftHandAxis > 0.99f)
                    Shoot();
            }
            else if (flintLockHammer.currentHand.GetComponent<Controller>().rightHand && rightHandAxis > 0.05f && canFire)
            {
                firing = true;
                transform.GetComponent<Animator>().SetFloat("TriggerAxis", rightHandAxis);
                print(rightHandAxis);
                if (rightHandAxis > 0.99f)
                    Shoot();
            }
            else
                firing = false;
        }
    }

    void Shoot()
    {
        //spawn particles

       // firing = false;
     //   cocked = false;
        print("Shot");
    }
}