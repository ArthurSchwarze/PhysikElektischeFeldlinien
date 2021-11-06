using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttributes : MonoBehaviour
{
    //Used for storing all data of the poles and for creating the returning lines

    private float scale;
    public float charge = 1;

    public List<Vector3> nearPoints = new List<Vector3>(); //All points that landed withing it's vicinity 

    private Data worldData;
    public bool returned = false;

    private Material materialPositive;
    private Material materialNegative;

    void Start()
    {
        //Pulls data from world.data
        worldData = GameObject.Find("World").GetComponent<Data>();

        materialNegative = worldData.materialNegative;
        materialPositive = worldData.materialPositive;
    }

    // Update is called once per frame
    void Update()
    {
        //Is used to adust the size of the pole according to it's charge
        scale = Mathf.Pow(Mathf.Abs(charge), 1f / 3f); //Radius is doubled when the charge is the 8thfold
        gameObject.transform.localScale = new Vector3(scale, scale, scale);

        //Changes the colour of positive and negative poles
        if (!returned && Mathf.Sign(charge) == -1)
        {
            returned = true;

            transform.gameObject.AddComponent<OtherCreatePointsOnSphere>();
        }

        if (Mathf.Sign(charge) == -1)
        {
            transform.gameObject.GetComponent<Renderer>().material = materialNegative;
        }
        else
        {
            transform.gameObject.GetComponent<Renderer>().material = materialPositive;
        }
    }

    //Collects all points in the vicinity
    public void collectPoints(Vector3 point)
    {
        nearPoints.Add(point);
    }

}
