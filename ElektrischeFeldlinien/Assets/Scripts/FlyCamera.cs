using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.24f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private Vector3 lastMouseMove;
    private float totalRun = 1.0f;

    void Update()
    {
        //if (Input.mouseScrollDelta != Vector2.zero)

        if (Input.GetMouseButtonDown(1))
        {
            lastMouse = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(2))
        {
            lastMouseMove = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
            //Mouse  camera angle done. 
        }
         

        //Keyboard commands
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        { // only move while a direction key is pressed
            if (Input.GetKey(KeyCode.LeftShift))
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
            /*Vector3 newPosition = transform.position;
            if (Input.GetKey(KeyCode.Space))
            { //If player wants to move on X and Z axis only
                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            } 
            else*/
            {
                transform.Translate(p);
            }
        }
    }

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
