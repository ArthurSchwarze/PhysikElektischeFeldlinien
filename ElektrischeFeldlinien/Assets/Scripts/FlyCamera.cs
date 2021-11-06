using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    //scipt for moving the camera, similar to unitys own camera movement withing the editor

    float mainSpeed = 10.0f; //Regular speed
    float shiftAdd = 100.0f; //Higher speed when shift is pressed
    float maxShift = 1000.0f; //Maximum speed when holdin shift
    float camSens = 0.24f; //Sensitivity of the mouse

    private Vector3 lastMouse = new Vector3(255, 255, 255); //Old position of the mouse
    private Vector3 lastMouseMove;
    private float totalRun = 1.0f;

    void Update()
    { 
        if (Input.GetMouseButtonDown(1)) //reset for right mouse button
        {
            lastMouse = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(2)) //reset for middle mouse button
        {
            lastMouseMove = Input.mousePosition;
        }

        if (Input.GetMouseButton(1)) //movement for rotation with the right mouse button
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        }
         
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        { // only moves while a direction key is pressed
            if (Input.GetKey(KeyCode.LeftShift)) // increases speed with shift pressed
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }
            p = p * Time.deltaTime;

            {
                transform.Translate(p);
            }
        }
    }

    //Moves camera according to the mouse when scrolling or mooving with the middle mouse button pressed
    private Vector3 GetBaseInput()
    { 
        Vector3 p_Velocity = new Vector3();
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            p_Velocity += new Vector3(0, 0, Input.mouseScrollDelta.y * 2);
        }

        if (Input.GetMouseButton(2))
        {
            lastMouseMove = Input.mousePosition - lastMouseMove;
            p_Velocity += 1.5f * new Vector3(-lastMouseMove.x * camSens, -lastMouseMove.y * camSens, 0);
        }

        lastMouseMove = Input.mousePosition;

        return p_Velocity;
    }
}
