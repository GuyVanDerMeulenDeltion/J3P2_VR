using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR;
using Valve.VR;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    private bool leftHandOccupied;
    private bool rightHandOccupied;

    public GameObject leftHandSpawnPos {  get {  return (transform.GetChild(0).gameObject); } }
    public GameObject rightHandSpawnPos {  get { return (transform.GetChild(1).gameObject); } }

    public float rightHandAxis { get { return SteamVR_Input._default.inActions.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand); } }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "interactible")
        {
            if (rightHandAxis > 0.85f)
            {

            }
        }
    }
}
