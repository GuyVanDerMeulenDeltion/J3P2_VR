using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Interactables : MonoBehaviourPunCallbacks {

    [SerializeField] private bool useSkinned = false;
    [SerializeField] private MeshRenderer render;
    [SerializeField] private SkinnedMeshRenderer s_Render;

    public Vector3 pickupRotation;
    public Vector3 pickupPosition;

    public bool isInteracting = false;

    private void Awake()
    {
        if (useSkinned == true && s_Render != null)
            s_Render.materials[0].SetFloat("_Thickness", 0);
        else if (render != null)
            render.materials[0].SetFloat("_Thickness", 0);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Hand" && isInteracting == false)
        {
            if (useSkinned == true && s_Render != null)
                s_Render.materials[0].SetFloat("_Thickness", 1);
            else if(render != null)
                render.materials[0].SetFloat("_Thickness", 1);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Hand")
        {
            if (useSkinned == true && s_Render != null)
                s_Render.materials[0].SetFloat("_Thickness", 0);
            else if (render != null)
                render.materials[0].SetFloat("_Thickness", 0);
        }
    }

    public virtual void Interact()
    {
        isInteracting = true;
        if (useSkinned == true && s_Render != null)
            s_Render.materials[0].SetFloat("_Thickness", 0);
        else if (render != null)
            render.materials[0].SetFloat("_Thickness", 0);
    }

    public virtual void DeInteract()
    {
        isInteracting = false;
    }
}
