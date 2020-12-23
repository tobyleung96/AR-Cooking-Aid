using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;

public class Ordering3 : MonoBehaviour
{   
    public ARSessionOrigin arSessionOrigin;
    public GameObject boardPrefab;
    public GameObject pizzaPrefab;
    public GameObject pepPrefab;
    public GameObject onionPrefab;
    public GameObject redpepperPrefab;
    public GameObject jalapenyoPrefab;
    public Text instruction;
    public GameObject OrderInfo; 
    public GameObject boxPrefab;
    
    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
    private ARRaycastManager arRaycastManager; 
    private bool planeReady; 
    private bool hasPizza;
    private float pizzaSize;
    // Start is called before the first frame update
    private List<GameObject> pepperoniList = new List<GameObject>();
    private List<GameObject> onionList = new List<GameObject>();
    private List<GameObject> redpepperList = new List<GameObject>();
    private List<GameObject> jalapenyoList = new List<GameObject>();
   
    private string pepAmount;
    private string onionsAmount;
    private string redpepperAmount;
    private string jalapenyoAmount;

    private AudioSource audio; // audio for debugging

    float waveSpeed = 2f;
    bool sinewave = false;
    GameObject pep;
    GameObject onion;
    GameObject redpep;
    GameObject jalapenyo;

    Color LabelOpacity;

    void Start()
    {
        arRaycastManager = arSessionOrigin.GetComponent<ARRaycastManager> ();
        planeReady = false;
        hasPizza = false;
        audio = gameObject.GetComponent<AudioSource>();
        pepAmount = "NONE";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sinewave == true)
        {
            pep.transform.GetChild(0).gameObject.transform.position = new Vector3 (pep.transform.GetChild(0).gameObject.transform.position.x, (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f, pep.transform.GetChild(0).gameObject.transform.position.z);
            onion.transform.GetChild(0).gameObject.transform.position = new Vector3 (onion.transform.GetChild(0).gameObject.transform.position.x, (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f, onion.transform.GetChild(0).gameObject.transform.position.z);
            redpep.transform.GetChild(0).gameObject.transform.position = new Vector3 (redpep.transform.GetChild(0).gameObject.transform.position.x, (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f, redpep.transform.GetChild(0).gameObject.transform.position.z);
            jalapenyo.transform.GetChild(0).gameObject.transform.position = new Vector3 (jalapenyo.transform.GetChild(0).gameObject.transform.position.x, (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f, jalapenyo.transform.GetChild(0).gameObject.transform.position.z);
        }


        if (Input.touchCount > 0)
        {            
            if(planeReady == false){
                PlacePlane();
            }
        }


        if (Input.touchCount == 1 && Input.GetTouch(0).phase ==  TouchPhase.Began) {
         Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
         RaycastHit hit;
         
        //  RaycastHit[] hits = Physics.RaycastAll(touchRay);
         
        //  Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
         if(Physics.Raycast(ray, out hit))
         {
            //  Debug.Log(hit.transform.name);
             if (hit.collider != null) {
                 if(hit.collider.gameObject.name == "TransparentPizza"){
                    PlacePizza();
                    hit.collider.gameObject.SetActive(false);
                    boardPrefab.transform.GetChild(6).gameObject.SetActive(true);
                    
                    pep = boardPrefab.transform.GetChild(2).gameObject;
                    onion = boardPrefab.transform.GetChild(3).gameObject;
                    redpep = boardPrefab.transform.GetChild(4).gameObject;
                    jalapenyo = boardPrefab.transform.GetChild(5).gameObject;
                    
                    pep.SetActive(true);
                    onion.SetActive(true);
                    redpep.SetActive(true);
                    jalapenyo.SetActive(true);

                    sinewave = true;
    
                    // Changing label opacity
                    // LabelOpacity.a = 0.5f;
                    // pep.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = LabelOpacity;
                    // onion.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = LabelOpacity;
                    // redpep.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = LabelOpacity;
                    // jalapenyo.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = LabelOpacity;
                    
                 }
                 else if(hit.collider.gameObject.name == "placePepperoni"){
                    PlacePepperoni();
                    if(pepAmount == "EXTRA"){
                        hit.collider.gameObject.SetActive(false);
                    }
                 }
                else if(hit.collider.gameObject.name == "placeOnions"){
                    PlaceOnions();
                    if(onionsAmount == "EXTRA"){
                        hit.collider.gameObject.SetActive(false);
                    }
                 }
                else if(hit.collider.gameObject.name == "placeRedPepper"){
                    PlaceRedPepper();
                    if(redpepperAmount == "EXTRA"){
                        hit.collider.gameObject.SetActive(false);
                    }
                 }

                else if(hit.collider.gameObject.name == "placeJalapenyo"){
                    PlaceJalapenyo();
                    if(jalapenyoAmount == "EXTRA"){
                        hit.collider.gameObject.SetActive(false);
                    }
                 }

                 else if(hit.collider.gameObject.name == "PlaceOrder"){
                    audio.Play();
                    SubmitOrder();
                 }
             }
         }
        }

        if(hasPizza){
            pizzaSize = pizzaPrefab.GetComponent<ShowSize>().GetSize();
            if (pizzaSize < 0.25)
            {
                OrderInfo.GetComponent<TextMesh>().text = "pizza: SMALL \npepperoni: " + pepAmount;
            } else if (pizzaSize > 0.35)
            {
                OrderInfo.GetComponent<TextMesh>().text = "pizza: LARGE \nnpepperoni: " + pepAmount;
            }
            else
            {
                OrderInfo.GetComponent<TextMesh>().text = "pizza: MEDIUM \nnpepperoni: " + pepAmount;
            }
        }
    }

    void PlacePlane(){
        var touch = Input.GetTouch(0);            

        if (touch.phase == TouchPhase.Ended)
        {  
            Debug.Log("touched");
            if(arRaycastManager.Raycast(touch.position, arRaycastHits, TrackableType.PlaneWithinPolygon))  {
                instruction.enabled = false;
                var pose = arRaycastHits[0].pose;
                // Instantiate(cubePrefab, pose.position, Quaternion.identity);
                boardPrefab = Instantiate(boardPrefab, pose.position, Quaternion.identity);
                planeReady = true;
                // boardPrefab.transform.LookAt(Camera.main.transform);
                OrderInfo = Instantiate(OrderInfo, boardPrefab.transform.position, boardPrefab.transform.rotation);
                // OrderInfo.transform.SetParent(boardPrefab.transform);
                OrderInfo.transform.position += OrderInfo.transform.up*0.2f;// OrderInfo.transform.LookAt(Camera.main.transform);
                OrderInfo.transform.position += OrderInfo.transform.forward*0.05f;// OrderInfo.transform.LookAt(Camera.main.transform);
                OrderInfo.GetComponent<TextMesh>().text = "Place the pizza";

            }
        }
        
        // OrderInfo.GetComponent<TextMesh>().text = "Place pizza on the board";
    }


    void PlacePizza(){
        if(!hasPizza){
            Vector3 pizzaposition = boardPrefab.transform.GetChild(1).position;
            pizzaPrefab = Instantiate(pizzaPrefab, pizzaposition, boardPrefab.transform.rotation);
            hasPizza = true;
            audio.Play();

            // OrderInfo.GetComponent<TextMesh>().text = "Place pepperoni on the board \n Drag to resize the Pizza \n Order total: 5$ (medium pizza no pepperoni)\n ";
            // pizzaSize = pizzaPrefab.GetComponent<ShowSize>().GetSize();
            // OrderInfo.GetComponent<TextMesh>().text = pizzaSize.ToString("F2") + 'm';

        }

    }

    void PlacePepperoni(){
        if(hasPizza){
            audio.Play();
            // Vector3 PepperoniPosition = pizzaPrefab.transform.position;
            // PepperoniPosition += pizzaPrefab.transform.up * 0.01f;
            // PepperoniPosition += pizzaPrefab.transform.right * Random.Range(-0.25f, 0.05f);
            // PepperoniPosition += pizzaPrefab.transform.forward * Random.Range(-0.10f, 0.15f);

            Vector3 centerpizza = pizzaPrefab.transform.GetChild(0).transform.position;
            
            centerpizza += pizzaPrefab.transform.up * 0.01f;
            Vector2 newposition = Random.insideUnitCircle * 0.1f;
            centerpizza.x = centerpizza.x + newposition.x;
            centerpizza.z = centerpizza.z + newposition.y;
            
            GameObject curPepPrefab =  Instantiate(pepPrefab, centerpizza, pizzaPrefab.transform.rotation);
            pepperoniList.Add(curPepPrefab);
            curPepPrefab.transform.SetParent(pizzaPrefab.transform);
        }

        if(pepperoniList.Count == 0){
            pepAmount = "NONE";
        }else if(pepperoniList.Count < 20){
            pepAmount = "FEW";
        }else{
            pepAmount = "EXTRA";
        }
    }

    void PlaceOnions(){
        if(hasPizza){
            audio.Play();
            Vector3 centerpizza = pizzaPrefab.transform.GetChild(0).transform.position;
            
            centerpizza += pizzaPrefab.transform.up * 0.01f;
            Vector2 newposition = Random.insideUnitCircle * 0.1f;
            centerpizza.x = centerpizza.x + newposition.x;
            centerpizza.z = centerpizza.z + newposition.y;
            
            GameObject curOnionPrefab =  Instantiate(onionPrefab, centerpizza, pizzaPrefab.transform.rotation);
            curOnionPrefab.transform.localScale = Vector3.one * Random.Range(0.001f, 0.004f);
            onionList.Add(curOnionPrefab);
            curOnionPrefab.transform.SetParent(pizzaPrefab.transform);
        }

        if(onionList.Count == 0){
            onionsAmount = "NONE";
        }else if(onionList.Count < 20){
            onionsAmount = "FEW";
        }else{
            onionsAmount = "EXTRA";
        }
    }

    void PlaceRedPepper(){
        if(hasPizza){
            audio.Play();
            Vector3 centerpizza = pizzaPrefab.transform.GetChild(0).transform.position;
            
            centerpizza += pizzaPrefab.transform.up * 0.012f;
            Vector2 newposition = Random.insideUnitCircle * 0.12f;
            centerpizza.x = centerpizza.x + newposition.x;
            centerpizza.z = centerpizza.z + newposition.y;
            GameObject curRedPepperPrefab =  Instantiate(redpepperPrefab, centerpizza, Quaternion.Euler(new Vector3 (pizzaPrefab.transform.rotation.x, Random.Range(0, 360), pizzaPrefab.transform.rotation.y)));
            curRedPepperPrefab.transform.localScale = Vector3.one * Random.Range(0.001f, 0.004f);
            redpepperList.Add(curRedPepperPrefab);
            curRedPepperPrefab.transform.SetParent(pizzaPrefab.transform);
        }

        if(redpepperList.Count == 0){
            redpepperAmount = "NONE";
        }else if(redpepperList.Count < 20){
            redpepperAmount = "FEW";
        }else{
            redpepperAmount = "EXTRA";
        }
    }

    void PlaceJalapenyo(){
        if(hasPizza){
            audio.Play();
            Vector3 centerpizza = pizzaPrefab.transform.GetChild(0).transform.position;
            
            centerpizza += pizzaPrefab.transform.up * 0.015f;
            Vector2 newposition = Random.insideUnitCircle * 0.1f;
            centerpizza.x = centerpizza.x + newposition.x;
            centerpizza.z = centerpizza.z + newposition.y;
            
            GameObject curJalapenyoPrefab =  Instantiate(jalapenyoPrefab, centerpizza, pizzaPrefab.transform.rotation);
            curJalapenyoPrefab.transform.localScale = Vector3.one * Random.Range(0.001f, 0.004f);
            jalapenyoList.Add(curJalapenyoPrefab);
            curJalapenyoPrefab.transform.SetParent(pizzaPrefab.transform);
        }

        if(jalapenyoList.Count == 0){
            jalapenyoAmount = "NONE";
        }else if(jalapenyoList.Count < 30){
            jalapenyoAmount = "FEW";
        }else{
            jalapenyoAmount = "EXTRA";
        }
    }

    void SubmitOrder(){
        boxPrefab =  Instantiate(boxPrefab, pizzaPrefab.transform.position, pizzaPrefab.transform.rotation);
        boxPrefab.transform.position += boxPrefab.transform.right * (-0.1f);
        boxPrefab.transform.position += boxPrefab.transform.up * (0.01f);
        
        pizzaPrefab.SetActive(false);
        pep.SetActive(false);
        onion.SetActive(false);
        redpep.SetActive(false);
        jalapenyo.SetActive(false);

        for(int i = pepperoniList.Count -1 ; i > -1; i--){
            Destroy(pepperoniList[i]);
            pepperoniList.RemoveAt(i);
        }
        hasPizza = false;
        OrderInfo.GetComponent<TextMesh>().text = "ORDER SUBMITTED!";
    }
}
