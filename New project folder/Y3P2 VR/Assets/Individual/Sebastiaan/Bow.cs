using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Interactables
{


    private void Update()
    {
        if(transform.GetChild(2).GetComponent<BowString>().firing == false)
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;

    }
}

