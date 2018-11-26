using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentaZone : MonoBehaviour
{
    public enum UpdatedSize {
        Small,
        Big,
        Bigger,
        Biggest
    }

    internal static UpdatedSize state;

    public static float _GROWTIME = 1;
    private static float _MAXSCALE = 10;

    private Vector3 _FirstStage = new Vector3(3, 3, 3);
    private Vector3 _SecondStage = new Vector3(6, 6, 6);
    private Vector3 _FinalStage = new Vector3(10, 10, 10);


    public static void ResetGrowZone() {
        state = UpdatedSize.Small;
    }

    private void OnTriggerStay(Collider _O) {
        if(_O.transform.tag == "Player") {
            BasicMovement _Player = _O.transform.GetComponent<BasicMovement>();
  
            if (ScaleApproximate(_O.transform.localScale, _FirstStage) && state == UpdatedSize.Small) {
                _Player.SetNameDuringSession("[Level 126] Tyler the Grown");
                if(_Player.enabled)
                state = UpdatedSize.Big;
                return;
            } else

            if (ScaleApproximate(_O.transform.localScale, _SecondStage) && state == UpdatedSize.Big) {
                _Player.SetNameDuringSession("[Level 420] Tyler the Destroyer Of Worlds");
                if(_Player.enabled)
                state = UpdatedSize.Bigger;
                return;
            } else

            if (ScaleApproximate(_O.transform.localScale, _FinalStage) && state == UpdatedSize.Bigger) {
                _Player.SetNameDuringSession("[Level 1500] Tyler the Invincible");
                if(_Player.enabled)
                state = UpdatedSize.Biggest;
                return;
            } 
            
            if(state != UpdatedSize.Biggest)
            _O.transform.localScale += new Vector3(_GROWTIME * Time.deltaTime, _GROWTIME * Time.deltaTime, _GROWTIME * Time.deltaTime);
        }
    }

    private bool CheckIfVectorIsMaxed(float _VectorAxis) {
        if (_VectorAxis > (_FinalStage.x + 1))
            return true;

        return false;
    }

    private static bool ScaleApproximate(Vector3 _Value, Vector3 _ValueTwo) {
        float[] _ValueFloats = { _Value.x, _Value.y, _Value.z };
        float[] _ValueFloatsTwo = { _ValueTwo.x, _ValueTwo.y, _ValueTwo.z };

        for(int i = 0; i < _ValueFloats.Length; i++) {
            if (_ValueFloats[i] < _ValueFloatsTwo[i])
                return false;
        }

        return true;
    }
}
