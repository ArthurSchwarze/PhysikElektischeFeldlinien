using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAllSystems()
    {
        CanvasGroup group = transform.GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;

        CanvasGroup objectStart = GameObject.Find("NotOnStart").transform.GetComponent<CanvasGroup>();
        objectStart.alpha = 1;
        objectStart.interactable = true;
        objectStart.blocksRaycasts = true;
    }
}
