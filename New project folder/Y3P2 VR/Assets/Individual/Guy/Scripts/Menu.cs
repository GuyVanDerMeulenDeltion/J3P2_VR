using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    public bool isActive = false;

    [SerializeField]private Animator thisAnim;
    [SerializeField]private Animator extraAnim;

    public void SetMenuState(bool _State) {
        thisAnim.SetBool("Open", _State);
        isActive = _State;

        if (_State == false && extraAnim != null) {
            extraAnim.SetBool("Open", _State);
            extraAnim.gameObject.SetActive(false);
            Buttons.DeactivateAllButtons();
        }

        if (_State == false) {
            Buttons.DeactivateAllButtons();
        }
    }
}
