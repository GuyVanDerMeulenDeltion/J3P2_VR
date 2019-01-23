using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Interactables
{
    public new void OnEnable()
    {
        transform.GetChild(2).GetComponent<BowString>().enabled = true;
    }


    private void Update()
    {
        if(transform.GetChild(2).GetComponent<BowString>().firing == false)
        transform.localEulerAngles = pickupRotation;
        transform.localPosition = pickupPosition;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        transform.GetChild(2).GetComponent<BowString>().enabled = false;
    }
}

