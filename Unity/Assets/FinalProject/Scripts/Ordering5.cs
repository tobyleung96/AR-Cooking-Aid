using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;

public class Ordering5 : MonoBehaviour
{   
    public ARSessionOrigin arSessionOrigin;
    public GameObject boardPrefab;
    public GameObject pizzaPrefab;
    public GameObject pepPrefab;
    // public Text instruction;
    public GameObject OrderInfo; 
    public GameObject boxPrefab;
    public GameObject onionPrefab;
    public GameObject redpepperPrefab;
    public GameObject jalapenyoPrefab;
    public GameObject opaqueBoard;
    public GameObject indicatorPrefab;

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
    private List<GameObject> onionList = new List<GameObject>();
    private List<GameObject> redpepperList = new List<GameObject>();
    private List<GameObject> jalapenyoList = new List<GameObject>();
    private string pepAmount;
    private string onionsAmount;
    private string redpepperAmount;
    private string jalapenyoAmount;    

    //animated button
    float waveSpeed = 2f;
    bool sinewave = false;
    GameObject pep;
    GameObject onion;
    GameObject redpep;
    GameObject jalapenyo;


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
        pepAmount = "NONE";  onionsAmount= "NONE"; redpepperAmount = "NONE"; jalapenyoAmount = "NONE"; 
        indicatorPrefab = Instantiate(indicatorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        indicatorPrefab.SetActive(false);


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

        checkOffScreen();

        //animated button
        if (sinewave == true)
        {
            pep.transform.GetChild(0).gameObject.transform.position 
                                                    = new Vector3 (pep.transform.GetChild(0).gameObject.transform.position.x, 
                                                    (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f + pep.transform.position.y, 
                                                    pep.transform.GetChild(0).gameObject.transform.position.z);
            onion.transform.GetChild(0).gameObject.transform.position 
                                                    = new Vector3 (onion.transform.GetChild(0).gameObject.transform.position.x, 
                                                    (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f + onion.transform.position.y, 
                                                    onion.transform.GetChild(0).gameObject.transform.position.z);
            redpep.transform.GetChild(0).gameObject.transform.position 
                                                    = new Vector3 (redpep.transform.GetChild(0).gameObject.transform.position.x, 
                                                    (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f + redpep.transform.position.y, 
                                                    redpep.transform.GetChild(0).gameObject.transform.position.z);
            jalapenyo.transform.GetChild(0).gameObject.transform.position 
                                                    = new Vector3 (jalapenyo.transform.GetChild(0).gameObject.transform.position.x, 
                                                    (Mathf.Sin(Time.time * waveSpeed) + 0.5f) * 0.01f + jalapenyo.transform.position.y, 
                                                    jalapenyo.transform.GetChild(0).gameObject.transform.position.z);
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
                 }
                 else if(hit.collider.gameObject.name == "placePepperoni"){
                    // hit.collider.gameObject.GetComponent<ExplosionEffect>().Explode();

                    PlacePepperoni();
                    if(pepAmount == "EXTRA"){
                        hit.collider.gameObject.SetActive(false);
                    }
                    // audio.Play();
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
                    hit.collider.gameObject.GetComponent<ExplosionEffect>().Explode();

                    audioBG.Stop();
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
                OrderInfo.transform.GetChild(3).gameObject.GetComponent<TextMeshPro>().text = "pizza: SMALL \npepperoni: " + pepAmount
                                                                                    + "\nonion: " + onionsAmount
                                                                                    + "\nredpepper: " + redpepperAmount
                                                                                    + "\njalapenyo:" + jalapenyoAmount;
            } else if (pizzaSize > 0.35)
            {
                // OrderInfo.transform.GetChild(3).gameObject.GetComponent<TextMeshPro>().text = "LARGE";

                OrderInfo.transform.GetChild(3).GetComponent<TextMeshPro>().text = "pizza: LARGE \nnpepperoni: "  + pepAmount
                                                                                    + "\nonion: " + onionsAmount
                                                                                    + "\nredpepper: " + redpepperAmount
                                                                                    + "\njalapeno: " + jalapenyoAmount;
            }
            else
            {
                // OrderInfo.transform.GetChild(3).gameObject.GetComponent<TextMeshPro>().text = "MEDIUM";

                OrderInfo.transform.GetChild(3).GetComponent<TextMeshPro>().text = "pizza: MEDIUM \nnpepperoni: "  + pepAmount
                                                                                    + "\nonion: " + onionsAmount
                                                                                    + "\nredpepper: " + redpepperAmount
                                                                                    + "\njalapeno: " + jalapenyoAmount;
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

    //check if the board is off screem, show relevant mark
    void checkOffScreen(){
        if(planeReady){
                Vector3 viewPos = Camera.main.WorldToViewportPoint(boardPrefab.transform.position);
            // if(!boardPrefab.GetComponent<Renderer>().isVisible)
                if((viewPos.x*viewPos.y>1.0f)||(viewPos.x*viewPos.y<0)){
                    // audio.PlayOneShot(clicked);
                    indicatorPrefab.SetActive(true);
                    Vector3 markerPos = new Vector3(viewPos.x,viewPos.y, 1f);
                    if(viewPos.y>1){
                        markerPos.y = 0.95f;
                    }else if(viewPos.y<0){
                        markerPos.y = 0.05f;
                    }
                    if(viewPos.x>1){
                        markerPos.x = 0.95f;
                    }else if(viewPos.x<0){
                        markerPos.x = 0.05f;
                    }

                    indicatorPrefab.transform.position = Camera.main.ViewportToWorldPoint(markerPos);
                    indicatorPrefab.transform.LookAt(Camera.main.transform);
                }else{
                    indicatorPrefab.SetActive(false);
                }
        }else{
            indicatorPrefab.SetActive(false);

        }

    }


    /** Opaque board on the plane where the pointer points
    **/
    void MarkPlane(){
        arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), arRaycastHits, TrackableType.PlaneWithinPolygon);

        // if we hit an AR plane surface, update the position and rotation
        if(arRaycastHits.Count > 0)
        {
            opaqueBoard.SetActive(true);
            var pose = arRaycastHits[0].pose;
            opaqueBoard.transform.position = pose.position;
            opaqueBoard.transform.rotation = pose.rotation;

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
                // instruction.enabled = false;
                    // var pose = arRaycastHits[0].pose;
                    // Instantiate(cubePrefab, pose.position, Quaternion.identity);
                boardPrefab = Instantiate(boardPrefab, opaqueBoard.transform.position, opaqueBoard.transform.rotation);
                planeReady = true;
                OrderInfo = Instantiate(OrderInfo, boardPrefab.transform.position, boardPrefab.transform.rotation);
                OrderInfo.transform.position += OrderInfo.transform.up*0.2f;// OrderInfo.transform.LookAt(Camera.main.transform);
                OrderInfo.transform.position += OrderInfo.transform.forward*1f;// OrderInfo.transform.LookAt(Camera.main.transform);

            }
        }

    }

    public bool GetPlaneReady(){
        return planeReady;
    }


    void PlacePizza(){
        if(!hasPizza){
            Vector3 pizzaposition = boardPrefab.transform.GetChild(1).position;
            pizzaPrefab = Instantiate(pizzaPrefab, pizzaposition, boardPrefab.transform.rotation);
            hasPizza = true;
            // audio.Play();
            audio.PlayOneShot(phaseChange, 1.0f);


        }

    }


    void PlacePepperoni(){
        if(hasPizza){
            audio.PlayOneShot( clicked);

            Vector3 centerpizza = pizzaPrefab.transform.GetChild(0).transform.position;
            
            centerpizza += pizzaPrefab.transform.up * 0.015f;
            Vector2 newposition = Random.insideUnitCircle * 0.1f;
            centerpizza.x = centerpizza.x + newposition.x;
            centerpizza.z = centerpizza.z + newposition.y;
            
            // GameObject curOnionPrefab =  Instantiate(onionPrefab, centerpizza, pizzaPrefab.transform.rotation);
            // curOnionPrefab.transform.localScale = Vector3.one * Random.Range(0.001f, 0.004f);
            // onionList.Add(curOnionPrefab);
            // curOnionPrefab.transform.SetParent(pizzaPrefab.transform);

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
            audio.PlayOneShot( clicked);
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
            onionsAmount = "A FEW";
        }else{
            onionsAmount = "EXTRA";
        }
    }

    void PlaceRedPepper(){
        if(hasPizza){
            audio.PlayOneShot( clicked);
            Vector3 centerpizza = pizzaPrefab.transform.GetChild(0).transform.position;
            
            centerpizza += pizzaPrefab.transform.up * 0.012f;
            Vector2 newposition = Random.insideUnitCircle * 0.12f;
            centerpizza.x = centerpizza.x + newposition.x;
            centerpizza.z = centerpizza.z + newposition.y;
            
            GameObject curRedPepperPrefab =  Instantiate(redpepperPrefab, centerpizza, 
                                            Quaternion.Euler(new Vector3 (pizzaPrefab.transform.rotation.x, Random.Range(0, 360), pizzaPrefab.transform.rotation.y)));
            curRedPepperPrefab.transform.localScale = Vector3.one * Random.Range(0.001f, 0.004f);
            redpepperList.Add(curRedPepperPrefab);
            curRedPepperPrefab.transform.SetParent(pizzaPrefab.transform);
        }

        if(redpepperList.Count == 0){
            redpepperAmount = "NONE";
        }else if(redpepperList.Count < 20){
            redpepperAmount = "A FEW";
        }else{
            redpepperAmount = "EXTRA";
        }
    }

    void PlaceJalapenyo(){
        if(hasPizza){
            audio.PlayOneShot( clicked);
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
            jalapenyoAmount = "A FEW";
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
        
        // for(int i = pepperoniList.Count -1 ; i > -1; i--){
        //     Destroy(pepperoniList[i]);
        //     pepperoniList.RemoveAt(i);
        // }
        hasPizza = false;
        // OrderInfo.GetComponent<TextMesh>().text = "ORDER SUBMITTED!";

        audio.PlayOneShot(submitted);

    }
}
