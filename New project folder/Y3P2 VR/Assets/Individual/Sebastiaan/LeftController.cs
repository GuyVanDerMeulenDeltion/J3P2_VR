using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LeftController : MonoBehaviour
{

    public GameObject leftHandItem;
    public GameObject leftHandSpawnPos { get { return (transform.GetChild(0).gameObject); } }

   // public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }

    private Vector3 lastPosition;
    public const float force = 2;

    private void Update()
    {
        lastPosition = transform.position;
        DropItem();
        if (leftHandItem != null)
            print(leftHandItem.GetComponent<FixedJoint>().currentForce);
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Interactable")
        {
            if(Input.GetButtonDown("Fire1"))
            //if (leftHandAxis > 0.85f && leftHandItem == null)
            {
                leftHandItem = other.gameObject;
                other.gameObject.transform.SetParent(leftHandSpawnPos.transform);               
                leftHandItem.GetComponent<Rigidbody>().useGravity = false;
                leftHandItem.GetComponent<FixedJoint>().connectedBody = transform.GetComponent<Rigidbody>();
                leftHandItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                leftHandItem.transform.position = leftHandSpawnPos.transform.position;
                leftHandItem.transform.rotation = leftHandSpawnPos.transform.rotation;
            }
        }
    }

    public void DropItem()
    {
        if (leftHandItem)
            if(Input.GetButtonDown("Fire2"))
            //if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                leftHandItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                leftHandItem.transform.SetParent(null);
                leftHandItem.GetComponent<Rigidbody>().useGravity = true;
                leftHandItem.GetComponent<FixedJoint>().connectedBody = null;
                leftHandItem = null;
            }
    }
}
