using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour {

    public int currentEmotion = 1;
    public float timer = 0;
    private float multiplier = 1;
    public bool hurt;
    public bool dead;
    public KekDoll kekdollScript;

    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject obj5;

    public void Start()
    {
        UpdateFace();
        kekdollScript = GetComponentInParent<KekDoll>();
    }

    public void Update()
    {
        if(kekdollScript != null)
        {
            hurt = kekdollScript.doRagdoll;
        }
        
        if(hurt)
        {
            currentEmotion = 4;
            timer = 1;
            UpdateFace();
        }
        if (timer > 0)
        {
            if(!hurt)
            {
                timer -= Time.deltaTime;
            }

            
        }
        else
        {
            timer = 3;
            UpdateFace();
            currentEmotion = Random.Range(1, 3);
        }
        if(dead)
        {
            currentEmotion = 5;
            obj1.SetActive(false);
            obj2.SetActive(false);
            obj3.SetActive(false);
            obj4.SetActive(false);
            obj5.SetActive(true);
        }
    }

    public void UpdateFace()
    {
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
        obj5.SetActive(false);

        if(currentEmotion == 1)
        {
            obj1.SetActive(true);
        }
        if (currentEmotion == 2)
        {
            obj2.SetActive(true);
        }
        if (currentEmotion == 3)
        {
            obj3.SetActive(true);
        }
        if (currentEmotion == 4)
        {
            obj4.SetActive(true);
        }
        if (currentEmotion == 5)
        {
            obj5.SetActive(true);
        }
    }
}
