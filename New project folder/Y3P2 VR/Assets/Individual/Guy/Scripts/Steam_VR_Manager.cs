using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Steam_VR_Manager : MonoBehaviour {

    public static Steam_VR_Manager steamManager;

    public SteamVR_Render render;

	private void Awake () {
        if (steamManager != null) Destroy(this);
        steamManager = this;
	}
	
    public void EnableRender() {
        render.enabled = true;
    }
}
