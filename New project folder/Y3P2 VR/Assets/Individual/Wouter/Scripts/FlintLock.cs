using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlintLock : MonoBehaviour
{
    private Animator myanim;
    private float triggerTimeline;
    private bool readyForRecock;
    public float cooldown;
    private float currentCooldown;

    //public float triggerAxis;
    //public float recockAxis;
    void Start()
    {
        myanim = GetComponent<Animator>();
    }

    void Update()
    {
        //Notes for implementer:
        //Replace the following "Input.GetAxis("Horizontal")" with the HTC Vive Trigger on the current hand (min 0, max 1).
        myanim.SetFloat("TriggerAxis", Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f));


        if (currentCooldown < 0)
        {
            //Notes for implementer:
            //Replace the following "Input.GetAxis("Vertical")" with recocking action progress (min 0, max 1).
            myanim.SetFloat("RecockAxis", Mathf.Clamp(Input.GetAxis("Vertical"), 0, 0.95f));
        }

        if (currentCooldown >= 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void ChangeStateReadyToFire()
    {
        myanim.SetBool("Ready", true);
    }

    public void ApplyCooldown()
    {
        currentCooldown = cooldown;
    }
}