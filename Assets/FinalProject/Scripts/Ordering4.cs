using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;

public class Ordering4 : MonoBehaviour
{   
    public ARSessionOrigin arSessionOrigin;
    public GameObject boardPrefab;
    public GameObject pizzaPrefab;
    public GameObject pepPrefab;
    public Text instruction;
    public GameObject OrderInfo; 
    public GameObject boxPrefab;
    public GameObject opaqueBoard;

    public AudioClip bg1;
    public AudioClip phaseChange;
    public AudioClip clicked;
    public AudioClip submitted;


    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
    private ARRaycastManager arRaycastManager; 
    private bool planeReady; 
    private bool hasPizza;
    private float pizzaSize;
    // Start is called before the first frame update
    private List<GameObject> pepperoniList = new List<GameObject>();
    private string pepAmount;
    

    private AudioSource audio; // audio for debugging
    private AudioSource audioBG;

    void Start()
    {
        arRaycastManager = arSessionOrigin.GetComponent<ARRaycastManager> ();
        planeReady = false;
        hasPizza = false;
        audio = gameObject.GetComponent<AudioSource>();
        StartBG();
        opaqueBoard = Instantiate(opaqueBoard, new Vector3(0, 0, 0), Quaternion.identity);
        opaqueBoard.SetActive(false);
        pepAmount = "NONE";
        // audioBG.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        // if plane is not instantiated, then make the transparent board
        if(!planeReady){
            MarkPlane();
        }else{
            opaqueBoard.SetActive(false);
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
                 if(hit.collider.gameObject.name == "placePizzaCube"){
                    PlacePizza();
                    hit.collider.gameObject.GetComponent<ExplosionEffect>().Explode();

                    hit.collider.gameObject.SetActive(false);
                 }
                 else if(hit.collider.gameObject.name == "placePepCube"){
                    hit.collider.gameObject.GetComponent<ExplosionEffect>().Explode();

                    PlacePepperoni();
                    if(pepAmount == "EXTRA"){
                        hit.collider.gameObject.SetActive(false);
                    }
                    // audio.Play();
                 }
                 else if(hit.collider.gameObject.name == "UndoCube"){
                    hit.collider.gameObject.GetComponent<ExplosionEffect>().Explode();

                    audio.Play();
                    SubmitOrder();
                    hit.collider.gameObject.SetActive(false);

                 }
             }
         }
        }

        if(hasPizza){
            pizzaSize = pizzaPrefab.GetComponent<ShowSize>().GetSize();
            if (pizzaSize < 0.25)
            {
                OrderInfo.transform.GetChild(2).GetComponent<TextMeshPro>().text = "pizza: SMALL \npepperoni: " + pepAmount;
            } else if (pizzaSize > 0.35)
            {
                OrderInfo.GetComponent<TextMeshPro>().text = "pizza: LARGE \nnpepperoni: " + pepAmount;
            }
            else
            {
                OrderInfo.GetComponent<TextMeshPro>().text = "pizza: MEDIUM \nnpepperoni: " + pepAmount;
            }
        }

    }

    //start background music
    void StartBG(){
        audioBG = gameObject.AddComponent<AudioSource>();
        audioBG.clip = bg1;
        audioBG.loop = true;
        audioBG.Play();
    }


    /** Goal: Opaque board on the plane where the pointer points
    *   cast a ray from camera
    *   if find a plane 
            if board is not null
                instantiate a opaque board
            board.get position of the ray
        if not find a plane 
            opaqueBoard.setactive(false)
        if touched -> PlacePlane()
    *
    **/
    void MarkPlane(){
        //  List<ARRaycastHit> hits = new List<ARRaycastHit>();
        // if(!opaqueBoard.activeSelf){
        //     opaqueBoard.SetActive(true);
        // }

        arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), arRaycastHits, TrackableType.PlaneWithinPolygon);

        // if we hit an AR plane surface, update the position and rotation
        if(arRaycastHits.Count > 0)
        {
            opaqueBoard.SetActive(true);
            var pose = arRaycastHits[0].pose;
            opaqueBoard.transform.position = pose.position;
            opaqueBoard.transform.rotation = pose.rotation;

            // transform.position = arRaycastHits[0].pose.position;
            // transform.rotation = arRaycastHits[0].pose.rotation;

            // enable the visual if it's disabled
        }else{
            opaqueBoard.SetActive(false);

        }

    }

    //place the board on the scene
    void PlacePlane(){
        var touch = Input.GetTouch(0);            

        if (touch.phase == TouchPhase.Ended)
        {  
            if(opaqueBoard.activeSelf){

                // audioBG.Stop();
                audioBG.volume = 0.2f;
                audio.PlayOneShot(phaseChange, 1.0f);
                Debug.Log("touched");
                instruction.enabled = false;
                    // var pose = arRaycastHits[0].pose;
                    // Instantiate(cubePrefab, pose.position, Quaternion.identity);
                boardPrefab = Instantiate(boardPrefab, opaqueBoard.transform.position, opaqueBoard.transform.rotation);
                planeReady = true;
                // boardPrefab.transform.LookAt(Camera.main.transform);
                OrderInfo = Instantiate(OrderInfo, boardPrefab.transform.position, boardPrefab.transform.rotation);
                // OrderInfo.transform.SetParent(boardPrefab.transform);
                OrderInfo.transform.position += OrderInfo.transform.up*0.2f;// OrderInfo.transform.LookAt(Camera.main.transform);
                OrderInfo.transform.position += OrderInfo.transform.forward*0.05f;// OrderInfo.transform.LookAt(Camera.main.transform);
                OrderInfo.GetComponent<TextMesh>().text = "Place the pizza";
                // if(arRaycastManager.Raycast(touch.position, arRaycastHits, TrackableType.PlaneWithinPolygon))  {
                

                // }
            }
        }
        
        // OrderInfo.GetComponent<TextMesh>().text = "Place pizza on the board";
    }

    public bool GetPlaneReady(){
        return planeReady;
    }


    void PlacePizza(){
        if(!hasPizza){
            pizzaPrefab = Instantiate(pizzaPrefab, boardPrefab.transform.position, boardPrefab.transform.rotation);
            
            pizzaPrefab.transform.position += pizzaPrefab.transform.up*0.05f;
            pizzaPrefab.transform.position -= pizzaPrefab.transform.forward*0.06f;
            pizzaPrefab.transform.position += pizzaPrefab.transform.right*0.09f;
            hasPizza = true;
            // audio.Play();
            audio.PlayOneShot(phaseChange, 1.0f);



            // OrderInfo.GetComponent<TextMesh>().text = "Place pepperoni on the board \n Drag to resize the Pizza \n Order total: 5$ (medium pizza no pepperoni)\n ";

            // pizzaSize = pizzaPrefab.GetComponent<ShowSize>().GetSize();
            // OrderInfo.GetComponent<TextMesh>().text = pizzaSize.ToString("F2") + 'm';

        }

    }

    void PlacePepperoni(){
        if(hasPizza){
            audio.PlayOneShot( clicked);

            Vector3 PepperoniPosition = pizzaPrefab.transform.position;
            PepperoniPosition += pizzaPrefab.transform.up * 0.01f;
            PepperoniPosition += pizzaPrefab.transform.right * Random.Range(-0.25f, 0.05f);
            PepperoniPosition += pizzaPrefab.transform.forward * Random.Range(-0.10f, 0.15f);
            GameObject curPepPrefab =  Instantiate(pepPrefab, PepperoniPosition, pizzaPrefab.transform.rotation);
            pepperoniList.Add(curPepPrefab);
            curPepPrefab.transform.SetParent(pizzaPrefab.transform);
        }

        if(pepperoniList.Count == 0){
            pepAmount = "NONE";
        }else if(pepperoniList.Count < 12){
            pepAmount = "FEW";
        }else{
            pepAmount = "EXTRA";
        }

    }

    void SubmitOrder(){
        boxPrefab =  Instantiate(boxPrefab, pizzaPrefab.transform.position, pizzaPrefab.transform.rotation);
        boxPrefab.transform.position += boxPrefab.transform.right * (-0.1f);
        
        pizzaPrefab.SetActive(false);
        for(int i = pepperoniList.Count -1 ; i > -1; i--){
            Destroy(pepperoniList[i]);
            pepperoniList.RemoveAt(i);
        }
        hasPizza = false;
        OrderInfo.GetComponent<TextMesh>().text = "ORDER SUBMITTED!";

        audio.PlayOneShot(submitted);

    }
}
