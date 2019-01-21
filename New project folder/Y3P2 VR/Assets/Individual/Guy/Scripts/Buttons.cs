using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {

    private static List<Buttons> allButtons = new List<Buttons>();

    [SerializeField]private UnityEvent function;
    [SerializeField]private Outline outline;
    [SerializeField]private Animator thisAnim;
    private bool canInteract = false;

    internal GameObject hand;
    
    private void Awake() {
        allButtons.Add(this);
    }

    private void OnTriggerEnter(Collider _O) {
        if (_O.transform.tag == "Hand" && _O.gameObject != gameObject) {
            SetCurrentButton();
            hand = _O.gameObject;
        }
    }

    private void OnTriggerExit(Collider _O) {
        if (_O.transform.tag == "Hand" && _O.gameObject != gameObject) {
            DeactivateCurrentButton();
            hand = null;
        }
    }

    private void SetCurrentButton() {
        if (allButtons == null) return;

        foreach(Buttons _Button in allButtons) {
            if(_Button.thisAnim != null)
            _Button.thisAnim.enabled = false;
            _Button.outline.enabled = false;
        }

        thisAnim.enabled = true;
        canInteract = true;
        outline.enabled = true;
    }

    private void DeactivateCurrentButton() {
        thisAnim.enabled = false;
        canInteract = false;
        outline.enabled = false;
    }

    public static void Activate(GameObject _Hand) {
        foreach(Buttons _But in allButtons) {
            if (_But.canInteract && _But.hand == _Hand) {
                _But.function.Invoke();
                return;
            }
        }
        print("Activated function...");
    }

    public static void DeactivateAllButtons() {
        if (allButtons == null) return;
        foreach (Buttons _Button in allButtons) {
            if (_Button.thisAnim != null && _Button.outline.enabled == true)
            {
                _Button.thisAnim.enabled = false;
                _Button.outline.enabled = false;
            }
        }
    }
}
