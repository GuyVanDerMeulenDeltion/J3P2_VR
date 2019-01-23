using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using Photon.Pun;

public class BowString : Bow
{
    public GameObject ammo;
    private GameObject currentHand;
    private GameObject startPos { get { return transform.parent.GetChild(3).gameObject; } }

    private DrawOutline drawOutline { get { return new DrawOutline(); } }

    [HideInInspector]
    public bool firing;
    [SerializeField] private float drawoffset = 1.35f;

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
        GameObject iArrow = null;
        // -----------------------------------------------Left hand ------------------------------------------------
        if (currentHand != null && !firing)
        {
            if (leftHandAxis > 0.85f && currentHand.GetComponent<Controller>().leftHand)
            {
                firing = true;
                if (PhotonNetwork.IsConnected)
                    iArrow = PhotonNetwork.Instantiate(ammo.name, transform.position, transform.rotation) as GameObject;
                else
                    iArrow = Instantiate(ammo, transform.position, transform.rotation) as GameObject;

                iArrow.GetPhotonView().TransferOwnership(currentHand.GetPhotonView().Owner);
                if (iArrow.GetComponent<NetworkedAmmo>())
                    iArrow.GetComponent<NetworkedAmmo>().enabled = false;
                iArrow.transform.SetParent(transform);
                iArrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                iArrow.transform.localEulerAngles = new Vector3(-180, -90, 90);
                iArrow.transform.localPosition = new Vector3(0.98f, 0, -23.02f);
                currentHand.GetComponent<Controller>().item = iArrow.gameObject;
            }
        }
        else if (firing && currentHand != null)
        {
            if (currentHand.GetComponent<Controller>().leftHand)
            {
                transform.parent.LookAt(currentHand.transform.position, transform.parent.up);
                transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", Vector3.Distance(startPos.transform.position, currentHand.transform.position) * drawoffset);

                if (leftHandAxis == 0)
                {
                    transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    transform.GetChild(0).GetComponent<Rigidbody>().AddForce(-transform.forward * 5000f * transform.parent.GetComponent<Animator>().GetFloat("DrawAxis"));
                    transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().enabled = true;
                    //------------ temp
                    if (transform.GetChild(0).GetComponent<NetworkedAmmo>())
                    {
                        transform.GetChild(0).GetComponent<NetworkedAmmo>().canHit = true;
                        transform.GetChild(0).GetComponent<Destroy>().enabled = true;
                    }
                    if (transform.parent.GetComponent<Animator>().GetFloat("DrawAxis") >= 0.95f)
                        for (int i = 0; i < transform.GetChild(0).childCount; i++)
                        {
                            if (transform.GetChild(0).GetChild(i).GetComponent<ParticleSystem>())
                                transform.GetChild(0).GetChild(i).GetComponent<ParticleSystem>().Play();
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
                if (PhotonNetwork.IsConnected)
                    iArrow = PhotonNetwork.Instantiate(ammo.name, transform.position, transform.rotation) as GameObject;
                else
                    iArrow = Instantiate(ammo, transform.position, transform.rotation) as GameObject;

                iArrow.GetPhotonView().TransferOwnership(currentHand.GetPhotonView().Owner);
                if (iArrow.GetComponent<NetworkedAmmo>())
                    iArrow.GetComponent<NetworkedAmmo>().enabled = false;
                iArrow.transform.SetParent(transform);
                iArrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                iArrow.transform.localEulerAngles = new Vector3(-180, -90, 90);
                iArrow.transform.localPosition = new Vector3(0.98f, 0, -23.02f);
                currentHand.GetComponent<Controller>().item = iArrow.gameObject;
            }
        }
        else if (firing && currentHand != null)
        {
            if (currentHand.GetComponent<Controller>().rightHand)
            {
                transform.parent.LookAt(currentHand.transform.position, transform.parent.up);
                transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", Vector3.Distance(startPos.transform.position, currentHand.transform.position) * drawoffset);

                if (rightHandAxis == 0)
                {
                    iArrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    iArrow.GetComponent<Rigidbody>().AddForce(-transform.forward * 5000f * transform.parent.GetComponent<Animator>().GetFloat("DrawAxis"));
                    iArrow.GetComponent<NetworkedAmmo>().canHit = true;
                    iArrow.GetComponent<Destroy>().enabled = true;
                    currentHand.GetComponent<Controller>().item = null;
                    transform.GetChild(0).SetParent(null);
                    firing = false;
                    currentHand = null;
                }
            }
        }

        if (!firing && currentHand == null && transform.parent.GetComponent<Animator>().GetFloat("DrawAxis") != 0)
            transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", Mathf.Lerp(transform.parent.GetComponent<Animator>().GetFloat("DrawAxis"), 0, 0.2f));
    }

    public override void OnDisable()
    {
        if (firing)
        {
            firing = false;
            currentHand = null;
            Destroy(transform.GetChild(0).gameObject);
            transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", 0);
        }
    }
}