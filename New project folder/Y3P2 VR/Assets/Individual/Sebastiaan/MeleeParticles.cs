using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeParticles : MonoBehaviour {

    public TrailRenderer trailrenderer { get { return transform.GetComponent<TrailRenderer>(); } }

    protected Vector3 oldPos;
    protected Vector3 newPos;
    protected Vector3 customVelocity;

    public float minTrailSize;
    public float maxTrailSize;
    public float trailMultiplier;

    void Start () {
		
	}
	
	void Update () {
        if (transform.parent.parent != null)
        {
            newPos = transform.parent.parent.position;
            customVelocity = (oldPos - newPos) / Time.deltaTime;
            oldPos = newPos;
        }

        if (customVelocity.magnitude > 1)
        {
            trailrenderer.widthMultiplier = Mathf.Clamp(customVelocity.magnitude * trailMultiplier, minTrailSize, maxTrailSize);
            trailrenderer.emitting = true;

        }
        else
            trailrenderer.emitting = false;
	}
}
