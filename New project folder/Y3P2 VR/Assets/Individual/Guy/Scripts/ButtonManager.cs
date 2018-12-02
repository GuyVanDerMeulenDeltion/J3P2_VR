using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    public static ButtonManager buttonsManager;

    [Header("UI Settings:")]
    [SerializeField]private Animator optionsAnim;
    public GameObject optionsMenu;

    private void Awake() {
        if (buttonsManager != null) Destroy(this);
        buttonsManager = this;
    }

    public void OpenOptionsMenu() {
        optionsMenu.SetActive(true);
        optionsAnim.SetBool("Open", !optionsAnim.GetBool("Open"));
    }
}
