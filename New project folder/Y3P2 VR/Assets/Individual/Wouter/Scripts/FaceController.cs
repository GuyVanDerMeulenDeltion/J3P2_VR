using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour {

    public int currentEmotion;
    public float timer = 0;
    private float multiplier = 1;

    

 

    void Update()
    {


    }

    public void RandomFaces()
    {
        if (timer <= 0)
        {
            timer = 5;
            multiplier = Random.Range(0.75f, 1.5f);
        }
        else
        {
            timer -= Time.deltaTime * multiplier;
        }
    }


}
