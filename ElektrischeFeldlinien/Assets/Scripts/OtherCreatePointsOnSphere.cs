using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCreatePointsOnSphere : MonoBehaviour
{
    //Mirrors the "CreatePointsOnSphere, only that the Movement Vector is inverted
    //Used for the returning lines

    public Material raycastColour;
    public float lineThickness;

    public int pointsOnSphere;
    private Data worldData;

    void Start()
    {
        //Same things as "CreatePointsOnSphere"
        worldData = GameObject.Find("World").GetComponent<Data>();
        pointsOnSphere = worldData.sphereNumber;
        lineThickness = worldData.lineThickness;
        raycastColour = worldData.rayCastColour;

        worldData.totalOppositeLines += pointsOnSphere;

        float scaling = 0.5f;
        Vector3[] pts = PointsOnSphere(pointsOnSphere);
        List<GameObject> uspheres = new List<GameObject>();
        int i = 0;
        Vector3[] possibleNearPoints = transform.GetComponent<ObjectAttributes>().nearPoints.ToArray();

        foreach (Vector3 value in pts)
        {
            List<float> distancePoints = new List<float>();
            float smallestDistance;

            uspheres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            uspheres[i].transform.parent = transform;
            uspheres[i].transform.position = value * scaling + transform.position;

            Vector3 position = uspheres[i].transform.position; // position of created object
            foreach (Vector3 vector in possibleNearPoints) //Checks for near points, where we already have a line
            {
                float distance = Mathf.Abs((vector - position).sqrMagnitude);
                distancePoints.Add(distance);
            }

            smallestDistance = Mathf.Min(distancePoints.ToArray());

            uspheres[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            
            if (smallestDistance > 0.06f) //Doesn't draw a line if there is one already in it's vicinity
            {
                uspheres[i].gameObject.AddComponent<LineRenderer>();
                uspheres[i].gameObject.AddComponent<OtherMoveParticle>();

                uspheres[i].gameObject.GetComponent<LineRenderer>().material = raycastColour;
                uspheres[i].gameObject.GetComponent<LineRenderer>().startWidth = lineThickness;
                
                uspheres[i].GetComponent<OtherMoveParticle>().smallestDistance = Mathf.Min(distancePoints.ToArray());

                uspheres[i].GetComponent<OtherMoveParticle>().MoveAndDraw();
            }
            else
            {
                Destroy(uspheres[i].gameObject);
                worldData.totalOppositeLines--;
            }
            
            i++;
        }
        worldData.secondPhaseStarted = true;
    }

    //Same thing as before
    Vector3[] PointsOnSphere(int n)
    {
        List<Vector3> upts = new List<Vector3>();
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2.0f / n;
        float x = 0;
        float y = 0;
        float z = 0;
        float r = 0;
        float phi = 0;

        for (var k = 0; k < n; k++)
        {
            y = k * off - 1 + (off / 2);
            r = Mathf.Sqrt(1 - y * y);
            phi = k * inc;
            x = Mathf.Cos(phi) * r;
            z = Mathf.Sin(phi) * r;

            upts.Add(new Vector3(x, y, z));
        }
        Vector3[] pts = upts.ToArray();
        return pts;
    }
}
