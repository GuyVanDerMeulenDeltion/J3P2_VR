using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINetworkBillboard : MonoBehaviour
{
    private Transform _Target;

    // Start is called before the first frame update
    void Start()
    {
        _Target = PlayerManager.thisPlayer.player_cam.player_Camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(_Target != null)
        Quaternion.LookRotation(transform.position - _Target.position);
    }
}
