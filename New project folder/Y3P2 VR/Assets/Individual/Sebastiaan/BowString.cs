using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using Photon.Pun;

public class BowString : Bow
{
    public GameObject ammo;
    private GameObject currentHand;
    private GameObject startPos {  get { return transform.parent.GetChild(3).gameObject; } }

    private DrawOutline drawOutline { get { return new DrawOutline(); } }

    [HideInInspector]
    public bool firing;

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    private void Update()
    {
        Shoot();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Hand")
        {
            currentHand = other.gameObject;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Hand" && !firing)
            currentHand = null;
    }

    public void Shoot()
    {
        // -----------------------------------------------Left hand ------------------------------------------------
        if (currentHand != null && !firing)
        {
            if (leftHandAxis > 0.85f && currentHand.GetComponent<Controller>().leftHand)
            {
                GameObject _arrow;
                firing = true;
                if(PhotonNetwork.IsConnected)
                 _arrow = PhotonNetwork.Instantiate(ammo.name, transform.position, transform.rotation) as GameObject;
                else
                    _arrow = Instantiate(ammo, transform.position, transform.rotation) as GameObject;

                if (_arrow.GetComponent<NetworkedAmmo>())
                    _arrow.GetComponent<NetworkedAmmo>().enabled = false;
                _arrow.transform.SetParent(transform);
                _arrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                currentHand.GetComponent<Controller>().item = _arrow.gameObject;
            }
        }
        else if (firing && currentHand != null)
        {
            if (currentHand.GetComponent<Controller>().leftHand)
            {
                transform.parent.LookAt(currentHand.transform.position,transform.parent.up);
                transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", Vector3.Distance(startPos.transform.position, currentHand.transform.position));
                
                if (leftHandAxis == 0)
                {
                    transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    transform.GetChild(0).GetComponent<Rigidbody>().AddForce(-transform.forward * 5000f*transform.parent.GetComponent<Animator>().GetFloat("DrawAxis"));
                    //------------ temp
                    transform.GetChild(0).GetComponent<TempFirework>().enable = true;
                    if (transform.GetChild(0).GetComponent<NetworkedAmmo>()) {
                        transform.GetChild(0).GetComponent<NetworkedAmmo>().canHit = true;
                        transform.GetChild(0).GetComponent<Destroy>().enabled = true;
                    }
                    currentHand.GetComponent<Controller>().item = null;
                    transform.GetChild(0).SetParent(null);
                    firing = false;
                    currentHand = null;
                }
            }
        }
        //-------------------------------------------Right hand ------------------------------------------

        if (currentHand != null && !firing)
        {
            if (rightHandAxis > 0.85f && currentHand.GetComponent<Controller>().rightHand)
            {
                firing = true;
                GameObject _arrow = PhotonNetwork.InstantiateSceneObject("Ammo", transform.position, transform.rotation) as GameObject;
                _arrow.GetPhotonView().TransferOwnership(currentHand.GetPhotonView().Owner);
                _arrow.transform.SetParent(transform);
                _arrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                currentHand.GetComponent<Controller>().item = _arrow.gameObject;
            }
        }
        else if (firing && currentHand != null)
        {
            if (currentHand.GetComponent<Controller>().rightHand)
            {
                transform.parent.LookAt(currentHand.transform.position,transform.parent.up);
                transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", Vector3.Distance(startPos.transform.position,currentHand.transform.position));

                if (rightHandAxis == 0)
                {
                    transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    transform.GetChild(0).GetComponent<Rigidbody>().AddForce(-transform.forward * 5000f*transform.parent.GetComponent<Animator>().GetFloat("DrawAxis"));
                    //------------ temp
                    transform.GetChild(0).GetComponent<TempFirework>().enable = true;
                    if (transform.GetChild(0).GetComponent<NetworkedAmmo>()) {
                        transform.GetChild(0).GetComponent<NetworkedAmmo>().canHit = true;
                        transform.GetChild(0).GetComponent<Destroy>().enabled = true;
                    }
                    currentHand.GetComponent<Controller>().item = null;
                    transform.GetChild(0).SetParent(null);
                    firing = false;
                    currentHand = null;
                }
            }
        }

        if (!firing && currentHand == null && transform.parent.GetComponent<Animator>().GetFloat("DrawAxis") != 0)       
            transform.parent.GetComponent<Animator>().SetFloat("DrawAxis",Mathf.Lerp(transform.parent.GetComponent<Animator>().GetFloat("DrawAxis"), 0,0.2f));       
    }
}
