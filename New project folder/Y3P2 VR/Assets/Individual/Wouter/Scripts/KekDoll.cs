using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KekDoll : MonoBehaviour {

    private Animator myanim;
    private Rigidbody rb;


    public bool doRagdoll;

	// Use this for initialization
	void Start ()
    {
        myanim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(doRagdoll)
        {

            rb.constraints = RigidbodyConstraints.None;
            myanim.enabled = false;
        }
        else
        {

            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            myanim.enabled = true;
            Vector3 desiredX = new Vector3(0, transform.localEulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(desiredX), 1f / 0.4f * Time.deltaTime);
            //Vector3 desiredYpos = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            //transform.position = Vector3.Lerp(transform.rotation, Vector3(desiredYpos), 1f / 0.2f * Time.deltaTime);

        }
	}
}
