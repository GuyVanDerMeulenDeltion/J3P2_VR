using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller : MonoBehaviour
{
    public GameObject leftItem;
    public GameObject rightItem;

    SteamVR_Behaviour_Pose controllerPose {  get { return GetComponent<SteamVR_Behaviour_Pose>(); } }

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    public void Update()
    {
        DropObject();
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Interactable")
            if (leftHandAxis > 0.85f)
                InteractionManager.PickObject(transform.gameObject, other.gameObject, leftItem);
            else if (rightHandAxis > 0.85f)
                InteractionManager.PickObject(transform.gameObject, other.gameObject, rightItem);
    }

    public void DropObject()
    {
        if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
            InteractionManager.ThrowObject(transform.gameObject, leftItem,controllerPose);

        if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
            InteractionManager.ThrowObject(transform.gameObject, rightItem,controllerPose);
       
    }
}
