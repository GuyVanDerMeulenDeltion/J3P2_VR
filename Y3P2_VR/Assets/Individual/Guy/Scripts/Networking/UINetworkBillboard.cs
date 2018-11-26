using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINetworkBillboard : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.thisPlayer.camera != null) {
            print("yes");
            transform.rotation = Quaternion.LookRotation(PlayerManager.thisPlayer.camera.transform.position - transform.position);
        }
    }
}
