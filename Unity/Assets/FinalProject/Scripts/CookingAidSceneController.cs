using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class CookingAidSceneController : MonoBehaviour
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

    float waveSpeed = 3.0f;
    [SerializeField]
    Material glowingMaterial;
    [SerializeField]
    Material passiveMaterial;

    // From Heidi
    private AudioSource audio;
    [SerializeField]
    GameObject ARSessionOrigin;
    private ARTrackedImageManager ImageManager; // image tracking
    private ARTrackedImage trackedImage; // image tracking
    public GameObject QRCodeHand;

    // For Timer
    [SerializeField] TextMeshProUGUI CurrentTimer;
    [SerializeField] TextMeshProUGUI AverageTimer;
    float timer;
    float CurrentSeconds;
    float CurrentMinutes;
    float avg;
    float AverageSeconds;
    float AverageMinutes;
    List<float> CookingTimes = new List<float>();
    [SerializeField] GameObject BarsOverlay;
    float ratio;

    // to communicate with CollisionController Script
    public int[,] orderArray = new int[,] { { 1, 1, 1, 0, 0, 0, 1, 1, 2, 1},
                                            { 1, 1, 1, 0, 0, 0, 1, 1, 2, 1},
                                            { 1, 1, 0, 0, 0, 0, 0, 1, 0, 2},
                                            { 1, 1, 1, 0, 0, 0, 1, 0, 0, 3},
                                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                                            { 1, 1, 1, 0, 0, 1, 0, 0, 0, 1},
                                            { 1, 1, 0, 1, 1, 0, 0, 0, 0, 1},
                                            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 3},
                                            { 1, 1, 1, 0, 0, 2, 0, 0, 0, 2},
                                            { 1, 1, 0, 1, 1, 0, 0, 0, 0, 1},
                                            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 1},
                                            { 1, 1, 1, 0, 0, 0, 1, 0, 0, 3},
                                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 3},
                                            { 1, 1, 1, 0, 0, 1, 0, 0, 0, 3},
                                            { 1, 1, 0, 1, 1, 0, 0, 0, 0, 2},
                                            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 1},
                                            { 1, 1, 1, 0, 0, 2, 0, 0, 0, 2},
                                            { 1, 1, 0, 1, 1, 0, 0, 0, 0, 1},
                                            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 3},
                                            { 1, 1, 1, 0, 0, 0, 1, 0, 0, 3},
                                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 3},
                                            { 1, 1, 1, 0, 0, 1, 0, 0, 0, 2},
                                            { 1, 1, 0, 1, 1, 0, 0, 0, 0, 1},
                                            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 3},
                                            { 1, 1, 1, 0, 0, 0, 1, 0, 0, 1}};
    public int orderNumber = 0;
    int currentSum;

    // to display correct information in UI
    [SerializeField] GameObject pizza1TypeText;
    [SerializeField] GameObject ingredient1Text;
    [SerializeField] GameObject ingredient2Text;
    [SerializeField] GameObject ingredient3Text;
    [SerializeField] GameObject ingredient4Text;
    [SerializeField] GameObject ingredient5Text;
    [SerializeField] GameObject pizza2TypeText;
    [SerializeField] GameObject pizza3TypeText;
    [SerializeField] GameObject pizza4TypeText;
    [SerializeField] GameObject pizza6TypeText;
    int currentDisplayNumber = 0;
    List<string> ingredients = new List<string>();
    int ordersUnfulfilled;

    // to display ingredient countdowns
    [SerializeField] GameObject CheeseCountdown;
    [SerializeField] GameObject MarinaraSauceCountdown;
    [SerializeField] GameObject PepperoniCountdown;
    [SerializeField] GameObject ChickenCountdown;
    [SerializeField] GameObject BaconCountdown;
    [SerializeField] GameObject MushroomCountdown;
    [SerializeField] GameObject OnionCountdown;
    [SerializeField] GameObject PepperCountdown;
    [SerializeField] GameObject JalapenoCountdown;
    [SerializeField] GameObject PizzaCount;

    public int[] ingredientCountdownArray = new int[] { 50, 20, 50, 50, 50, 50, 50, 50, 50 };

    bool waited3Seconds = true;

    void Start()
    {
        ImageManager = ARSessionOrigin.GetComponent<ARTrackedImageManager>(); // image tracking
        audio = gameObject.GetComponent<AudioSource>();

        audio.Play();

        timer = 0;
    }

    void Update()
    {
        foreach (var thisTrackedImage in ImageManager.trackables)
        { // image tracking
            trackedImage = thisTrackedImage;
            if (trackedImage.referenceImage.name == "QRCodeHand")
            {
                //audio.Play();
                QRCodeHand.SetActive(true);
                QRCodeHand.transform.position = trackedImage.transform.position;
                QRCodeHand.transform.rotation = trackedImage.transform.rotation;
            }
        }
        if (waited3Seconds == true)
        {
            StartCoroutine(ImageTracking());
        }
        GlowControl();
        OrderCheck();
        CurrentTimerUpdate();
        AverageTimerUpdate();
        IngredientDisplay();
        CountdownTextUpdate();
    }

    IEnumerator ImageTracking()
    {
        waited3Seconds = false;
        foreach (var thisTrackedImage in ImageManager.trackables)
        { // image tracking
            trackedImage = thisTrackedImage;

            if (trackedImage.referenceImage.name == "FoodAssemblyStation")
            {
                //audio.Play();
                FoodAssemblyObjects.SetActive(true);
                FoodAssemblyObjects.transform.position = trackedImage.transform.position;
                //FoodAssemblyObjects.transform.position = new Vector3(trackedImage.transform.position.x, trackedImage.transform.position.y, trackedImage.transform.position.z - 0.15f);
                FoodAssemblyObjects.transform.rotation = trackedImage.transform.rotation;
            }
            //if (trackedImage.referenceImage.name == "QRCodeHand")
            //{
            //    audio.Play();
            //    QRCodeHand.SetActive(true);
            //    QRCodeHand.transform.position = trackedImage.transform.position;
            //    QRCodeHand.transform.rotation = trackedImage.transform.rotation;
            //}
            if (trackedImage.referenceImage.name == "Reset1")
            {
                //audio.Play();
                ingredientCountdownArray[0] = 50;
            }
            if (trackedImage.referenceImage.name == "reset2")
            {
                audio.Play();
                ingredientCountdownArray[1] = 20;
            }
            if (trackedImage.referenceImage.name == "Reset3")
            {
                //audio.Play();
                ingredientCountdownArray[2] = 50;
            }
            if (trackedImage.referenceImage.name == "Reset4")
            {
                //audio.Play();
                ingredientCountdownArray[3] = 50;
            }
            if (trackedImage.referenceImage.name == "Reset5")
            {
                //audio.Play();
                ingredientCountdownArray[4] = 50;
            }
            if (trackedImage.referenceImage.name == "Reset6")
            {
                //audio.Play();
                ingredientCountdownArray[5] = 50;
            }
            if (trackedImage.referenceImage.name == "Reset7")
            {
                //audio.Play();
                ingredientCountdownArray[6] = 50;
            }
            if (trackedImage.referenceImage.name == "Reset8")
            {
                //audio.Play();
                ingredientCountdownArray[7] = 50;
            }
            if (trackedImage.referenceImage.name == "Reset9")
            {
                //audio.Play();
                ingredientCountdownArray[8] = 50;
            }
        }
        yield return new WaitForSeconds(1);
        waited3Seconds = true;
    }
    
    void Glow(GameObject box, string ingredientName, int count)
    {
        box.GetComponent<MeshRenderer>().material = glowingMaterial;
        box.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        box.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0.8101695f, 1.0f, 0f, (Mathf.Sin(Time.time * waveSpeed) + 0.8f) * 0.5f));
        box.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = ingredientName + " x" + count.ToString();
    }

    public void AntiGlow(GameObject box)
    {
        box.GetComponent<MeshRenderer>().material = passiveMaterial;
        box.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "";
    }

    void GlowControl()
    {
        if (orderArray[orderNumber, 0] > 0)
        {
            Glow(Cheese, "Cheese" , orderArray[orderNumber, 0]);
        }
        else
        {
            AntiGlow(Cheese);
        }

        if (orderArray[orderNumber, 1] > 0)
        {
            Glow(MarinaraSauce, "Marinara Sauce", orderArray[orderNumber, 1]);
        }
        else
        {
            AntiGlow(MarinaraSauce);
        }

        if (orderArray[orderNumber, 2] > 0)
        {
            Glow(Pepperoni, "Pepperoni", orderArray[orderNumber, 2]);
        }
        else
        {
            AntiGlow(Pepperoni);
        }

        if (orderArray[orderNumber, 3] > 0)
        {
            Glow(Chicken, "Chicken", orderArray[orderNumber, 3]);
        }
        else
        {
            AntiGlow(Chicken);
        }

        if (orderArray[orderNumber, 4] > 0)
        {
            Glow(Bacon, "Bacon", orderArray[orderNumber, 4]);
        }
        else
        {
            AntiGlow(Bacon);
        }

        if (orderArray[orderNumber, 5] > 0)
        {
            Glow(Mushroom, "Mushroom", orderArray[orderNumber, 5]);
        }
        else
        {
            AntiGlow(Mushroom);
        }

        if (orderArray[orderNumber, 6] > 0)
        {
            Glow(Onion, "Onion", orderArray[orderNumber, 6]);
        }
        else
        {
            AntiGlow(Onion);
        }

        if (orderArray[orderNumber, 7] > 0)
        {
            Glow(Pepper, "Pepper", orderArray[orderNumber, 7]);
        }
        else
        {
            AntiGlow(Pepper);
        }

        if (orderArray[orderNumber, 8] > 0)
        {
            Glow(Jalapeno, "Jalapeno", orderArray[orderNumber, 8]);
        }
        else
        {
            AntiGlow(Jalapeno);
        }

    }

    public void OrderCheck()
    {
        currentSum = orderArray[orderNumber, 0] +
                       orderArray[orderNumber, 1] +
                       orderArray[orderNumber, 2] +
                       orderArray[orderNumber, 3] +
                       orderArray[orderNumber, 4] +
                       orderArray[orderNumber, 5] +
                       orderArray[orderNumber, 6] +
                       orderArray[orderNumber, 7] +
                       orderArray[orderNumber, 8];
        if (currentSum == 0)
        {
            orderNumber += 1;
        }
        ordersUnfulfilled = orderArray.GetLength(0) - orderNumber;
        if (orderArray.GetLength(0) > 4)
        {
            pizza6TypeText.GetComponent<TextMeshProUGUI>().text = (ordersUnfulfilled - 4).ToString();
        }
        else
        {
            pizza6TypeText.GetComponent<TextMeshProUGUI>().text = "";
        }
        PizzaCount.GetComponent<TextMeshProUGUI>().text = orderNumber.ToString();
    }

    void CurrentTimerUpdate()
    {
        timer += Time.deltaTime;
        CurrentSeconds = (int)(timer % 60);
        CurrentMinutes = (int)((timer / 60) % 60);

        CurrentTimer.text = CurrentMinutes.ToString("00") + ":" + CurrentSeconds.ToString("00");
    }

    void AverageTimerUpdate()
    {
        if (currentDisplayNumber == orderNumber)
        {
            CookingTimes.Add(timer);
            timer = 0;
        }

        if (CookingTimes.Count != 1)
        {
            avg = CookingTimes.Average();
            AverageSeconds = (int)(avg % 60);
            AverageMinutes = (int)((avg / 60) % 60);

            ratio = timer / avg;
            BarsOverlay.GetComponent<Image>().fillAmount = map(ratio, 2.0f, 0.25f, 0.0f, 0.8f);
            if (BarsOverlay.GetComponent<Image>().fillAmount < 0.04f)
            {
                BarsOverlay.GetComponent<Image>().fillAmount = 0.04f;
            }
            float redComponent = map(ratio, 0.25f, 1.0f, 0, 1);
            float greenComponent = map(ratio, 1.0f, 2.0f, 1, 0);
            Color barColor = new Color(redComponent, greenComponent, 0, 100);
            BarsOverlay.GetComponent<Image>().color = barColor;
        }
        else
        {
            avg = timer;
            AverageSeconds = CurrentSeconds;
            AverageMinutes = CurrentMinutes;
        }

        AverageTimer.text = AverageMinutes.ToString("00") + ":" + AverageSeconds.ToString("00");
    }

    void IngredientDisplay()
    {
        if (currentDisplayNumber == orderNumber)
        {
            // for item names
            if (orderArray.GetLength(0) > 0)
            {
                if (orderArray[orderNumber, 9] == 1)
                {
                    pizza1TypeText.GetComponent<TextMeshProUGUI>().text = "12inch Pizza";
                }
                else if (orderArray[orderNumber, 9] == 2)
                {
                    pizza1TypeText.GetComponent<TextMeshProUGUI>().text = "14inch Pizza";
                }
                else if (orderArray[orderNumber, 9] == 3)
                {
                    pizza1TypeText.GetComponent<TextMeshProUGUI>().text = "18inch Pizza";
                }
            }
            else
            {
                pizza1TypeText.GetComponent<TextMeshProUGUI>().text = "";
            }

            if (orderArray.GetLength(0) > 1)
            {
                if (orderArray[orderNumber + 1, 9] == 1)
                {
                    pizza2TypeText.GetComponent<TextMeshProUGUI>().text = "12inch Pizza";
                }
                else if (orderArray[orderNumber + 1, 9] == 2)
                {
                    pizza2TypeText.GetComponent<TextMeshProUGUI>().text = "14inch Pizza";
                }
                else if (orderArray[orderNumber + 1, 9] == 3)
                {
                    pizza2TypeText.GetComponent<TextMeshProUGUI>().text = "18inch Pizza";
                }
            }
            else
            {
                pizza2TypeText.GetComponent<TextMeshProUGUI>().text = "";
            }
            
            if (orderArray.GetLength(0) > 2)
            {
                if (orderArray[orderNumber + 2, 9] == 1)
                {
                    pizza3TypeText.GetComponent<TextMeshProUGUI>().text = "12inch Pizza";
                }
                if (orderArray[orderNumber + 2, 9] == 2)
                {
                    pizza3TypeText.GetComponent<TextMeshProUGUI>().text = "14inch Pizza";
                }
                if (orderArray[orderNumber + 2, 9] == 3)
                {
                    pizza3TypeText.GetComponent<TextMeshProUGUI>().text = "18inch Pizza";
                }
            }
            else
            {
                pizza3TypeText.GetComponent<TextMeshProUGUI>().text = "";
            }
            
            if (orderArray.GetLength(0) > 3)
            {
                if (orderArray[orderNumber + 3, 9] == 1)
                {
                    pizza4TypeText.GetComponent<TextMeshProUGUI>().text = "12inch Pizza";
                }
                if (orderArray[orderNumber + 3, 9] == 2)
                {
                    pizza4TypeText.GetComponent<TextMeshProUGUI>().text = "14inch Pizza";
                }
                if (orderArray[orderNumber + 3, 9] == 3)
                {
                    pizza4TypeText.GetComponent<TextMeshProUGUI>().text = "18inch Pizza";
                }
            }
            else
            {
                pizza4TypeText.GetComponent<TextMeshProUGUI>().text = "";
            }

            // for ingredient names
            ingredients.Clear();
            if (orderArray[orderNumber, 0] > 0)
            {
                ingredients.Add("Cheese" + " x" + orderArray[orderNumber, 0].ToString());
            }
            if (orderArray[orderNumber, 1] > 0)
            {
                ingredients.Add("MarinaraSauce" + " x" + orderArray[orderNumber, 1].ToString());
            }
            if (orderArray[orderNumber, 2] > 0)
            {
                ingredients.Add("Pepperoni" + " x" + orderArray[orderNumber, 2].ToString());
            }
            if (orderArray[orderNumber, 3] > 0)
            {
                ingredients.Add("Chicken" + " x" + orderArray[orderNumber, 3].ToString());
            }
            if (orderArray[orderNumber, 4] > 0)
            {
                ingredients.Add("Bacon" + " x" + orderArray[orderNumber, 4].ToString());
            }
            if (orderArray[orderNumber, 5] > 0)
            {
                ingredients.Add("Mushroom" + " x" + orderArray[orderNumber, 5].ToString());
            }
            if (orderArray[orderNumber, 6] > 0)
            {
                ingredients.Add("Onion" + " x" + orderArray[orderNumber, 6].ToString());
            }
            if (orderArray[orderNumber, 7] > 0)
            {
                ingredients.Add("Pepper" + " x" + orderArray[orderNumber, 7].ToString());
            }
            if (orderArray[orderNumber, 8] > 0)
            {
                ingredients.Add("Jalapeno" + " x" + orderArray[orderNumber, 8].ToString());
            }
            currentDisplayNumber += 1;
        }
        if (ingredients.Count > 0)
        {
            ingredient1Text.GetComponent<TextMeshProUGUI>().text = ingredients[0];
        }
        else
        {
            ingredient1Text.GetComponent<TextMeshProUGUI>().text = " ";
        }

        if (ingredients.Count > 1)
        {
            ingredient2Text.GetComponent<TextMeshProUGUI>().text = ingredients[1];
        }
        else
        {
            ingredient2Text.GetComponent<TextMeshProUGUI>().text = " ";
        }

        if (ingredients.Count > 2)
        {
            ingredient3Text.GetComponent<TextMeshProUGUI>().text = ingredients[2];
        }
        else
        {
            ingredient3Text.GetComponent<TextMeshProUGUI>().text = " ";
        }

        if (ingredients.Count > 3)
        {
            ingredient4Text.GetComponent<TextMeshProUGUI>().text = ingredients[3];
        }
        else
        {
            ingredient4Text.GetComponent<TextMeshProUGUI>().text = " ";
        }

        if (ingredients.Count > 4)
        {
            ingredient5Text.GetComponent<TextMeshProUGUI>().text = ingredients[4];
        }
        else
        {
            ingredient5Text.GetComponent<TextMeshProUGUI>().text = " ";
        }
    }

    void CountdownTextUpdate()
    {
        CheeseCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[0].ToString();
        MarinaraSauceCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[1].ToString();
        PepperoniCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[2].ToString();
        ChickenCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[3].ToString();
        BaconCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[4].ToString();
        MushroomCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[5].ToString();
        OnionCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[6].ToString();
        PepperCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[7].ToString();
        JalapenoCountdown.GetComponent<TextMeshProUGUI>().text = ingredientCountdownArray[8].ToString();

        float ratio1 = (float)ingredientCountdownArray[0] / (float)50;
        float ratio2 = (float)ingredientCountdownArray[1] / (float)20;
        float ratio3 = (float)ingredientCountdownArray[2] / (float)50;
        float ratio4 = (float)ingredientCountdownArray[3] / (float)50;
        float ratio5 = (float)ingredientCountdownArray[4] / (float)50;
        float ratio6 = (float)ingredientCountdownArray[5] / (float)50;
        float ratio7 = (float)ingredientCountdownArray[6] / (float)50;
        float ratio8 = (float)ingredientCountdownArray[7] / (float)50;
        float ratio9 = (float)ingredientCountdownArray[8] / (float)50;

        CheeseCountdown.GetComponent<TextMeshProUGUI>().color = new Color (map(ratio1, 0.5f, 1, 1, 0), map(ratio1, 0, 0.5f, 0, 1), 0, 1);
        MarinaraSauceCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio2, 0.5f, 1, 1, 0), map(ratio2, 0, 0.5f, 0, 1), 0, 1);
        PepperoniCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio3, 0.5f, 1, 1, 0), map(ratio3, 0, 0.5f, 0, 1), 0, 1);
        ChickenCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio4, 0.5f, 1, 1, 0), map(ratio4, 0, 0.5f, 0, 1), 0, 1);
        BaconCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio5, 0.5f, 1, 1, 0), map(ratio5, 0, 0.5f, 0, 1), 0, 1);
        MushroomCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio6, 0.5f, 1, 1, 0), map(ratio6, 0, 0.5f, 0, 1), 0, 1);
        OnionCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio7, 0.5f, 1, 1, 0), map(ratio7, 0, 0.5f, 0, 1), 0, 1);
        PepperCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio8, 0.5f, 1, 1, 0), map(ratio8, 0, 0.5f, 0, 1), 0, 1);
        JalapenoCountdown.GetComponent<TextMeshProUGUI>().color = new Color(map(ratio9, 0.5f, 1, 1, 0), map(ratio9, 0, 0.5f, 0, 1), 0, 1);
    }

    private static float map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }


}