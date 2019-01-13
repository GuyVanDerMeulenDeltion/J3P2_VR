using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class PlayerHeadIK : MonoBehaviour
{

    protected Animator animator;
    [Header("Rotate body only on Y axis")]
    public bool ikActive = true;
    [Header("This to rotate the head in other axis")]
    public Transform lookObj = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // Set the right hand target position and rotation, if one has been assigned


            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {

                animator.SetLookAtWeight(0);
            }
        }
    }
}