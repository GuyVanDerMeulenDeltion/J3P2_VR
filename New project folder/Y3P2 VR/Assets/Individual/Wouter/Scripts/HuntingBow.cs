using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingBow : MonoBehaviour
{
    public Transform stringConnection1;
    public Transform stringConnection2;
    public Transform stringConnection3;
    private LineRenderer myline;
    private Animator myAnim;

    public void Start()
    {
        myline = GetComponent<LineRenderer>();
        myline.enabled = true;
        myAnim = GetComponent<Animator>();
    }

    public void Update()
    {
        if(transform.GetComponent<Bow>().enabled)
        UpdateStringConnections();
    }

    public void UpdateStringConnections()
    {
        myline.SetPosition(0, stringConnection1.position);
        myline.SetPosition(1, stringConnection2.position);
        myline.SetPosition(2, stringConnection3.position);
    }
}
