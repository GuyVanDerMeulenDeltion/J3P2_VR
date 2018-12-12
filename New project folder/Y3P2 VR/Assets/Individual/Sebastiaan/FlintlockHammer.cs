using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlintlockHammer : MonoBehaviour {

    public GameObject currentHand;
    
    private FlintLock flintLock { get { return transform.parent.parent.parent.GetComponent<FlintLock>(); } }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Hand")
            currentHand = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Hand" && flintLock.firing == false &&transform.parent.parent.parent.GetComponent<FlintLock>().cocking == false)
            currentHand = null;
    }
}
