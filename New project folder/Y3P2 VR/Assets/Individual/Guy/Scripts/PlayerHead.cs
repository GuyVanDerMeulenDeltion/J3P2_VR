using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour {

    private static float rayRange = 12;

    public bool isWielding = false;
    public Menu menu;

    private bool checkedMenu = false;

    public void Update() {
        if (isWielding == true)
            menu.SetMenuState(false);
    }

    private void OnTriggerEnter(Collider _O) {
        if (_O.GetComponent<Menu>())
            menu.SetMenuState(true);
    }

    private void OnTriggerExit(Collider _O) {
        if (_O.GetComponent<Menu>())
            menu.SetMenuState(false);
    }
}
