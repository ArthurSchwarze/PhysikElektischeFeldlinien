using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllCreation : MonoBehaviour
{
    public InputField controllerCharge;
    public InputField controllerX;
    public InputField controllerY;
    public InputField controllerZ;
    
    public int textCoordinateX;
    public int textCoordinateY;
    public int textCoordinateZ;
    public int textCharge; 
    
    public int oldCoordinateX;
    public int oldCoordinateY;
    public int oldCoordinateZ;
    public int oldCharge;

    public GameObject objectSelf;
    private int numberOrder = 1;
    private Data worldData;

    // Start is called before the first frame update
    void Start()
    {
        worldData = GameObject.Find("World").GetComponent<Data>();

        controllerCharge = transform.GetChild(1).GetComponent<InputField>();
        controllerX = transform.GetChild(2).GetComponent<InputField>();
        controllerY = transform.GetChild(3).GetComponent<InputField>();
        controllerZ = transform.GetChild(4).GetComponent<InputField>();

        objectSelf = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        objectSelf.GetComponent<MeshRenderer>().enabled = false;
        objectSelf.AddComponent<ObjectAttributes>();
        objectSelf.GetComponent<ObjectAttributes>().enabled = false;

        oldCoordinateX = 1000;
        oldCoordinateY = 1000;
        oldCoordinateZ = 1000;
        oldCharge = 1000;

        objectSelf.GetComponent<ObjectAttributes>().charge = 1;
        transform.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        transform.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void ChangedVariable()
    {
        if (controllerX.text != "" && controllerY.text != "" && controllerZ.text != "" && controllerCharge.text != "")
        {
            StartCoroutine(WaitTime());
        }
    }

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
