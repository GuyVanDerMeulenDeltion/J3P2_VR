using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour {

    private static float rayRange = 4;

    public LayerMask menuMask;

    private RaycastHit hit;

    private bool checkedAllMenus = false;

	// Update is called once per frame
	private void Update () {
        CastRay();
	}

    //Checks if it has hit a menu type object
    private void CastRay() {
        Debug.DrawRay(transform.position, transform.forward * rayRange);
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayRange, menuMask)) {
            print("y");
            checkedAllMenus = false;
            hit.transform.GetComponent<Menu>().SetMenuState(true);
            return;
        }

        if(checkedAllMenus == false) {
            print("Closing menu...");
            if(Menu.currentMenus != null)
                foreach(Menu _Menu in Menu.currentMenus) {
                _Menu.SetMenuState(false);
                checkedAllMenus = true;
            }
        }
    }
}
