using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buttons : DrawOutline {

    [SerializeField]private UnityEvent function;
    private bool canInteract = false;

    protected override void OnTriggerEnter(Collider _O) {
        base.OnTriggerEnter(_O);
        canInteract = true;
    }

    protected override void OnTriggerExit(Collider _O) {
        base.OnTriggerExit(_O);
        canInteract = false;
    }

    private void Activate() {
        function.Invoke();
    }
}
