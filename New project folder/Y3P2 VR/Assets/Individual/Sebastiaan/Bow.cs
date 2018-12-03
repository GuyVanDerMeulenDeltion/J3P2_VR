using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Interactables {

    private void Update()
    {
        transform.localEulerAngles = pickupOffset;
    }


    public void DrawArrow()
    {

    }
}
