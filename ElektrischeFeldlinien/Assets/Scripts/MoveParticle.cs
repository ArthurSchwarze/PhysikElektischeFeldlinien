using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParticle : MonoBehaviour
{
    public Vector3 particleMovement;
    public Vector3 sumOfCharge;

    public Rigidbody rb;

    private Data worldData;
   
    private double e0;
    private float constantChange;

    // Start is called before the first frame update
    void Start()
    {
        worldData = GameObject.Find("World").GetComponent<Data>();
    }

    // Update is called once per frame
    void Update()
    {
        sumOfCharge = Vector3.zero;
        
        e0 = worldData.e0;
        constantChange = (float)(1 / (4 * e0 * Mathf.PI));

        Vector3 Cor = transform.position;

        for (int i = 0; i < worldData.objectCount; i++) 
        {
            ObjectAttributes chargedObject = worldData.chargedObjects[i];
            float charge = chargedObject.charge;

            Vector3 CorChar = chargedObject.transform.position;
            Vector3 CorDiff = Cor - CorChar;

            sumOfCharge += ((charge) / Mathf.Pow((Mathf.Pow(CorDiff.x, 2f) + Mathf.Pow(CorDiff.y, 2f) + Mathf.Pow(CorDiff.z, 2f)), 3f / 2f)) * new Vector3(CorDiff.x, CorDiff.y, CorDiff.z);
        }
       
        Vector3 movementVector = constantChange * sumOfCharge * Time.deltaTime / 1000000;

        rb.velocity = movementVector;


        //particleMovement = 
    }
}
