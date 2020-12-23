using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


public class DeliverySceneUpdate : MonoBehaviour
{

    public ARSessionOrigin arSessionOrigin;    
    public GameObject cubePrefab;
    public GameObject linePrefab;
    public GameObject quad;
    private LineRenderer curve; //the bezeir curve
    private ARRaycastManager arRaycastManager;    

    //list of objects
    private List<GameObject> cubeList = new List<GameObject>();
    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
    private GameObject curCube;


    //for smooth damp
    private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 prevPosition;
    private float depth;
    

    //buttons
    public Button undoButton;
    public Button resetButton;
    public Button placeButton;
    public Slider mySlider;

    void Awake()
    {
        
    }

    void Start()
    {
        depth = 2.0f;
        curCube = Instantiate(cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * depth, Quaternion.identity);
        curve = gameObject.GetComponent<LineRenderer>();
        curve.positionCount = 50;
        linePrefab = Instantiate(linePrefab, transform.position, Quaternion.identity);

        //shadow initiate
        arRaycastManager = arSessionOrigin.GetComponent<ARRaycastManager> ();
        quad = Instantiate(quad, transform.position, quad.transform.rotation);
        quad.active = false;


        //button interaction
        undoButton.onClick.AddListener(Undo);
        resetButton.onClick.AddListener(Restart);
        placeButton.onClick.AddListener(PlaceCube);
        mySlider.value = depth/10f; //transfer slider value to 0m-10m

        
    }

    void Update()
    {
        depth = mySlider.value*10; 
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * depth;
        prevPosition = curCube.transform.position;
        // move the cube towards that target position
        curCube.transform.position = Vector3.SmoothDamp(curCube.transform.position, targetPosition, ref velocity, smoothTime);

        DrawCurve();

        // draw shadow
        Ray ray = new Ray(curCube.transform.position, transform.TransformDirection(Vector3.down));
        if(arRaycastManager.Raycast(ray, arRaycastHits, TrackableType.PlaneWithinPolygon)){
            // GameObject newCube = Instantiate(cubePrefab, curCube.transform.position, Quaternion.identity);
            quad.active = true;

            var pose = arRaycastHits[0].pose;
            quad.transform.position = pose.position;
            
        }else{
            quad.active = false;
        }

    }

    void Undo()
    {
        if(cubeList.Count>0){
            Destroy(cubeList[cubeList.Count-1]); // remove last cube
            cubeList.RemoveAt(cubeList.Count-1);
            linePrefab.GetComponent<DistanceCalculator>().removeLastPosition(); //remove text
        }
    }

    void Restart()
    {
        for(int i = cubeList.Count -1 ; i > -1; i--){
            Destroy(cubeList[i]);
            cubeList.RemoveAt(i);
        }
        cubeList.Clear();
        linePrefab.GetComponent<DistanceCalculator>().restart();
    }

    void PlaceCube(){
        GameObject newCube = Instantiate(cubePrefab, curCube.transform.position, Quaternion.identity);
        // GameObject newCube = Instantiate(quad, curCube.transform.position, Quaternion.identity); // for debug

        cubeList.Add(newCube);
        linePrefab.GetComponent<DistanceCalculator>().addPosition(newCube.transform.position); // add postion to visualizer

    }

    void DrawCurve()
    {  
        Vector3 p2 = curCube.transform.position;
        Vector3 p0 = Camera.main.transform.position 
                        -  Camera.main.transform.up*0.2f;
        //calculate p1 from p0 and p2;
        // Vector3 p1 = (p2 + p0) / 2 + (new Vector3(2,0,0)); //debug
        Vector3 p1 = (p2 + p0) / 2 + velocity*0.5f; // slow down the curve a little bit

        //create the curve
        for(int i = 0; i < 50; i++){
            float t = (i+1) / 50f;

            Vector3 curPoint = CalcBezier(t, p0, p1, p2);
            // quation of 3-point bezier cruve: B(t) = (1-t)^2*P0 + 2(1-t)t*P1 + t^2*P2 
            // Vector3 curPoint = p0+ t*(p2-p0);
            curve.SetPosition(i, curPoint);
        }

    }

    Vector3 CalcBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // quation of 3-point bezier cruve: B(t) = (1-t)^2*P0 + 2(1-t)t*P1 + t^2*P2 
        Vector3 bezeirPoint = (1-t)*(1-t)*p0 + 2*(1-t)*t*p1 + t*t*p2;
        return bezeirPoint;
    }


}
