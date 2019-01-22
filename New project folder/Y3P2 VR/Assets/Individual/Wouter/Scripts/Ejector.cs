using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ejector : MonoBehaviour {

    public Transform ejectionLocation;
    public GameObject obj;

    public void Eject()
    {
        GameObject g = Instantiate(obj, ejectionLocation.position, ejectionLocation.rotation);
        Destroy(g, 5);
        g.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up + Vector3.right * Random.Range(7, 15));
    }
}
