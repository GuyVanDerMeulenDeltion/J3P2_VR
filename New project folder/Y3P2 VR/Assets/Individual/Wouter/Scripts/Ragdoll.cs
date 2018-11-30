using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {

    public bool doRagdoll;
    private Animator myAnim;
    private Rigidbody myRB;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(doRagdoll)
        {
            myAnim.enabled = false;
            myRB.constraints = RigidbodyConstraints.None;
        }
        else
        {
            myAnim.enabled = true;
            myRB.constraints = RigidbodyConstraints.FreezeAll;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 1f / 1 * Time.deltaTime * 2);
            //transform.position = Vector3.Lerp(transform.position, Physics.Raycast( )
        }
    }
}
