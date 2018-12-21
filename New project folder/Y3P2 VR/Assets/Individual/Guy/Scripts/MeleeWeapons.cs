using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeleeWeapons : MonoBehaviourPunCallbacks {

    //First we'll need a couple of variables to do the calculation.
    Quaternion rotationLast; //The value of the rotation at the previous update
    Quaternion rotationDelta; //The difference in rotation between now and the previous update

    //Initialize rotationLast in start, or it will cause an error
    void Start() {
        rotationLast = transform.rotation;
    }

    void Update() {
        //Update both variables, so they're accurate every frame.
        rotationDelta = Quaternion.Euler(transform.rotation.eulerAngles - rotationLast.eulerAngles);
        rotationLast = transform.rotation;

    }

    //Ta daa!
    public Vector3 angularVelocity {
        get {
            return rotationDelta.eulerAngles;
        }
    }
}
