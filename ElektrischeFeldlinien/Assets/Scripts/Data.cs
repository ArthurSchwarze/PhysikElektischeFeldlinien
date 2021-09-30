using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    // Start is called before the first frame update
    public double e0;
    
    public int objectCount;

    public ObjectAttributes[] chargedObjects;


    void Start()
    {
        e0 = 8.85 * Mathf.Pow(10, -12);

        chargedObjects = GameObject.FindObjectsOfType<ObjectAttributes>();
        //chargedObjects = Object
        objectCount = chargedObjects.Length;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
