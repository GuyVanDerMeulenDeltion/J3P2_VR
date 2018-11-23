using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryParticleInstance : MonoBehaviour
{
    public static float _DESTROYTIMER = 2;

      private IEnumerator Destroy() {
        yield return new WaitForSeconds(_DESTROYTIMER);
        Destroy(gameObject);
    }
}
