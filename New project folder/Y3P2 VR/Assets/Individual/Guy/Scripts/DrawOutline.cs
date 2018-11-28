using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutline : MonoBehaviour {

    private MeshRenderer thisRender { get { return GetComponent<MeshRenderer>(); } }

    private void OnTriggerStay(Collider _O) {
        if(_O.transform.tag == "Hand") {
            print(thisRender.materials[1]);
            thisRender.materials[1].SetFloat("_Thickness", 1);
        }
    }

    private void OnTriggerExit(Collider _O) {
        if (_O.transform.tag == "Hand") {
            print("no");
            thisRender.materials[1].SetFloat("_Thickness", 0);
        }
    }
}
