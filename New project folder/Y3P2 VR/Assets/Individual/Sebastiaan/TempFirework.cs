using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFirework : MonoBehaviour {

    public bool enable;
    public GameObject firework;
    private float timer = 1.5f;

    private void Update()
    {
        if (enable)       
            timer -= 1 * Time.deltaTime;
        
        if (timer <= 0)
        {
            Instantiate(firework, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
