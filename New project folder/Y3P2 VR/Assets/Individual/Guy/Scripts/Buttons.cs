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
    
    private void Awake() {
        allButtons.Add(this);
    }

    public void Update() {
        if(canInteract == true) {
            if (Input.GetKeyDown(KeyCode.L))
                Activate();
        }
    }

    private void OnTriggerEnter(Collider _O) {
        SetCurrentButton();
    }

    private void OnTriggerExit(Collider _O) {
        DeactivateCurrentButton();
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

    public void Activate() {
        function.Invoke();
        print("Activated function...");
    }

    public static void DeactivateAllButtons() {
        if (allButtons == null) return;
        foreach (Buttons _Button in allButtons) {
            _Button.thisAnim.enabled = false;
            _Button.outline.enabled = false;
        }
    }
}
