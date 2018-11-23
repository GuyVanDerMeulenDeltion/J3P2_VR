using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public static float _FORCE = 40;
    public static float _DESTROYTIMER = 10;

    private Rigidbody thisbody;

    void Start()
    {
        StartCoroutine(DestroySelf());
        thisbody = GetComponent<Rigidbody>();
        thisbody.AddForce(-transform.right * _FORCE, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision _C) {
        if (_C.transform.tag == "Player") {
            if(_C.transform.GetComponent<BasicMovement>())
                if(_C.transform.GetComponent<BasicMovement>().enabled) {
                    _C.transform.GetComponent<BasicMovement>().GetDamaged();
                }
        }

        Destroy(gameObject);
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(_DESTROYTIMER);
        Destroy(gameObject);
    }
}
