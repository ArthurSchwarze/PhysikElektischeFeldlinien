using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttributes : MonoBehaviour
{
    // Start is called before the first frame update

    private float scale;
    public float charge = 1;

    public List<Vector3> nearPoints = new List<Vector3>();

    private Data worldData;
    public bool returned = false;

    private Material materialPositive;
    private Material materialNegative;

    void Start()
    {
        worldData = GameObject.Find("World").GetComponent<Data>();

        materialNegative = worldData.materialNegative;
        materialPositive = worldData.materialPositive;
    }

    // Update is called once per frame
    void Update()
    {
        scale = Mathf.Pow(Mathf.Abs(charge), 1f / 3f);
        gameObject.transform.localScale = new Vector3(scale, scale, scale);

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

    public void collectPoints(Vector3 point)
    {
        nearPoints.Add(point);
    }

}
