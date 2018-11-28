using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutline : MonoBehaviour {

    public static Material outline;

    [Header("Selection Visual Settings:")]
    public bool passItAsNewStatic = false;
    public Material newSelectionMat;

    private MeshRenderer thisRender { get { return GetComponent<MeshRenderer>(); } }

    private void Awake() {
        SetNewMaterial();
    }

    private void SetNewMaterial() {
        if(passItAsNewStatic == true) {
            outline = newSelectionMat;
        }
    }

    private void OnTriggerStay(Collider _O) {
        if(_O.transform.tag == "Hand") {
            Material[] _Materials = {thisRender.material, outline};
        }
    }

    private void OnTriggerExit(Collider _O) {
        if (_O.transform.tag == "Hand") {
            Material[] _Materials = {thisRender.materials[0]};
            thisRender.materials = _Materials;
        }
    }
}
