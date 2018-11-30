using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{


    public GameObject leftHandSpawnPos { get { return (transform.GetChild(0).gameObject); } }
    public GameObject rightHandSpawnPos { get { return (transform.GetChild(1).gameObject); } }

    public static void PickObject(GameObject controller, GameObject pickUpObject, GameObject hasItem)
    {
        if (hasItem == null)
        {
            pickUpObject.transform.SetParent(controller.transform);
            pickUpObject.GetComponent<Rigidbody>().isKinematic = true;
            pickUpObject.GetComponent<Rigidbody>().useGravity = false;
            pickUpObject.GetComponent<Transform>().position = controller.transform.position;
            pickUpObject.GetComponent<Transform>().rotation = controller.transform.rotation;
            hasItem = pickUpObject;
        }
    }

    public static void ThrowObject(GameObject controller, GameObject throwable, SteamVR_Behaviour_Pose trackedObj)
    {
        throwable.transform.SetParent(null);
        throwable.GetComponent<Rigidbody>().isKinematic = false;
        throwable.GetComponent<Rigidbody>().useGravity = true;
        throwable.GetComponent<Rigidbody>().velocity = trackedObj.GetVelocity();
        throwable.GetComponent<Rigidbody>().angularVelocity = trackedObj.GetAngularVelocity();
        throwable = null;
    }
}
