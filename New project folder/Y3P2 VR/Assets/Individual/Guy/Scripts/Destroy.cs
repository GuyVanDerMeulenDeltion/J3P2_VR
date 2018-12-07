using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroy : MonoBehaviour {

    [SerializeField]private float timeTillDestroy = 5;
    [SerializeField]private GameObject toDestroy;

	// Use this for initialization
	private void OnEnable () {
        StartCoroutine(DestroyObject());
        if (toDestroy == null)
            toDestroy = gameObject;
            Debug.LogWarning(gameObject.name +" couldn't find a object to destroy, so it chose itself.");
	}
	
    private IEnumerator DestroyObject() {
        yield return new WaitForSeconds(timeTillDestroy);
        if (!toDestroy.GetComponent<PhotonView>())
            Destroy(toDestroy);
        else
            PhotonNetwork.Destroy(toDestroy);
    }
}
