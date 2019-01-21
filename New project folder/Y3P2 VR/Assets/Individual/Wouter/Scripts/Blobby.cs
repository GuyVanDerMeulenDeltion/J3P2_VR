using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blobby : MonoBehaviour {

    [SerializeField] private Transform _Head;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 _HeadRot = _Head.transform.eulerAngles;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _HeadRot.y + 360, transform.eulerAngles.z);
    }
}
