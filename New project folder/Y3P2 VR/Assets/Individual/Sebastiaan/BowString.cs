using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class BowString : Bow
{
    public GameObject ammo;

    private DrawOutline drawOutline {  get { return new DrawOutline(); } }

    private bool firing;

    private Vector3 startArrowPos;

    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    public void OnEnable()
    {
        drawOutline.enabled = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Hand")
        {
            if(leftHandAxis >0.85f && other.transform.GetComponent<Controller>().leftHand)
            {
                if (!firing)
                {
                    GameObject _arrow = Instantiate(ammo, transform.position, transform.rotation) as GameObject;
                    _arrow.transform.SetParent(transform);
                }
                transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", other.transform.position.z - transform.position.z);

            }
            if(rightHandAxis >0.85f && other.transform.GetComponent<Controller>().rightHand)
            {
                if (!firing)
                {
                    startArrowPos = other.transform.position;

                    GameObject _arrow = Instantiate(ammo, transform.position, transform.rotation) as GameObject;
                    _arrow.transform.SetParent(transform);
                    firing = true;
                }
                transform.parent.GetComponent<Animator>().SetFloat("DrawAxis", other.transform.position.z - transform.position.z);
                print(startArrowPos - other.transform.position);
            }
        }
    }

    public void OnDisable()
    {
        drawOutline.enabled = false;
    }
}
