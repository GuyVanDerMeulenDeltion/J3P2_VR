using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class RightController : MonoBehaviour
{

    public GameObject rightHandItem;
    public GameObject rightHandSpawnPos { get { return (transform.GetChild(0).gameObject); } }
    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    private Vector3 lastPosition;
    public const float force = 2;


    private void Update()
    {
        DropItem();
        lastPosition = transform.position;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Interactable")
        {
            if (rightHandAxis > 0.85f && rightHandItem == null)
            {
                rightHandItem = other.gameObject;
                other.gameObject.transform.SetParent(rightHandSpawnPos.transform);
                rightHandItem.GetComponent<Rigidbody>().useGravity = false;
                rightHandItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rightHandItem.transform.position = rightHandSpawnPos.transform.position;
                rightHandItem.transform.rotation = rightHandSpawnPos.transform.rotation;
            }
        }
    }

    public void DropItem()
    {
        if (rightHandItem)
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                rightHandItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                rightHandItem.transform.SetParent(null);
                rightHandItem.GetComponent<Rigidbody>().useGravity = true;
                rightHandItem.GetComponent<Rigidbody>().velocity = transform.forward + (transform.position - lastPosition * force);
                rightHandItem = null;
            }
    }

}
