using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static float _SENSITIVITY = 150;

    [SerializeField] private Transform camera_Anchor;
                     public Camera player_Camera;

    private Vector3 eulerRotation;

    public void OnEnable() {
        player_Camera.enabled = true;
        eulerRotation = camera_Anchor.eulerAngles;
    }

    public void Update() {
        if (Input.GetButton("Fire2"))
            eulerRotation += new Vector3(IncrementRotation().x, IncrementRotation().y ,0);
        camera_Anchor.eulerAngles = eulerRotation;
    }

    private static Vector2 IncrementRotation() {
        Vector2 _NewInputs;
        _NewInputs.x = Input.GetAxis("Mouse Y") * _SENSITIVITY * Time.deltaTime;
        _NewInputs.y = Input.GetAxis("Mouse X") * _SENSITIVITY * Time.deltaTime;
        return _NewInputs;
    }
}
