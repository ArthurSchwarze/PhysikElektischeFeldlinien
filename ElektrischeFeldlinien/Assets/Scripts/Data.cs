using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    // Start is called before the first frame update
    public double e0;
    
    public int objectCount;
    public int sphereNumber = 24;
    public float lineThickness = 0.03f;

    public ObjectAttributes[] chargedObjects;
    public Material rayCastColour;

    public int totalNormalLines;
    public int totalOppositeLines;

    public bool secondPhaseStarted = false;
    public bool restartable = true;
    public bool reajustLines = false;

    public Material materialPositive;
    public Material materialNegative;

    void Start()
    {
        e0 = 8.85 * Mathf.Pow(10, -12);

        chargedObjects = GameObject.FindObjectsOfType<ObjectAttributes>();
        objectCount = chargedObjects.Length;

        GameObject.Find("InputCreation").gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("InputCreation").gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (totalOppositeLines == 0 && secondPhaseStarted)
        {
            restartable = true;
        }
        chargedObjects = GameObject.FindObjectsOfType<ObjectAttributes>();
        foreach (ObjectAttributes charged in chargedObjects)
        {
            if (charged.name != "sphere1")
            {
                List<ObjectAttributes> randoms = new List<ObjectAttributes>(chargedObjects);
                randoms.Remove(charged);
                chargedObjects = (randoms.ToArray());
            }
        }
        
        objectCount = chargedObjects.Length;

        if (reajustLines && transform.gameObject.GetComponent<CreateNewObject>().amountOfClones == objectCount)
        {
            reajustLines = false;
            transform.gameObject.GetComponent<StartDrawing>().StartDrawingLines();
        }
    }

    public void StartSystem()
    {
        secondPhaseStarted = false;

        foreach (ObjectAttributes sphere in chargedObjects)
        {
            float chargeSphere = sphere.GetComponent<ObjectAttributes>().charge;
            if (Mathf.Sign(chargeSphere) == 1 && chargeSphere != 0)
            {
                sphere.gameObject.AddComponent<CreatePointsOnSphere>();
            }
        }
    }

    public void ResetObjectAtribute()
    {
        foreach (ObjectAttributes sphere in chargedObjects)
        {
            if (sphere.returned && Mathf.Sign(sphere.charge) == -1)
            {
                sphere.gameObject.AddComponent<OtherCreatePointsOnSphere>();
            }
        }
    }
}
