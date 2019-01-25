using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeleeWeapons : Interactables {

    //First we'll need a couple of variables to do the calculation.
    protected Quaternion rotationLast; //The value of the rotation at the previous update
    protected Quaternion rotationDelta; //The difference in rotation between now and the previous update

    protected Vector3 oldPos;
    protected Vector3 newPos;
    protected Vector3 customVelocity;
    protected Vector3 customAngularVelocity;

    //Initialize rotationLast in start, or it will cause an error
    protected virtual void Start() {
        this.enabled = false;
        rotationLast = transform.rotation;
    }

    protected virtual void Update() {
        SetCustomVelocity();
    }

    protected void SetCustomVelocity() {
        newPos = transform.position;
        customVelocity = (oldPos - newPos) / Time.deltaTime;
        oldPos = newPos;

        rotationDelta = Quaternion.Euler(transform.rotation.eulerAngles - rotationLast.eulerAngles);
        customAngularVelocity = rotationDelta.eulerAngles / Time.deltaTime;
        rotationLast = transform.rotation;
    }

    //Ta daa!
    protected Vector3 angularVelocity {
        get {
            return rotationDelta.eulerAngles;
        }
    }

    protected float CalculateKinetics() {
        return Mathf.Pow(customVelocity.magnitude, 2) * 0.5f;
    }
}
