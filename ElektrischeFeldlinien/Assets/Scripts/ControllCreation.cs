using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllCreation : MonoBehaviour
{
    //Input fiels for the position of the poles (x, y, z) and charge
    public InputField controllerCharge;
    public InputField controllerX;
    public InputField controllerY;
    public InputField controllerZ;
    
    //Saves for the data of the poles
    public int textCoordinateX;
    public int textCoordinateY;
    public int textCoordinateZ;
    public int textCharge; 
    
    //Old saves from previous runs, to check the difference
    public int oldCoordinateX;
    public int oldCoordinateY;
    public int oldCoordinateZ;
    public int oldCharge;

    public GameObject objectSelf; //The created GameObject
    private int numberOrder = 1; 
    private Data worldData; //Save of all data

    // Start is called before the first frame update
    void Start()
    {
        worldData = GameObject.Find("World").GetComponent<Data>();

        //Sets all inputs
        controllerCharge = transform.GetChild(1).GetComponent<InputField>();
        controllerX = transform.GetChild(2).GetComponent<InputField>();
        controllerY = transform.GetChild(3).GetComponent<InputField>();
        controllerZ = transform.GetChild(4).GetComponent<InputField>();

        //Creates the object that will be later activated coordinates and charge are present
        objectSelf = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        objectSelf.GetComponent<MeshRenderer>().enabled = false;
        objectSelf.AddComponent<ObjectAttributes>();
        objectSelf.GetComponent<ObjectAttributes>().enabled = false;

        //Sets comparable coordinates, otherwise these would be 0, because int can't be NaN
        oldCoordinateX = 1000;
        oldCoordinateY = 1000;
        oldCoordinateZ = 1000;
        oldCharge = 1000;

        //Makes the UI visible
        objectSelf.GetComponent<ObjectAttributes>().charge = 1;
        transform.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        transform.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks all the input fields and looks if they are withing in the defined space

        if (int.TryParse(controllerX.text, out textCoordinateX))
        {
            if (textCoordinateX != oldCoordinateX)
            {
                if (Mathf.Abs(textCoordinateX) > 100)
                {
                    textCoordinateX = (int)Mathf.Sign(textCoordinateX) * 100;
                    controllerX.text = textCoordinateX.ToString();
                }
                ChangedVariable();
                oldCoordinateX = textCoordinateX;
            }
        }

        if (int.TryParse(controllerY.text, out textCoordinateY))
        {
            if (textCoordinateY != oldCoordinateY)
            {
                if (Mathf.Abs(textCoordinateY) > 100)
                {
                    textCoordinateY = (int)Mathf.Sign(textCoordinateY) * 100;
                    controllerY.text = textCoordinateY.ToString();
                }
                ChangedVariable();
                oldCoordinateY = textCoordinateY;
            }
        }

        if (int.TryParse(controllerZ.text, out textCoordinateZ))
        {
            if (textCoordinateZ != oldCoordinateZ)
            {
                if (Mathf.Abs(textCoordinateZ) > 100)
                {
                    textCoordinateZ = (int)Mathf.Sign(textCoordinateZ) * 100;
                    controllerZ.text = textCoordinateZ.ToString();
                }
                ChangedVariable();
                oldCoordinateZ = textCoordinateZ;
            }
        }

        if (int.TryParse(controllerCharge.text, out textCharge))
        {
            if (textCharge != oldCharge && textCharge != 0)
            {
                if (Mathf.Abs(textCharge) > 16)
                {
                    textCharge = (int) Mathf.Sign(textCharge) * 16;
                    controllerCharge.text = textCharge.ToString();
                }
                ChangedVariable();
                oldCharge = textCharge;
            }
        }
    }

    //If every variable is set this starts the activation process
    public void ChangedVariable()
    {
        if (controllerX.text != "" && controllerY.text != "" && controllerZ.text != "" && controllerCharge.text != "")
        {
            StartCoroutine(WaitTime());
        }
    }

    //Waits for changes and then makes the object visible
    IEnumerator WaitTime()
    {
        objectSelf.GetComponent<ObjectAttributes>().enabled = true;

        objectSelf.name = "sphere" + numberOrder;
        objectSelf.GetComponent<ObjectAttributes>().charge = textCharge;
        objectSelf.transform.position = new Vector3(textCoordinateX, textCoordinateY, textCoordinateZ);

        objectSelf.GetComponent<MeshRenderer>().enabled = true;

        yield return new WaitForSecondsRealtime(0.01f);
        //yield return new WaitForSeconds(2f); 
        worldData.GetComponent<StartDrawing>().StartDrawingLines();
    }
}
