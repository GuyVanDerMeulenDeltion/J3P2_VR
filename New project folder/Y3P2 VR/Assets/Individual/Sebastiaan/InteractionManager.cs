using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR;
using Valve.VR;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public GameObject leftHandItem;
    public GameObject rightHandItem;

    public GameObject leftHandSpawnPos {  get {  return (transform.GetChild(0).gameObject); } }
    public GameObject rightHandSpawnPos {  get { return (transform.GetChild(1).gameObject); } }

    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }
    public float leftHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand); } }

    private void Update()
    {
        DropItem();
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Interactable")
        {
            if(leftHandAxis >0.85f &&leftHandItem == null)
            {
                other.gameObject.transform.SetParent(leftHandItem.transform);
                leftHandItem = other.gameObject;
                leftHandItem.GetComponent<Rigidbody>().useGravity = false;
                leftHandItem.transform.position = leftHandSpawnPos.transform.position;
                leftHandItem.transform.rotation = leftHandSpawnPos.transform.rotation;
            }

            if (rightHandAxis > 0.85f &&rightHandItem == null)
            {
                other.gameObject.transform.SetParent(rightHandItem.transform);
                rightHandItem = other.gameObject;
                rightHandItem.GetComponent<Rigidbody>().useGravity = false;
                rightHandItem.transform.position = rightHandSpawnPos.transform.position;
                rightHandItem.transform.rotation = rightHandSpawnPos.transform.rotation;
            }
        }
    }

    public void DropItem()
    {
        if (leftHandItem)
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                leftHandItem.transform.SetParent(null);
                leftHandItem.GetComponent<Rigidbody>().useGravity = true;
                leftHandItem = null;
            }        
        if(rightHandItem)
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                rightHandItem.transform.SetParent(null);
                rightHandItem.GetComponent<Rigidbody>().useGravity = true;
                rightHandItem = null;
            }
    }
}
