using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeParticles : MonoBehaviour {

    public TrailRenderer trailrenderer { get { return transform.GetComponent<TrailRenderer>(); } }

	void Start () {
		
	}
	
	void Update () {
        trailrenderer.widthMultiplier = Mathf.Clamp(transform.root.GetComponent<Rigidbody>().velocity.magnitude, 0.1f, 1);
	}
}
