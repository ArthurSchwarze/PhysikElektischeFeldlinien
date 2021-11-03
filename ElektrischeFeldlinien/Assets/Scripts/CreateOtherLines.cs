using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOtherLines : MonoBehaviour
{
    private Data worldData;
    private int lineCount = 1;

    public LineRenderer lineRenderer;

    public Material raycastColour;
    public float lineThickness;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        worldData = GameObject.Find("World").GetComponent<Data>();

        lineRenderer.positionCount = lineCount;
        lineRenderer.SetPosition(0, transform.position);

        lineThickness = worldData.lineThickness;
        raycastColour = worldData.rayCastColour;

        lineRenderer.material = raycastColour;
        lineRenderer.startWidth = lineThickness;

        MoveAndDraw();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveAndDraw()
    {
        while (true)
        {
            Vector3 sumOfCharge = Vector3.zero;

            double e0 = worldData.e0;
            float constantChange = (float)(1 / (4 * e0 * Mathf.PI));

            Vector3 Cor = transform.position;

            for (int i = 0; i < worldData.objectCount; i++)
            {
                ObjectAttributes chargedObject = worldData.chargedObjects[i];
                float charge = chargedObject.charge;

                Vector3 CorChar = chargedObject.transform.position;
                Vector3 CorDiff = Cor - CorChar;

                sumOfCharge += ((charge) / Mathf.Pow((Mathf.Pow(CorDiff.x, 2f) + Mathf.Pow(CorDiff.y, 2f) + Mathf.Pow(CorDiff.z, 2f)), 3f / 2f)) * new Vector3(CorDiff.x, CorDiff.y, CorDiff.z);
            }

            Vector3 movementVector = sumOfCharge * Time.deltaTime * 30f;

            var Coordinates = new List<float> { Mathf.Abs(movementVector.x), Mathf.Abs(movementVector.y), Mathf.Abs(movementVector.z) };

            float maxValue = Mathf.Max(Coordinates.ToArray());
            if (maxValue == 0)
            {
                maxValue = 1 / 1000;
            }

            int position = Coordinates.IndexOf(maxValue);

            //rb.velocity = movementVector;

            Vector3 balancesMovement = movementVector / Mathf.Abs(maxValue * 5);

            //
            //balancesMovement = new Vector3(-balancesMovement.z, balancesMovement.y, -balancesMovement.x);
            balancesMovement = Quaternion.Euler(0, -90, 0) * balancesMovement;
            //

            transform.position += balancesMovement;

            lineCount++;

            foreach (ObjectAttributes circle in worldData.chargedObjects)
            {
                Vector3 objectDistance = transform.position - circle.transform.position;
                if (Mathf.Abs(objectDistance.magnitude) < circle.GetComponent<SphereCollider>().radius && Mathf.Sign(circle.GetComponent<ObjectAttributes>().charge) == -1)
                {
                    lineRenderer.positionCount = lineCount;
                    lineRenderer.SetPosition(lineCount - 1, balancesMovement + Cor);

                    //Debug.Log(Mathf.Abs(objectDistance.magnitude));
                    GetComponent<MeshRenderer>().enabled = false;

                    circle.GetComponent<ObjectAttributes>().collectPoints(transform.position);
                    worldData.totalNormalLines--;

                    return;
                }
            }


            if (lineCount < 2000) //2450 originally
            {
                lineRenderer.positionCount = lineCount;
                lineRenderer.SetPosition(lineCount - 1, balancesMovement + Cor);
            }
            else
            {
                //Debug.Log(lineCount);
                GetComponent<MeshRenderer>().enabled = false;
                worldData.totalNormalLines--;
                return;
            }
        }
    }
}
