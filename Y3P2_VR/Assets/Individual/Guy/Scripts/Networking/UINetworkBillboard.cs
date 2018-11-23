using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINetworkBillboard : MonoBehaviour
{
    private Transform _Target;

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.thisPlayer != null) {
            _Target = PlayerManager.thisPlayer.player_cam.player_Camera.transform;
            Quaternion.LookRotation(transform.position - _Target.position);
        }
    }
}
