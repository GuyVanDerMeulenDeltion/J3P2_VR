using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Interactables : MonoBehaviourPunCallbacks {

    private DrawOutline drawOutline;
    public Vector3 pickupRotation;
    public Vector3 pickupPosition;

    public void Awake()
    {
        drawOutline = new DrawOutline();
    }

	public virtual void Interact()
    {
        drawOutline.transform.GetComponent<MeshRenderer>().material.SetFloat("_Thickness", 0);
        drawOutline.enabled = false;
    }

    public virtual void DeInteract()
    {
        drawOutline.enabled = true;
    }
}
