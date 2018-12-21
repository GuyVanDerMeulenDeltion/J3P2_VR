using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitfield : MonoBehaviour {

    private Quaternion rotation;

    void Awake() {
        rotation = transform.rotation;
    }
    void LateUpdate() {
        transform.rotation = rotation;
    }
}
