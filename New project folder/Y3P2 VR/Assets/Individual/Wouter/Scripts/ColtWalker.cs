using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColtWalker : MonoBehaviour {

    public Animator myAnim;
    private float triggerAxis;
    public int bulletsLoaded;

    void Update()
    {
        triggerAxis = Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 0.95f);

        myAnim.SetFloat("TriggerAxis", triggerAxis);
        myAnim.SetInteger("Bullets", bulletsLoaded);
    }

    public void EditBulletcount(int amount)
    {
        bulletsLoaded = bulletsLoaded + amount;
    }
}
