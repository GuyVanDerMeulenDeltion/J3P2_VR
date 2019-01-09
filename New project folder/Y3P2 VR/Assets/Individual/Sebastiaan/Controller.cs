using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class Controller : MonoBehaviourPunCallbacks
{
    public static Vector3 currentPos;

    public GameObject item;
    public GameObject otherController;

    [Header("Movement Settings:")]
    public Teleport teleport;

    [Header("Hand visuals:")]
    public Mitten mitten;
    SteamVR_Behaviour_Pose controllerPose { get { return GetComponent<SteamVR_Behaviour_Pose>(); } }

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    public bool touchpadLeft {  get { return SteamVR_Input._default.inActions.TouchpadTouch.GetState(SteamVR_Input_Sources.LeftHand); } }
    public bool touchpadRight { get { return SteamVR_Input._default.inActions.TouchpadTouch.GetState(SteamVR_Input_Sources.RightHand); } }

    public bool touchpadLeftPress { get { return SteamVR_Input._default.inActions.TouchpadPress.GetStateDown(SteamVR_Input_Sources.LeftHand); } }
    public bool touchpadRightPress { get { return SteamVR_Input._default.inActions.TouchpadPress.GetStateDown(SteamVR_Input_Sources.RightHand); } }

    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;

    public bool leftHand, rightHand;

    private bool activated = false;
    private bool teleported = false;

    private void Awake() {
        currentPos = transform.root.position;
    }

    public void Update()
    {
        SetMitten();
        DropObject();
        HideController();
        ActivateButton();
        CurrentPosition();
    }

    private void CurrentPosition() {
        transform.root.position = Vector3.Lerp(transform.root.position, currentPos, teleport.teleportLerpSpeed * Time.deltaTime);
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

        if ((touchpadLeft && leftHand) || (touchpadRight && rightHand)) {
            teleport.enabled = true;
        } else {
            teleport.enabled = false;
        }

        if (teleported == false && ((leftHand && touchpadLeftPress) || (rightHand && touchpadRightPress))) {
            print("yes");
            teleport.TeleportPlayer();
            teleported = true;
        } else {
            teleported = false;
        }

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
