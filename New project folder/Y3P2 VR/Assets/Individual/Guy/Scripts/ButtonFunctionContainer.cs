using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionContainer : MonoBehaviour {

    [Header("Option Menu:")]
    public Animator options;

    private bool openedOptions = false;

    public void OpenOptionsMenu() {
        if(openedOptions == false) {
            openedOptions = true;
            options.SetBool("Open", true);
        } else if(openedOptions == true) {
            openedOptions = false;
            options.SetBool("Open", false);
        }

    }

    public void Quit() {
        print("Quit");
        Application.Quit();
    }

    public void JoinGame() {
        print("Soon to be added.");
    }
}
