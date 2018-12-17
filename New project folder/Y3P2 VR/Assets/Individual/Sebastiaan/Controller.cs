using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class Controller : MonoBehaviourPunCallbacks
{
    public GameObject item;
    public GameObject otherController;

    [Header("Hand visuals:")]
    public Mitten mitten;

    SteamVR_Behaviour_Pose controllerPose { get { return GetComponent<SteamVR_Behaviour_Pose>(); } }

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }
    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool leftHand, rightHand;

    private bool activated = false;

    public void Update()
    {
        SetMitten();
        DropObject();
        HideController();
        ActivateButton();
    }

    //Used for the hand visuals
    private void SetMitten() {
        if(leftHand == true) {
            mitten.triggerAxis = leftHandAxis;
            return;
        }

        if(rightHand == true) {
            mitten.triggerAxis = rightHandAxis;
            return;
        }
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
                if(item != null)
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.gameObject.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID, true);
                else
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.gameObject.GetComponent<PhotonView>().ViewID, 0, false);

            if (rightHand && rightHandAxis > 0.85f)
                if(item != null)
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID, true);
                else
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.GetComponent<PhotonView>().ViewID, 0, false);


            if (PlayerManager.thisPlayer != null)
                PlayerManager.thisPlayer.SetCurrentState();
        }
    }

    public void DropObject()
    {
        if (leftHand)
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand)) 
                InteractionManager.intManager.DropObjectNetwork(transform.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID);
            
        if (rightHand)
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
                InteractionManager.intManager.DropObjectNetwork(transform.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID);
            

        if(PlayerManager.thisPlayer != null)
        PlayerManager.thisPlayer.SetCurrentState();
    }

    void HideController()
    {
        transform.GetComponent<SphereCollider>().enabled = item ? false : true;
        mitten.transform.GetChild(1).gameObject.SetActive(item ? false : true);
    }
}
