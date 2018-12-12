using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller : MonoBehaviour
{
    public GameObject item;
    public GameObject otherController;

    SteamVR_Behaviour_Pose controllerPose { get { return GetComponent<SteamVR_Behaviour_Pose>(); } }

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }
    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool leftHand, rightHand;

    private bool activated = false;

    public void Update()
    {
        DropObject();
        HideController();
        ActivateButton();
    }

    public void ActivateButton() {
        if ((leftHand && leftHandAxis > 0.85f) || (rightHand && rightHandAxis > 0.85f) && activated == false) {
            Buttons.Activate(gameObject);
            activated = true;
        } else if ((leftHand && leftHandAxis < 0.85f) || (rightHand && rightHandAxis < 0.85f) && activated == true){
            activated = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Interactable" && otherController.GetComponent<Controller>().item != other.gameObject)
        {
            if (leftHand && leftHandAxis > 0.85f)
                InteractionManager.PickObject(transform.gameObject, other.gameObject, item);

            if (rightHand && rightHandAxis > 0.85f)
                InteractionManager.PickObject(transform.gameObject, other.gameObject, item);

            if (PlayerManager.thisPlayer != null)
                PlayerManager.thisPlayer.SetCurrentState();
        }
    }

    public void DropObject()
    {
        if (leftHand)
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand)) 
                InteractionManager.ThrowObject(transform.gameObject, item, controllerPose);
            
        if (rightHand)
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand)) 
                InteractionManager.ThrowObject(transform.gameObject, item, controllerPose);
            

        if(PlayerManager.thisPlayer != null)
        PlayerManager.thisPlayer.SetCurrentState();
    }

    void HideController()
    {
        transform.GetComponent<SphereCollider>().enabled = item ? false : true;
        transform.GetChild(0).gameObject.SetActive(item ? false : true);
    }
}
