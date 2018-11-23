using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] private bool calledRay = false;
    [SerializeField] private PlayerMenu menu;

    private RaycastHit hit;

    private void Update() {
        CastSphereRayForMenu();
    }

    private void CastSphereRayForMenu() {
        menu.LoadUI(CheckForMenu());             
    }

    private bool CheckForMenu() {
        if(Physics.SphereCast(transform.position, 0.2f, Vector3.forward, out hit)) {
            if(hit.transform.GetComponentInChildren<PlayerMenu>()) {
                return true;
            }
        }

        return false;
    }
}
