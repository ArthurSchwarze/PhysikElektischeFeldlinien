using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSystem : MonoBehaviour
{
    //Contolls the Startscrenn and the visibility of all other elements

    public void StartAllSystems()
    {
        //Makes the startscreen UI invisible
        CanvasGroup group = transform.GetComponent<CanvasGroup>(); 
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;

        //Makes all other UI objects visible
        CanvasGroup objectStart = GameObject.Find("NotOnStart").transform.GetComponent<CanvasGroup>();
        objectStart.alpha = 1;
        objectStart.interactable = true;
        objectStart.blocksRaycasts = true;
    }

    //Switches from full screen to window mode
    public void SwitchFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
