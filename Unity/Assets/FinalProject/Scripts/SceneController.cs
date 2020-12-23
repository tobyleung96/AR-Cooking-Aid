using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


public class SceneController : MonoBehaviour
{
    public GameObject ARSessionOrigin;
    public GameObject PepperoniInstantiate;
    // public GameObject TablePlane;

    // public List<GameObject> addedInstances = new List<GameObject>();  

    private Transform ImageTransform;
    private ARTrackedImageManager ImageManager; // image tracking
    private ARTrackedImage trackedImage; // image tracking

    [SerializeField]
    public GameObject Pizza;
    private AudioSource audio; // audio for debugging
    public GameObject Pepperoni;
    private Vector3 PepperoniPosition;

    public Button PepperoniButton, PizzaButton;
    public Text centerText;

    private bool pizzaReady;



    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(TablePlane, PepperoniPosition, Quaternion.identity);
        Pizza.SetActive(false);
        pizzaReady = false;
        ImageManager = ARSessionOrigin.GetComponent<ARTrackedImageManager>(); // image tracking
        audio = gameObject.GetComponent<AudioSource>();
        PepperoniButton.onClick.AddListener(AddPepperoni);
        PizzaButton.onClick.AddListener(PlacePizza);

    }

    // Update is called once per frame
    void Update()
    {
        // ImageTransform = ImageManager.GetComponent<ARTrackedImage>().transform;

        // Pizza.transform.SetParent(ImageTransform);
        if(trackedImage == null){
            // audio.Play();
            foreach (var thisTrackedImage in ImageManager.trackables){ // image tracking
                trackedImage = thisTrackedImage;
                // Pizza.SetActive(true);
                // Pizza.transform.SetParent(trackedImage.transform);
            }
        }




        // PizzaPosition = ARSessionOrigin.GetComponent<ARTrackedImageManager>().transform.position;
        // if (ARSessionOrigin.GetComponent<ARTrackedImageManager>() != null)
        // TrackingState state;
        // if (System.Enum.TryParse("Tracking", out state) 
        //     && state == ImageManager.GetComponent<ARTrackedImage>().trackingState)
        // // if(ImageManager.GetComponent<ARTrackedImage>().trackingState = "Tracking")
        // {
        //    // PizzaPosition = GameObject.Find("Pizza14").transform.position;
        //    ImageTransform = ImageManager.GetComponent<ARTrackedImage>().transform;
        //    Pizza.SetActive(true);
        // //    Pizza.transform = ImageTransform;
        //    Pizza.transform.SetParent(ImageTransform);
        //     audio.Play();
        // }else{
        //     audio.Stop();
        // }


    }

    bool HasImage(){
        bool hasImage = false;
        TrackingState state;
        if (System.Enum.TryParse("None", out state) 
            && state != trackedImage.trackingState){
                hasImage = true;
                audio.Play();

            }

        return hasImage;

    }

    void PlacePizza(){
        // audio.Play();
        if(HasImage()){
            Pizza.SetActive(true);
            Pizza.transform.position = trackedImage.transform.position;
            Pizza.transform.position -= Pizza.transform.up * 0.05f;

            Pizza.transform.rotation = trackedImage.transform.rotation;
            Pizza.transform.SetParent(trackedImage.transform); // image tracking
            pizzaReady = true;

            centerText.text = " Pizza added, please add ingredients ";

        }else{

        }

    }

    void AddPepperoni(){
        // PepperoniPosition = new Vector3 (Pizza.transform.position.x + Random.Range(-0.10f, 0.10f),
        //              Pizza.transform.position.y + 0.001f, 
        //              Pizza.transform.position.z + Random.Range(-0.10f, 0.10f));
        if(pizzaReady){
            audio.Play();

                PepperoniPosition = Pizza.transform.position;
                PepperoniPosition += Pizza.transform.up * 0.06f;
                PepperoniPosition += Pizza.transform.right * Random.Range(-0.25f, 0.05f);
                PepperoniPosition += Pizza.transform.forward * Random.Range(-0.10f, 0.15f);
                // PepperoniPosition = new Vector3 (Pizza.transform.position.x + Pizza.transform.up,
                //         Pizza.transform.position.y + 0.001f, 
                //         Pizza.transform.position.z + Random.Range(-0.10f, 0.10f));
                Pepperoni =  Instantiate(Pepperoni, PepperoniPosition, Pizza.transform.rotation);
                Pepperoni.transform.SetParent(trackedImage.transform);
        }

    }

}
