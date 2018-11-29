using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour {

    public static float launchForce = 10;
    public static float timeAfterCollision = 5;

    public ParticleSystem fadeParticles;

    private int dir;

    private bool collided = false;

    // Use this for initialization
    private void Start () {
        fadeParticles.Stop();
        dir = Random.Range(0, 3);

        switch (dir) {
            case 0:
                GetComponent<Rigidbody>().AddForce(Vector3.up * launchForce, ForceMode.Impulse);
                break;
            case 1:
                GetComponent<Rigidbody>().AddForce(Vector3.right * launchForce, ForceMode.Impulse);
                break;
            case 2:
                GetComponent<Rigidbody>().AddForce(Vector3.down * launchForce, ForceMode.Impulse);
                break;
            case 3:
                GetComponent<Rigidbody>().AddForce(-Vector3.right * launchForce, ForceMode.Impulse);
                break;

        }
	}

    private void OnCollisionEnter(Collision _C) {
        if(collided == false) {
            collided = true;
            StartCoroutine(ActivateAnimator());
        }
    }

    private IEnumerator ActivateAnimator() {
        yield return new WaitForSeconds(timeAfterCollision);
        GetComponent<Animator>().enabled = true;
    }

    public void SetFinal() {
        fadeParticles.transform.position = transform.position;
        fadeParticles.Play();
    }
}
