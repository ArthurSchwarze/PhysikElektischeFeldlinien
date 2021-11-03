using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePointsOnSphere : MonoBehaviour
{
    public Material raycastColour;
    public float lineThickness;
    public int sphereNumber;

    private Data worldData;

    void Start()
    {
        worldData = GameObject.Find("World").GetComponent<Data>();
        sphereNumber = worldData.sphereNumber;
        lineThickness = worldData.lineThickness;
        raycastColour = worldData.rayCastColour;
        
        float scaling = 0.5f;
        Vector3[] pts = PointsOnSphere(sphereNumber);
        List<GameObject> uspheres = new List<GameObject>();
        int i = 0;

        foreach (Vector3 value in pts)
        {
            uspheres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            uspheres[i].transform.parent = transform;
            uspheres[i].transform.position = value * scaling + transform.position;
            uspheres[i].gameObject.AddComponent<LineRenderer>();
            uspheres[i].gameObject.AddComponent<MoveParticle>();

            uspheres[i].gameObject.GetComponent<LineRenderer>().material = raycastColour;
            uspheres[i].gameObject.GetComponent<LineRenderer>().startWidth = lineThickness;
            uspheres[i].gameObject.GetComponent<MoveParticle>().MoveAndDraw();
            i++;
        }

        worldData.totalNormalLines += sphereNumber;
        worldData.ResetObjectAtribute();
    }

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
