using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlintlockHammer : MonoBehaviour {

    public GameObject currentHand;
    
    private FlintLock flintLock { get { return GameObject.Find("FlintLock").GetComponent<FlintLock>(); } }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Hand")
            currentHand = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Hand" && flintLock.firing == false)
            currentHand = null;
    }
}
