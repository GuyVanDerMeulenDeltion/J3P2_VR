using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterReload : MonoBehaviour
{

    public GameObject currentHand;
    private Repeater repeater { get { return transform.root.GetComponent<Repeater>(); } }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Hand")
        {
            currentHand = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Hand" && repeater.firing == false)
            currentHand = null;
    }
}
