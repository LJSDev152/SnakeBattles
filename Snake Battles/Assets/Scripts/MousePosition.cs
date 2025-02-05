using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    // Set to private as only used for this script
    private Camera mainCam;

    // Built-in function: Called on first frame
    private void Start()
    {
        // Intitialises the main camera
        mainCam = Camera.main;
    }

    // Built-in Function: Called every frame
    private void Update()
    {
        InitialiseMouse();
    }

    // Stored as a method so it can be easily changed to public & be accessible to other scripts if needed, improving the flexibility, reusability & readability of the code

    private void InitialiseMouse()
    {
        // Gets the position of the mouse relative to its global position, used by the object
        Vector2 mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        // Sets the object's position to the mouse position which is updated in the SnakeMovement script
        transform.position = mouseWorldPosition;
    }
}