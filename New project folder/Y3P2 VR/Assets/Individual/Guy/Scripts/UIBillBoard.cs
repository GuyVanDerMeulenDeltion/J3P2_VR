using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillBoard : MonoBehaviour {

    private Transform _Target;
	
	// Update is called once per frame
	private void Update () {
        Rotate();
	}

    private void Rotate() {
        if(_Target == null) {
            if(PlayerManager.thisPlayer != null)
                if(PlayerManager.thisPlayer.camera != null) {
                _Target = PlayerManager.thisPlayer.camera.transform;
            }
        }

        if(_Target != null)
        transform.rotation = Quaternion.LookRotation(_Target.position - transform.position);
    }
}
