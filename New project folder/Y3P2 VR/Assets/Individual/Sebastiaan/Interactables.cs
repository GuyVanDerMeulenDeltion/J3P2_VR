using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<EnemyArrow>() && !GetComponent<Shield>())
        {
            if(transform.parent != null)
            {
                if(transform.parent.GetComponent<Controller>())
                {
                    transform.parent.GetComponent<Controller>().DropObject(true);
                    Destroy(collision.gameObject);
                    PlayerManager.thisPlayer.playerMain.SendMessageLocally("Oops, your weapon did an oopsie! ¯|_(ツ)_|¯");
                }
            }
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

    public void Haptic(float _Strength = 120)
    {
        if (transform.parent.GetComponent<Controller>().leftHand)
            transform.parent.GetComponent<Haptic>().Pulse(0.2f, 240, _Strength, SteamVR_Input_Sources.LeftHand);
        else
            transform.parent.GetComponent<Haptic>().Pulse(0.2f, 90, _Strength, SteamVR_Input_Sources.RightHand);
    }

    public void HapticSpecific(float _Strength, Controller _Cont = null) {
        if (_Cont != null) {
            if (_Cont.leftHand)
                _Cont.transform.GetComponent<Haptic>().Pulse(0.2f, 240, _Strength, SteamVR_Input_Sources.LeftHand);
            else
                _Cont.transform.GetComponent<Haptic>().Pulse(0.2f, 90, _Strength, SteamVR_Input_Sources.RightHand);
        }
    }
}
