using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Particles : MonoBehaviour {

    private void OnEnable() {
        GetComponent<ParticleSystem>().Play();
    }
}
