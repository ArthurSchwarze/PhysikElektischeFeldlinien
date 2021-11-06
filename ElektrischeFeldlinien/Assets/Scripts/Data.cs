using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    public double e0; //gloval variable of e0
    
    public int objectCount; //number of poles used at the moment
    public int sphereNumber = 24; //number of lines made per pole ar maximum
    public float lineThickness = 0.03f; //thickness of the lines for the line renderer

    public ObjectAttributes[] chargedObjects; //all poles
    public Material rayCastColour; //colour of the line renderer

    public int totalNormalLines; //number of lines going from positive to negative
    public int totalOppositeLines; //number of lines going from negative to positive 

    public bool secondPhaseStarted = false; //see if the return has happened
    public bool restartable = true; //see if you can restart the drawing of the lines
    public bool reajustLines = false; //see if you can redraw the lines

    public Material materialPositive; //material for positive poles
    public Material materialNegative; //material for negative poles

    void Start()
    {
        e0 = 8.85 * Mathf.Pow(10, -12); 

        chargedObjects = GameObject.FindObjectsOfType<ObjectAttributes>(); //finds all charged objects
        objectCount = chargedObjects.Length;

        GameObject.Find("InputCreation").gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("InputCreation").gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        //looks if you can start with drawing the lines back
        if (totalOppositeLines == 0 && secondPhaseStarted) 
        {
            restartable = true;
        }

        //finds all poles and eliminates those, that aren't active at the time
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

        //looks if it has to draw lines again (comming from the deletion script of the UI)
        if (reajustLines && transform.gameObject.GetComponent<CreateNewObject>().amountOfClones == objectCount)
        {
            reajustLines = false;
            transform.gameObject.GetComponent<StartDrawing>().StartDrawingLines();
        }
    }

    //called when lines need to be draw after creating the objects
    public void StartSystem()
    {
        secondPhaseStarted = false;

        //takes every pole and adds the script for drawing lines if they fit the criteria of having a positive charge
        foreach (ObjectAttributes sphere in chargedObjects)
        {
            float chargeSphere = sphere.GetComponent<ObjectAttributes>().charge;
            if (Mathf.Sign(chargeSphere) == 1 && chargeSphere != 0)
            {
                sphere.gameObject.AddComponent<CreatePointsOnSphere>();
            }
        }
    }

    //Same as StartSystem, but for negative charges
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
