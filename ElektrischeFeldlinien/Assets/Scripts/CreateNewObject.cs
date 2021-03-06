using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewObject : MonoBehaviour
{
    //Creates new UI blocks that are used for creation more Spheres in the space

    private GameObject currentUIBlock;
    private GameObject canvasObject;
    public GameObject nextUIBlock;

    private List<GameObject> lastCreatedUIBlocks = new List<GameObject>();
    private CanvasGroup deleterGroup;
    private Data worldData;

    public int amountOfClones = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentUIBlock = GameObject.Find("InputCreation");
        canvasObject = GameObject.Find("Canvas");
        deleterGroup = GameObject.Find("Delete").GetComponent<CanvasGroup>();
        worldData = GameObject.Find("World").GetComponent<Data>();
    }
    
    //Creates new UI clones
    public void OnButtonPress()
    {
        if (amountOfClones == 10) //maximum spheres at once (set at 10)
        {
            return;
        }

        if (amountOfClones == 0)
        {
            deleterGroup.alpha = 1;
            deleterGroup.interactable = true;
            deleterGroup.blocksRaycasts = true;
        }

        amountOfClones++;
        nextUIBlock = Instantiate(currentUIBlock, canvasObject.transform);
        nextUIBlock.transform.position -= new Vector3(0, amountOfClones * 75, 0);
        nextUIBlock.AddComponent<ControllCreation>();

        lastCreatedUIBlocks.Add(nextUIBlock);

        deleterGroup.gameObject.transform.position -= new Vector3(0, 75, 0);
    }

    //Deletes the newest UI clone
    public void DestroyClone()
    {
        amountOfClones--;
        deleterGroup.gameObject.transform.position += new Vector3(0, 75, 0);
        Destroy(lastCreatedUIBlocks[amountOfClones]);

        Destroy(lastCreatedUIBlocks[amountOfClones].GetComponent<ControllCreation>().objectSelf);
        lastCreatedUIBlocks.Remove(lastCreatedUIBlocks[amountOfClones]);

        if (amountOfClones == 0)
        {
            deleterGroup.alpha = 0;
            deleterGroup.interactable = false;
            deleterGroup.blocksRaycasts = false;
        }
        else
        {
            worldData.GetComponent<Data>().reajustLines = true;
        }
    }
}
