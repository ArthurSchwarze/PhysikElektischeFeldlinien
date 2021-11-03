using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartDrawing : MonoBehaviour
{
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

    public void StartDrawingLines()
    {
        if (worldData.restartable)
        {
            ClearRun();

            worldData.StartSystem();
        }
        
    }

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

    private void MoveSelf(ObjectAttributes sphere)
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
