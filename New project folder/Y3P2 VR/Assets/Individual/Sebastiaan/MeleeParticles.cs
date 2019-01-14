using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeParticles : MonoBehaviour {

    public TrailRenderer trailrenderer { get { return transform.GetComponent<TrailRenderer>(); } }

    protected Vector3 oldPos;
    protected Vector3 newPos;
    protected Vector3 customVelocity;

    void Start () {
		
	}
	
	void Update () {
        newPos = transform.parent.parent.position;
        customVelocity = (oldPos - newPos) / Time.deltaTime;
        oldPos = newPos;

        trailrenderer.widthMultiplier = Mathf.Clamp(customVelocity.magnitude*20, 1, 20);
	}
}
