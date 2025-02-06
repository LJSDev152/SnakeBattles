using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using System;
// Reference for Random
using Random = UnityEngine.Random;

public class RandomSpawn : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public SnakeTail SnakeTail;

    public List<GameObject> foodList = new List<GameObject>();
    public GameObject[] foodChoices = new GameObject[3];

    public int foodAmount;

    // SerializeField allows the value held in the variables below to be visible in the inspector
    // All set to private as not used in other scripts
    [SerializeField] private TextMeshProUGUI foodText;

    [SerializeField] private GameObject playerObj;

    private int choice;

    private Vector2 pos;

    private float plrDistance;
    private float foodDistance;
    [SerializeField] private float foodNearestDistance = 1000;
    [SerializeField] private float plrNearestDistance = 1000;

    // Built-in function: Called on first frame
    private void Start()
    {
        InitialiseSizeCounter();
    }

    // Built-in Function: Called every frame
    private void Update()
    {
        SpawnFood();
        IsFoodObjInCamView();
    }

    // Everything is stored as a method so they can be easily changed to public & be accessible to other scripts if needed, improving their flexibility, reusability & readability of the code

    private void SpawnFood()
    {
        // If the length of the list foodList is less than 10 then another object is spawned
        if (foodList.Count < 10)
        {
            RandomObjSpawn();
        }
    }

    private void IsFoodObjInCamView()
    {
        // Loops through all the items in foodList, checking if each index of an object is in view of the camera
        // This loop iterates backwards to prevent the issue of potentially skipping elements in the list when they are removed
        for (int i = foodList.Count - 1; i >= 0; i--)
        {
            // Makes sure their isn't a null object at the current index by removing it from the list before adding another object
            if (foodList[i] == null)
            {
                foodList.RemoveAt(i);
            }

            // Converts the position of the specified food object from its world position to a viewport point between 0 & 1
            Vector2 camPos = Camera.main.WorldToViewportPoint(foodList[i].transform.position);

            // Checks if the foodObject's position at a specified index is outside the range of the camera
            if (camPos.x < 0 || camPos.x > 1 || camPos.y < 0 || camPos.y > 1)
            {
                // The GameObject is destroyed first to avoid trying to remove a null object from the list, which would cause an error
                Destroy(foodList[i]);
                foodList.RemoveAt(i);
            }
        }
    }

    private void RandomObjSpawn()
    {
        // Creates 2 points that are 10 units away from the camera plane and will be 5% away from all the edges
        float x = Random.Range(0.05f, 0.95f);
        float y = Random.Range(0.05f, 0.95f);

        // Takes the 2 vectors & translates them to viewport points on the main camera
        pos = new Vector2(x, y);
        pos = Camera.main.ViewportToWorldPoint(pos);

        // Generates a random number between 1 & 3 which is used to decide which index in the array foodChoices will be chosen
        choice = Random.Range(0, 3);

        // Ensures the nearest distance is reset every time SpawnSpacing() is called to prevent a stack overflow from occurring in the case of having to generate another position
        foodNearestDistance = 1000;
        plrNearestDistance = 1000;

        // Handles the spacing of all food when it is spawned in
        SpawnSpacing();
    }

    private void SpawnSpacing()
    {
        // Spawn spacing for player

        for (int i = (SnakeTail.positions).Count - 1; i >= 0; i--)
        {
            // Gets the distance between the position of the current tail object being searched in the list to the random food position generated from RandomObjSpawn()
            plrDistance = Vector2.Distance(SnakeTail.positions[i], pos);

            // If the distance is less than the current nearest distance, the nearest distance is replaced with the current tail distance
            if (plrDistance < plrNearestDistance)
            {
                plrNearestDistance = plrDistance;
            }
        }

        // Spawn spacing for food

        for (int i = foodList.Count - 1; i >= 0; i--)
        {
            // Gets the distance between the position of the current food object being searched in the list to the random food position generated from RandomObjSpawn()
            foodDistance = Vector2.Distance(foodList[i].transform.position, pos);

            // If the distance is less than the current nearest distance, the nearest distance is replaced with the current food distance
            if (foodDistance < foodNearestDistance)
            {
                foodNearestDistance = foodDistance;
            }
        }

        // Confirmation of both conditions being met

        if ((plrNearestDistance > 2 && foodNearestDistance > 2))
        {
            // Generates a random object inside foodChoices at a random viewport position inside the view of the camera
            GameObject newObj = Instantiate(foodChoices[choice], pos, Quaternion.identity);
            // The instantiated object is added to the list to prevent trying to delete the prefab, which would cause an error
            foodList.Add(newObj);
        }

        // If the above conditions aren't met, a new position is generated
        else
        {
            RandomObjSpawn();
        }
    }

    private void InitialiseSizeCounter()
    {
        // Sets text of Size UI to 5 to create the illusion it starts at 5 when it starts at 0 & so it matches the starting size of the snake
        foodText.text = ("Size: 5");
        // Set in here as only way for it to be called on the first frame & guarantee the size GUI will start at 5
        foodAmount = 5;
    }

    // Updates the size counter UI's value by adding 1 each time the snake collides with a foodObject
    public void AddFoodToSizeCounter()
    {
        foodAmount++;
        foodText.text = ("Size: " + foodAmount.ToString());      
    }

    // Updates the size counter UI's value by subtracting 1 if the snake uses the speed boost for a predetermined amount of time
    public void RemoveFoodFromSizeCounter()
    {
        foodAmount--;
        foodText.text = ("Size: " + foodAmount.ToString());
    }
}