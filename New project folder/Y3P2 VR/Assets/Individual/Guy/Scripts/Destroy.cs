using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroy : MonoBehaviour {

    public bool networkDestroy = true;

    public GameObject toDestroy;
    [SerializeField]private float timeTillDestroy = 5;

	// Use this for initialization
	private void OnEnable () {
        StartCoroutine(DestroyObject());
        if (toDestroy == null)
            toDestroy = gameObject;
            Debug.LogWarning(gameObject.name +" couldn't find a object to destroy, so it chose itself.");
	}
	
    private IEnumerator DestroyObject() {
        yield return new WaitForSeconds(timeTillDestroy);
        if (!PhotonNetwork.IsConnected || networkDestroy == false)
            Destroy(toDestroy);
        else if (networkDestroy == true)
            PhotonNetwork.Destroy(toDestroy);
    }
}
