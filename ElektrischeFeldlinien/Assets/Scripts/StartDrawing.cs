using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDrawing : MonoBehaviour
{
    //Used for drawing lines after new creation or while the poles are moving, when not static

    private Data worldData;
    private Transform startButton;
    private MoveParticle[] particles;
    private OtherMoveParticle[] otherParticles;
    private CreatePointsOnSphere[] forceObjects;
    private OtherCreatePointsOnSphere[] otherForceObjects;

    private ControllCreation[] createdObjects;

    public bool isMovingPoles = false;

    // Start is called before the first frame update
    void Start()
    {
        createdObjects = FindObjectsOfType<ControllCreation>();        
        worldData = GameObject.Find("World").GetComponent<Data>();
        startButton = GameObject.Find("StartProcess").transform.GetChild(0);
    }

    //Clears old lines and starts the new lines
    public void StartDrawingLines()
    {
        if (worldData.restartable)
        {
            ClearRun();

            worldData.StartSystem();
        }
        
    }

    //Clears lines for every existing line renderer
    private void ClearRun()
    {
        foreach (ObjectAttributes parent in worldData.chargedObjects)
        {
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(parent.GetComponent<CreatePointsOnSphere>());
            Destroy(parent.GetComponent<OtherCreatePointsOnSphere>());

            ObjectAttributes parentObject = parent.GetComponent<ObjectAttributes>();
            parentObject.nearPoints = new List<Vector3>();
        }
    }

    //Controlls the button for starting and stopping the moving of the poles
    public void StartMovingPoles()
    {
        if (!isMovingPoles)
        {
            isMovingPoles = true;
            startButton.GetComponent<Text>().text = "Stop Moving Poles";
            return;
        }

        startButton.GetComponent<Text>().text = "Move Poles";

        createdObjects = FindObjectsOfType<ControllCreation>();
        foreach (ControllCreation creation in createdObjects)
        {
            creation.GetComponent<ControllCreation>().ChangedVariable();
        }

        isMovingPoles = false;
    }

    //Initiates the drawing of the lines when the system was started
    private void Update()
    {
        if (isMovingPoles)
        {
            foreach (ObjectAttributes sphere in worldData.chargedObjects)
            {
                MoveSelf(sphere);
            }
            StartDrawingLines();
        }
    }

    //Movement when the poles aren't static
    private void MoveSelf(ObjectAttributes sphere) //Copies the movement from "MoveObject", just slower for performance
    {
        Vector3 sumOfCharge = Vector3.zero;

        double e0 = worldData.e0;
        float constantChange = (float)(1 / (4 * e0 * Mathf.PI));

        Vector3 Cor = sphere.transform.position;

        for (int i = 0; i < worldData.objectCount; i++)
        {
            ObjectAttributes chargedObject = worldData.chargedObjects[i];

            if (sphere.transform.position != chargedObject.transform.position)
            {
                float charge = chargedObject.charge;

                Vector3 CorChar = chargedObject.transform.position;
                Vector3 CorDiff = Cor - CorChar;

                sumOfCharge += ((charge) / Mathf.Pow((Mathf.Pow(CorDiff.x, 2f) + Mathf.Pow(CorDiff.y, 2f) + Mathf.Pow(CorDiff.z, 2f)), 3f / 2f)) * new Vector3(CorDiff.x, CorDiff.y, CorDiff.z);
            }
        }

        float personalCharge = sphere.GetComponent<ObjectAttributes>().charge;
        Vector3 movementVector = sumOfCharge * Time.deltaTime * 100f * personalCharge;

        sphere.transform.position += movementVector;
    }
}
