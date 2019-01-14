﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour {

    private static float rotationTime = 8;
    [SerializeField]private float yAxisOffset = 0.6f;
    [SerializeField] private float offset = 0.6f;

    private void Update () {
        Vector3 _HeadRot = PlayerManager.thisPlayer.player_head.transform.eulerAngles;
        Vector3 _HeadPos = PlayerManager.thisPlayer.player_head.transform.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _HeadRot.y + 360, transform.eulerAngles.z);
        transform.position = new Vector3(_HeadPos.x, _HeadPos.y - yAxisOffset, _HeadPos.z);
	}
}
