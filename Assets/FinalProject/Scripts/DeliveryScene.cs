using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class DeliveryScene : MonoBehaviour
{
    public ARSessionOrigin arSessionOrigin;
    private Vector3 pizzaBoxPosition;
    private ARRaycastManager arRaycastManager; 

    public GameObject pizzaPrefab;
    public GameObject linePrefab;
    public GameObject dotPrefab;

    //list of objects
    private List<GameObject> cubeList = new List<GameObject>();
    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

    //line
    private const float k_MovementSpeed = 3f;
    private Vector3 curPosition;
    private GameObject curLine;

    public Button placeButton;
    // public Button undoButton;
    public Button resetButton;


    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = arSessionOrigin.GetComponent<ARRaycastManager> ();
        placeButton.onClick.AddListener(PlacePizza);
        resetButton.onClick.AddListener(Reset);
        curLine = Instantiate(linePrefab, transform.position, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Handle Touch Events
        if (Input.touchCount > 0)
        {            
            var touch = Input.GetTouch(0);            

            if (touch.phase == TouchPhase.Ended)
            {  

                Debug.Log("touched");
                if(arRaycastManager.Raycast(touch.position, arRaycastHits, TrackableType.PlaneWithinPolygon))  {

                    var pose = arRaycastHits[0].pose;
                    // Instantiate(cubePrefab, pose.position, Quaternion.identity);
                    GameObject curCube = Instantiate(dotPrefab, pose.position, Quaternion.identity);
                    cubeList.Add(curCube);
                    curLine.GetComponent<DistanceCalculator>().addPosition(pose.position); // add postion to visualizer
                    //initiate text

                }
            }
        }
    }

    void PlacePizza(){
        List<float> scales = curLine.GetComponent<DistanceCalculator>().getScales();
        if(scales != null){
            pizzaPrefab = Instantiate(pizzaPrefab, cubeList[0].transform.position, Quaternion.identity); // for debug
            // pizzaPrefab.transform.localScale = new Vector3(scales[0], scales[1], scales[2]) ;
        }
        
        // Reset();
    }

    void Reset() {
        for(int i = cubeList.Count -1 ; i > -1; i--){
            Destroy(cubeList[i]);
            cubeList.RemoveAt(i);
        }
        cubeList.Clear();
        curLine.GetComponent<DistanceCalculator>().restart();
        // Destroy(linePrefab);
    }
}
