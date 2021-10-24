using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParticle : MonoBehaviour
{
    public Vector3 movementVector;
    public Vector3 sumOfCharge;

    public LineRenderer lineRenderer;

    private Data worldData;
   
    private double e0;
    private float constantChange;
    private int lineCount = 1;
    private bool drawAndMove = true;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        worldData = GameObject.Find("World").GetComponent<Data>();
        lineRenderer.positionCount = lineCount;
        lineRenderer.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (drawAndMove) 
        {
            MoveAndDraw();
        }
    }

    private void MoveAndDraw() 
    {
        while (true)
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

            movementVector = sumOfCharge * Time.deltaTime * 100f;

            var Coordinates = new List<float> { Mathf.Abs(movementVector.x), Mathf.Abs(movementVector.y), Mathf.Abs(movementVector.z) };

            float maxValue = Mathf.Max(Coordinates.ToArray());
            if (maxValue == 0)
            {
                maxValue = 1 / 1000;
            }

            int position = Coordinates.IndexOf(maxValue);

            //rb.velocity = movementVector;

            Vector3 balancesMovement = movementVector / Mathf.Abs(maxValue * 5);

            transform.position += balancesMovement;

            lineCount++;

            if (lineCount < 450) //2450 originally
            {
                lineRenderer.positionCount = lineCount;
                lineRenderer.SetPosition(lineCount - 1, balancesMovement + Cor);
            }
            else
            {
                drawAndMove = false;
                Debug.Log(lineCount);
                GetComponent<MeshRenderer>().enabled = false;
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var chargeScript = collision.gameObject.GetComponent<ObjectAttributes>();
        
        if (chargeScript)
        {
            if (chargeScript.charge == -1)
            {
                lineCount = 2500;
            }
        }
    }
}
