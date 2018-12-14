using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitten : MonoBehaviour {

    private Animator myanim;

    private void Start()
    {
        myanim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Implementer notes
        //This controlls the mitten grabbing animation.
        //Replace " Input.GetAxis("Horizontal") " with Controller trigger axis.
        myanim.SetFloat("Timeline", Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f));
    }
}
