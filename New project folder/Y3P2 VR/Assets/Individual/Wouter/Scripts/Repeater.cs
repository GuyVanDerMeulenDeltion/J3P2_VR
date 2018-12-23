using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : MonoBehaviour {

    public Animator myAnim;
    private float triggerAxis;
    private float rechamberAxis;

    void Update()
    {
        triggerAxis = Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f);
        rechamberAxis = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 0.95f);

        myAnim.SetFloat("TriggerAxis", triggerAxis);
        myAnim.SetFloat("RechamberAxis", rechamberAxis);
    }
}
