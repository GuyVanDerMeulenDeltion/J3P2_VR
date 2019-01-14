using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour {

    private static float rotationTime = 8;
	
	private void Update () {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z), rotationTime * Time.deltaTime);
	}
}
