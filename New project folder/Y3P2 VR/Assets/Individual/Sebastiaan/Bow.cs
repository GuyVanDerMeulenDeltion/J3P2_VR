using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Interactables
{
    public GameObject bowString;
    public new void OnEnable()
    {
        bowString.GetComponent<BowString>().enabled = true;
    }


    private void Update()
    {
        if(bowString.GetComponent<BowString>().firing == false)
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;
    }

    public override void OnDisable()
    {
        base.OnDisable();
       bowString.GetComponent<BowString>().enabled = false;
    }
}

