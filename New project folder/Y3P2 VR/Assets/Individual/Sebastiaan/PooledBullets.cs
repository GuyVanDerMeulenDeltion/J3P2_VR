using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledBullets : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        tijdelijkebeweegfunctieAHHAHHAHAH();
    }

    void tijdelijkebeweegfunctieAHHAHHAHAH()
    {
        transform.transform.transform.transform.transform.Translate(Vector3.forward * 2 * Time.deltaTime);
    }
}
