using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public SnakeTail SnakeTail;

    public float rotateSpeed = 360f;
    public float velocityX = 0f;

    // SerializeField allows the value held in the variables below to be visible in the inspector
    // All set to private as not used in other scripts
    [SerializeField] private GameObject moveObjScreen;
    [SerializeField] private GameObject foodTextObj;
    [SerializeField] private GameObject mouse;
    private Camera mainCam;

    private float playerSpeed = 3;
    private int moveType = 0;

    private int snakeLength;

    // Built-in function: Called on first frame
    private void Start()
    {
        // Pauses the game so nothing is moving until the game is started
        Time.timeScale = 0;
        // Intitialises the main camera
        mainCam = Camera.main;
    }

    // Built-in Function: Called every frame
    private void Update()
    {
        MovementType();        
        SpeedBoost();
    }

    // Everything is stored as a method so they can be easily changed to public & be accessible to other scripts if needed, improving their flexibility, reusability & readability of the code

    private void MovementType()
    {
        // Movement Modes - Used to determine which type of movement should be ran
        if (moveType == 1)
        {
            HorizontalMovement();
        }

        else if (moveType == 2)
        {
            FollowMousePosition();
        }
    }

    // HorizontalMenu & MouseMenu set to public so accessible through a built-in OnClick() function in the inspector for text buttons

    public void HorizontalMenu()
    {
        // Locks the cursor so it isn't visible & to prevent moving the mouse
        Cursor.lockState = CursorLockMode.Locked;
        // Disables the movement option screen
        moveObjScreen.SetActive(false);
        // Enables the size GUI
        foodTextObj.SetActive(true);
        // Resumes the game
        Time.timeScale = 1;
        // Used to let Update() know what moveType should be ran
        moveType = 1;
    }

    private void HorizontalMovement()
    {
        // Move Type - Allows you to use A and D keys to move
        velocityX = Input.GetAxisRaw("Horizontal");

        // Movement - Moves the snake in the direction of the translation at set speed
        transform.Translate(playerSpeed * Time.deltaTime * Vector2.up, Space.Self);

        // Rotation - Rotates the snake in the direction the user inputs at set speed
        transform.Rotate(rotateSpeed * Time.deltaTime * -velocityX * Vector3.forward);
    }

    public void MouseMenu()
    {
        // Makes cursor invisible as object is used to represent mouse position
        Cursor.visible = false;
        // Disables the movement option screen
        moveObjScreen.SetActive(false);
        // Enables the size GUI
        foodTextObj.SetActive(true);
        // Makes the object visible
        mouse.SetActive(true);
        // Resumes the game
        Time.timeScale = 1;
        // Used to let Update() know what moveType should be ran
        moveType = 2;
    }

    private void FollowMousePosition()
    {
        // Movement & Rotation - Moves & rotates the snake towards the position of the mouse at set speed
        transform.position = Vector2.MoveTowards(transform.position, GetWorldPosFromMouse(), playerSpeed * Time.deltaTime);
    }

    private Vector2 GetWorldPosFromMouse()
    {
        // Gets the position of the mouse relative to its global position, used by FollowMousePosition() which is called in Update()
        return (mainCam.ScreenToWorldPoint(Input.mousePosition));
    }

    private void SpeedBoost()
    {
        // Used to hold the total length of the list snakeTail in the script SnakeTail for decision making when using the speed boost function
        snakeLength = SnakeTail.snakeTail.Count;

        // Speed Boost - When the key LeftShift is held down the speed of the snake increases to 4.5 if the snakeLength is greater or equal to 5
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (snakeLength >= 5)
            {
                playerSpeed = 4.5f;
            }

            else
            {
                playerSpeed = 3f;
            }
        }

        // If LeftShift is lifted/ not held or snakeLength becomes less than 5, the speed is reset to 3
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerSpeed = 3f;
        }
    }
}