using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class Controller : MonoBehaviourPunCallbacks
{
    public static Vector3 currentPos;
    public static float teleportTimer = 1; //Timer that decides when the held item can function again... 
    private static float timer;
    public static bool canDrop = true;

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

    private void Awake() {
        currentPos = transform.root.position;
        timer = teleportTimer;
    }

    public void Update()
    {
        SetMitten();
        Timer(false);
        HideController();

        if (PlayerManager.thisPlayer.died == true)
        {
            if(teleport.enabled == true)
            teleport.enabled = false;
            return;
        }

        DropObject(false);
        ActivateButton();
        CurrentPosition();
    }

    private void CurrentPosition() {
        transform.root.position = Vector3.Lerp(transform.root.position, currentPos, teleport.teleportLerpSpeed * Time.deltaTime);
    }

    private void Timer(bool _ResetTimer)
    {
        if(_ResetTimer == true)
        {

            timer = teleportTimer;
            foreach(Controller _Cont in PlayerManager.thisPlayer.player_controllers)
            {
                canDrop = false;
                if (_Cont.item != null)
                    if (_Cont.GetComponentInChildren<Sword>())
                    {
                        _Cont.GetComponentInChildren<Sword>().enabled = false;
                        if (_Cont.GetComponentInChildren<TrailRenderer>())
                            _Cont.GetComponentInChildren<TrailRenderer>().enabled = false;
                    }
                    else if (_Cont.GetComponentInChildren<Shield>())
                        _Cont.GetComponentInChildren<Shield>().enabled = false;

            }
        }

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            canDrop = true;
            foreach (Controller _Cont in PlayerManager.thisPlayer.player_controllers)
            {
                if (_Cont.item != null)
                    if (_Cont.GetComponentInChildren<Sword>())
                    {
                        _Cont.GetComponentInChildren<Sword>().enabled = true;
                        if(_Cont.GetComponentInChildren<TrailRenderer>())
                        _Cont.GetComponentInChildren<TrailRenderer>().enabled = true;
                    }
                    else if (_Cont.GetComponentInChildren<Shield>())
                        _Cont.GetComponentInChildren<Shield>().enabled = true;
            }
        }

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

        if ((touchpadLeft && leftHand) || (touchpadRight && rightHand) && PlayerManager.thisPlayer.died == false) {
            teleport.enabled = true;
        } else {
            teleport.enabled = false;
        }

        if ((leftHand && touchpadLeftPress) || (rightHand && touchpadRightPress) && PlayerManager.thisPlayer.died == false) {
            teleport.TeleportPlayer();
            Timer(true);
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
        if(other.transform.GetComponent<LevelTrigger>())
        {
            if (leftHand && leftHandAxis > 0.85f)
            {
                other.transform.GetComponent<LevelTrigger>().LoadLevel();
                return;
            }

            if (rightHand && rightHandAxis > 0.85f)
            {
                other.transform.GetComponent<LevelTrigger>().LoadLevel();
                return;
            }

        }

        if (other.transform.tag == "Interactable" && otherController.GetComponent<Controller>().item != other.gameObject)
        {
            if (leftHand && leftHandAxis > 0.85f && PlayerManager.thisPlayer.died == false)
                if(item != null)
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.gameObject.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID, true);
                else
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.gameObject.GetComponent<PhotonView>().ViewID, 0, false);

            if (rightHand && rightHandAxis > 0.85f && PlayerManager.thisPlayer.died == false)
                if(item != null)
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID, true);
                else
                    InteractionManager.intManager.PickObjectNetwork(GetComponent<PhotonView>().ViewID, other.GetComponent<PhotonView>().ViewID, 0, false);


            if (PlayerManager.thisPlayer != null)
                PlayerManager.thisPlayer.SetCurrentState();
        }
    }

    public void DropObject(bool _ForceDrop)
    {
        if (item != null & _ForceDrop == true)
        {
            InteractionManager.intManager.DropObjectNetwork(transform.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID);
            return;
        }

        if (canDrop == true)
        {
            if (leftHand)
                if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    if (item != null)
                        InteractionManager.intManager.DropObjectNetwork(transform.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID);
                    else
                        InteractionManager.intManager.DropObjectNetwork(transform.GetComponent<PhotonView>().ViewID, 0);

            if (rightHand)
                if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
                    if(item != null)
                    InteractionManager.intManager.DropObjectNetwork(transform.GetComponent<PhotonView>().ViewID, item.GetComponent<PhotonView>().ViewID);
                            else
                    InteractionManager.intManager.DropObjectNetwork(transform.GetComponent<PhotonView>().ViewID, 0);


            if (PlayerManager.thisPlayer != null)
                PlayerManager.thisPlayer.SetCurrentState();
        }
    }

    void HideController()
    {
        transform.GetComponent<SphereCollider>().enabled = item ? false : true;
        mitten.transform.GetChild(1).gameObject.SetActive(item ? false : true);
    }
}
