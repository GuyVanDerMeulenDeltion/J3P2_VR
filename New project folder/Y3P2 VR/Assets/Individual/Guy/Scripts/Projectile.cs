using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float baseDamageOutput = 20;
    public bool shouldCalcWithImpForce = false;

    [SerializeField]private Rigidbody thisBody;

    private void OnCollisionEnter(Collision _C) {
        
    }

    public float KineticEnergy(Rigidbody rb) {
        return baseDamageOutput * rb.mass * Mathf.Pow(rb.velocity.magnitude, 2);
    }
}
