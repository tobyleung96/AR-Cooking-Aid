using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject FoodAssemblyObjects;
    public GameObject Cheese;
    public GameObject MarinaraSauce;
    public GameObject Pepperoni;
    public GameObject Chicken;
    public GameObject Bacon;
    public GameObject Mushroom;
    public GameObject Onion;
    public GameObject Pepper;
    public GameObject Jalapeno;
    public GameObject SceneController;

    //public int[] order1FromController;
    public int[,] orderArrayFromController;

    int orderNumberFromController = 0;

    public int[] ingredientCountdownArrayFromController;


    // Start is called before the first frame update
    void Start()
    {
        //order1FromController = SceneController.GetComponent<CookingAidSceneController>().order1;
        orderArrayFromController = SceneController.GetComponent<CookingAidSceneController>().orderArray;
        ingredientCountdownArrayFromController = SceneController.GetComponent<CookingAidSceneController>().ingredientCountdownArray;
    }

    // Update is called once per frame
    void Update()
    {
        orderNumberFromController = SceneController.GetComponent<CookingAidSceneController>().orderNumber;
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("CollisionDetected");
        if (collider.gameObject.name == "Cheese")
        {
            int ingredientCounter = 0;
            //Debug.Log("Collided with Cheese");
            //Debug.Log(orderArrayFromController[orderNumberFromController, ingredientCounter]);
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                //Debug.Log("inside if statement");
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Cheese);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
                
            }
        }
        if (collider.gameObject.name == "MarinaraSauce")
        {
            int ingredientCounter = 1;
            
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(MarinaraSauce);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
        if (collider.gameObject.name == "Pepperoni")
        {
            int ingredientCounter = 2;
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Pepperoni);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
        if (collider.gameObject.name == "Chicken")
        {
            int ingredientCounter = 3;
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Chicken);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
        if (collider.gameObject.name == "Bacon")
        {
            int ingredientCounter = 4;
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Bacon);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
        if (collider.gameObject.name == "Mushroom")
        {
            int ingredientCounter = 5;
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Mushroom);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
        if (collider.gameObject.name == "Onion")
        {
            int ingredientCounter = 6;
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Onion);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
        if (collider.gameObject.name == "Pepper")
        {
            int ingredientCounter = 7;
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Pepper);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
        if (collider.gameObject.name == "Jalapeno")
        {
            int ingredientCounter = 8;
            if (orderArrayFromController[orderNumberFromController, ingredientCounter] > 0)
            {
                SceneController.GetComponent<CookingAidSceneController>().AntiGlow(Jalapeno);
                ingredientCountdownArrayFromController[ingredientCounter] = ingredientCountdownArrayFromController[ingredientCounter] - orderArrayFromController[orderNumberFromController, ingredientCounter];
                orderArrayFromController[orderNumberFromController, ingredientCounter] = 0;
            }
        }
    }
}
