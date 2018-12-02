using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    public static List<Menu> currentMenus = new List<Menu>();

    [SerializeField]private Animator thisAnim;
    [SerializeField]private Animator extraAnim;

    private void Start() {
        currentMenus.Add(this);
    }

    public void SetMenuState(bool _State) {
        thisAnim.SetBool("Open", _State);

        if (_State == false && extraAnim != null) {
            extraAnim.SetBool("Open", _State);
            extraAnim.gameObject.SetActive(false);
        }

        if (_State == false) {
            Buttons.DeactivateAllButtons();
        }
    }
}
