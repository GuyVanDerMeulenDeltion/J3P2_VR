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
            if (pickUpObject.GetComponent<Interactables>() != null)
                pickUpObject.GetComponent<Interactables>().enabled = true;
            pickUpObject.GetComponent<Transform>().position = controller.transform.position;
            pickUpObject.GetComponent<Transform>().rotation = controller.transform.rotation;
            controller.GetComponent<Controller>().item = pickUpObject;
        }
    }

    public static void ThrowObject(GameObject controller, GameObject throwable, SteamVR_Behaviour_Pose trackedObj)
    {
        if (throwable != null && controller != null)
        {
            controller.GetComponent<Controller>().item = null;
            throwable.transform.SetParent(null);
            if (throwable.GetComponent<Interactables>() != null)
                throwable.GetComponent<Interactables>().enabled = false;
            throwable.GetComponent<Rigidbody>().isKinematic = false;
            throwable.GetComponent<Rigidbody>().useGravity = true;
            throwable.GetComponent<Rigidbody>().velocity = trackedObj.GetVelocity();
            throwable.GetComponent<Rigidbody>().angularVelocity = trackedObj.GetAngularVelocity();
        }
    }
}
