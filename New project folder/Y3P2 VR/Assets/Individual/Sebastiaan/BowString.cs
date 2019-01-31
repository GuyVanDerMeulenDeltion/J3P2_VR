using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using Photon.Pun;

public class BowString : Bow {

    [Header("Bow References:")]
    [SerializeField] private float returnToStateSpeed = 8;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform startPos;

    [Header("Arrow Spawn Settings:")]
    [SerializeField] private GameObject ammo;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 pos;


    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    #region References
    private Animator parentAnim;

    private Transform currentHand;
    private GameObject _Arrow;

    internal bool firing;
    #endregion

    private void Awake() {
        parentAnim = parent.GetComponent<Animator>();
    }

    private void Update() {
        NewShoot(currentHand);
        parentAnim.SetFloat("DrawAxis", Mathf.Clamp(parentAnim.GetFloat("DrawAxis"), 0, 0.65f));
    }

    public void OnTriggerStay(Collider other) {
        if (other.transform.tag == "Hand") {
            currentHand = other.transform;;
        }
    }
    public override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        if (other.transform.tag == "Hand" && firing == false)
            currentHand = null;
    }

    private void ResetState(GameObject _ArrowNew, float _Force, Controller _Cont) {
        _ArrowNew.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _ArrowNew.GetComponent<Rigidbody>().AddForce(-transform.forward * 5000f * parentAnim.GetFloat("DrawAxis"));
        _ArrowNew.GetComponent<NetworkedAmmo>().canHit = true;
        _ArrowNew.GetComponent<Destroy>().enabled = true;
        _ArrowNew.transform.SetParent(null);
        HapticSpecific(0.5f, _Cont);
        if (_Force > 0.60f) {
            _ArrowNew.GetComponentInChildren<ParticleSystem>().Play();
            _ArrowNew.GetComponent<NetworkedAmmo>().calculateKinetics = false;
            _ArrowNew.GetComponent<NetworkedAmmo>().baseDamage = 750;
        }

        _Arrow = null;
        firing = false;
        currentHand.GetComponent<Controller>().item = null;
        currentHand = null;
    }

    private GameObject _SetArrow() {
        if (PhotonNetwork.IsConnected == false) return null;

        _Arrow = PhotonNetwork.InstantiateSceneObject(ammo.name, Vector3.zero, Quaternion.identity) as GameObject;
        _Arrow.transform.SetParent(transform);
        _Arrow.transform.localEulerAngles = rotation;
        _Arrow.transform.localPosition = pos;
        _Arrow.GetComponent<NetworkedAmmo>().enabled = false;
        _Arrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        currentHand.GetComponent<Controller>().item = _Arrow.gameObject;
        firing = true;

        return _Arrow;
    }

    public void NewShoot(Transform _Hand) {
        if (_Hand != null) {

            Controller _ControllerComp = _Hand.GetComponent<Controller>();

            switch (_ControllerComp.leftHand) {
                case true:
                    if (firing == false)
                        if (leftHandAxis > 0.85f) {
                            if (_Arrow == null)
                                _Arrow = _SetArrow();
                            return;
                        }

                    if (leftHandAxis != 0) {
                        Controller.canDrop = false;
                    }

                    parent.LookAt(_Hand.transform.position, parent.up);
                    parentAnim.SetFloat("DrawAxis", Vector3.Distance(startPos.position, _Hand.position));

                    if (leftHandAxis == 0 && _Arrow != null) {
                        Controller.canDrop = true;
                        ResetState(_Arrow, parentAnim.GetFloat("DrawAxis"), _ControllerComp);
                    }
                    break;

                case false:
                    if (firing == false)
                        if (rightHandAxis > 0.85f) {
                            if (_Arrow == null)
                                _Arrow = _SetArrow();
                            return;
                        }

                    if (leftHandAxis != 0) {
                        Controller.canDrop = false;
                    }

                    parent.LookAt(_Hand.transform.position, parent.up);
                    parentAnim.SetFloat("DrawAxis", Vector3.Distance(startPos.position, _Hand.position));

                    if (rightHandAxis == 0 && _Arrow != null) {
                        Controller.canDrop = true;
                        ResetState(_Arrow, parentAnim.GetFloat("DrawAxis"), _ControllerComp);
                    }
                    break;
            }
        }

        if (!firing && parentAnim.GetFloat("DrawAxis") != 0)
            parentAnim.SetFloat("DrawAxis", Mathf.Lerp(parentAnim.GetComponent<Animator>().GetFloat("DrawAxis"), 0, returnToStateSpeed * Time.deltaTime));
    }
}