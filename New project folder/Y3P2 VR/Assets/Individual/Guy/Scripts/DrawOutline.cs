using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutline : MonoBehaviour {

    private MeshRenderer thisRender { get { return GetComponent<MeshRenderer>(); } }

    private void OnTriggerStay(Collider _O) {
        if(_O.transform.tag == "Hand") {
            thisRender.material.SetFloat("Thickness", 4);
        }
    }

    private void OnTriggerExit(Collider _O) {
        if (_O.transform.tag == "Hand") {
            thisRender.material.SetFloat("Thickness", 0);
        }
    }
}
