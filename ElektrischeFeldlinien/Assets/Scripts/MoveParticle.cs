using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParticle : MonoBehaviour
{
    //Moves points to create the field lines
    //Only moves one point along it's path and deletes it on a hit with a pole

    public Vector3 movementVector;
    public Vector3 sumOfCharge;

    public LineRenderer lineRenderer;

    private Data worldData;
   
    private double e0;
    private float constantChange;
    private int lineCount = 1;

    void Awake()
    {
        //Pulls world data and the used line renderer
        lineRenderer = transform.GetComponent<LineRenderer>();
        worldData = GameObject.Find("World").GetComponent<Data>();

        lineRenderer.positionCount = lineCount;
        lineRenderer.SetPosition(0, transform.position);

        lineRenderer.positionCount = lineCount;
    }

    public void MoveAndDraw() 
    {
        while (true)
        {
            sumOfCharge = Vector3.zero; //Creates the used Vector3

            e0 = worldData.e0;
            constantChange = (float)(1 / (4 * e0 * Mathf.PI)); //First part of the formula

            Vector3 Cor = transform.position; //Position of the mooving point

            for (int i = 0; i < worldData.objectCount; i++) //Looks through every active pole and calculates the force of each one
            {
                ObjectAttributes chargedObject = worldData.chargedObjects[i];
                float charge = chargedObject.charge;

                Vector3 CorChar = chargedObject.transform.position;
                Vector3 CorDiff = Cor - CorChar;

                sumOfCharge += ((charge) / Mathf.Pow((Mathf.Pow(CorDiff.x, 2f) + Mathf.Pow(CorDiff.y, 2f) + Mathf.Pow(CorDiff.z, 2f)), 3f / 2f)) * new Vector3(CorDiff.x, CorDiff.y, CorDiff.z);
            }

            movementVector = sumOfCharge * Time.deltaTime * 30f; //Adjusts the speed of the point depending on the framerate

            //Makes the biggest part of the vector3 = 1, so that it doesn't get to big or small
            var Coordinates = new List<float> { Mathf.Abs(movementVector.x), Mathf.Abs(movementVector.y), Mathf.Abs(movementVector.z) };
            float maxValue = Mathf.Max(Coordinates.ToArray());
            if (maxValue == 0)
            {
                maxValue = 1 / 1000;
            }

            int position = Coordinates.IndexOf(maxValue);

            Vector3 balancesMovement = movementVector / Mathf.Abs(maxValue * 5);

            transform.position += balancesMovement; //Moves the point with the adjusted Vecor3

            lineCount++;

            //Checks if the point has reached any pole
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
                
            //Checks if the point has been move 2000 times, if yes then it stops the process
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
