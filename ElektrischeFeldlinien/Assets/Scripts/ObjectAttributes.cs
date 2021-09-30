using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttributes : MonoBehaviour
{
    // Start is called before the first frame update

    private float scale;
    public float charge = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scale = Mathf.Pow(Mathf.Abs(charge), 1f / 3f);
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }
}
