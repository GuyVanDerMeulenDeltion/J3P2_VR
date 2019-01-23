using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptic : MonoBehaviour {

    [SerializeField] private SteamVR_Action_Vibration haptic;

    internal void Pulse(float _Duration, float _Frequency, float _Amplitude, SteamVR_Input_Sources _Source)
    {
        haptic.Execute(0, _Duration, _Frequency, _Amplitude, _Source);
    }    
}
