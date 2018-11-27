using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    public static List<Menu> currentMenus = new List<Menu>();

    [SerializeField]private Animator thisAnim;

    private void Start() {
        currentMenus.Add(this);
    }

    public void SetMenuState(bool _State) {
        thisAnim.SetBool("Opened", _State);
    }
}
